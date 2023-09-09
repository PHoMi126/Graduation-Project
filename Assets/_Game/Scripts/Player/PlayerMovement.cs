using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk/Sprint")]
    [SerializeField] KeyCode forward = KeyCode.W;
    [SerializeField] KeyCode backward = KeyCode.S;
    [SerializeField] KeyCode left = KeyCode.A;
    [SerializeField] KeyCode right = KeyCode.D;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothDamp = 0.25f;

    [Header("Fall")]
    [SerializeField] float gravity = -12f;

    [Header("Jump")]
    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpMultiplier;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Crouch")]
    [SerializeField] float standHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] KeyCode crouchKey = KeyCode.C;

    [Header("Animation")]
    [SerializeField] AnimStates animStates;

    [Header("Player Speed")]
    [SerializeField] float forwardSpeed = 1f;
    [SerializeField] float notForwardSpeed = 0.5f;
    [SerializeField] float sprintSpeed = 4f;
    [SerializeField] float crouchSpeed = 1f;

    CharacterController player = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    float velocityY = 0f; //keep track downward speed
    float moveSpeed;
    bool isJumping;
    bool isSprinting;
    bool isCrouching;

    private void Start()
    {
        player = GetComponent<CharacterController>();
        moveSpeed = forwardSpeed;
        animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Jump();
        Sprint();
        Crouch();
    }

    private void Movement()
    {
        Vector2 targetDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); //avoid change in move speed when moving diagonally

        //changes a vector to desired goal overtime for smooth transition
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothDamp);

        if (player.isGrounded) //check if player touch the ground or not
            velocityY = 0f;
        //defind downward acceleration
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * moveSpeed + Vector3.up * velocityY;

        player.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
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
        animStates.ChangeAnim(AnimStates.AnimState.pistolIdle);
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(sprintKey) && !isSprinting)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }

        if (Input.GetKeyUp(sprintKey) && isSprinting)
        {
            isSprinting = false;
            moveSpeed = forwardSpeed;
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey) && !isCrouching)
        {
            isCrouching = true;
            player.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }

        if (Input.GetKeyUp(crouchKey) && isCrouching)
        {
            isCrouching = false;
            player.height = standHeight;
            moveSpeed = forwardSpeed;
        }
    }
}
