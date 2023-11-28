using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Walk/Sprint")]
    [SerializeField] KeyCode forward = KeyCode.W;
    [SerializeField] KeyCode backward = KeyCode.S;
    [SerializeField] KeyCode left = KeyCode.A;
    [SerializeField] KeyCode right = KeyCode.D;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothDamp = 0.25f;
    internal float moveSpeed;

    [Header("Fall")]
    [SerializeField] float gravity = -12f;
    float velocityY = 0f; //keep track downward speed

    [Header("Jump")]
    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpMultiplier;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    internal bool isJumping;

    [Header("Crouch")]
    [SerializeField] float standHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] KeyCode crouchKey = KeyCode.C;
    private bool isCrouching;

    [Header("Player Speed Based on Stamina")]
    public StaminaBar stamBar;

    [Header("Flashlight")]
    public Light flashlight;
    [SerializeField] KeyCode lightKey = KeyCode.F;
    bool lightToggle;

    [Header("Weapon Bob")]
    [SerializeField] WeaponBob weaponBob;

    internal CharacterController player = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    private void Start()
    {
        player = GetComponent<CharacterController>();
        stamBar = GetComponent<StaminaBar>();
        flashlight.gameObject.SetActive(false);
        moveSpeed = stamBar.normalSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        Actions();

        if (Input.GetKeyDown(jumpKey) && player.isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(sprintKey))
        {
            StartSprint();
        }
        else
        {
            StopSprint();
        }

        Flashlight();

        Crouch();
    }

    private void Actions()
    {
        Vector2 targetDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); //avoid change in move speed when moving diagonally

        //changes a vector to desired goal overtime for smooth transition
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothDamp);

        if (player.isGrounded) velocityY = 0f;
        //defind downward acceleration
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * moveSpeed + Vector3.up * velocityY;

        player.Move(velocity * Time.deltaTime);
    }

    public void Jump() //Combine with Stamina Cost
    {
        if (stamBar.currentStam < stamBar.jumpCost)
            return;

        if (stamBar.currentStam >= (stamBar.maxStam * stamBar.jumpCost / stamBar.maxStam))
        {
            isJumping = true;
            stamBar.currentStam -= stamBar.jumpCost;
            stamBar.sprintSpeed -= stamBar.speedChangeOnJump;

            StartCoroutine(JumpEvent());
        }
    }

    IEnumerator JumpEvent()
    {
        float timeInAir = 0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir); //curve value
            player.Move(jumpForce * jumpMultiplier * Time.deltaTime * Vector3.up);
            timeInAir += Time.deltaTime;
            yield return null;
        }
        while (!player.isGrounded
                && player.collisionFlags != CollisionFlags.Above /* fall down if touch ceiling*/);
        isJumping = false;
    }

    public void StartSprint()
    {
        moveSpeed = stamBar.sprintSpeed;
        if (moveSpeed > stamBar.normalSpeed)
        {
            if (stamBar.currentStam > 0f)
            {
                stamBar.StamSprint();
            }
        }
    }

    public void StopSprint()
    {
        stamBar.isSprinting = false;
        moveSpeed = stamBar.normalSpeed;
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey) && !isCrouching)
        {
            isCrouching = true;
            player.height = crouchHeight;
            moveSpeed = stamBar.crouchSpeed;
        }

        if (Input.GetKeyUp(crouchKey) && isCrouching)
        {
            isCrouching = false;
            player.height = standHeight;
            moveSpeed = stamBar.normalSpeed;
        }
    }

    private void Flashlight()
    {
        if (Input.GetKeyDown(lightKey))
        {
            lightToggle = !lightToggle;
        }

        if (lightToggle)
        {
            flashlight.gameObject.SetActive(true);
        }
        else
        {
            flashlight.gameObject.SetActive(false);
        }
    }
}
