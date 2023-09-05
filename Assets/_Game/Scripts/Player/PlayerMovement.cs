using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk/Sprint")]
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 4f;
    [SerializeField] KeyCode sprintKey;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothDamp = 0.25f;

    [Header("Fall")]
    [SerializeField] float gravity = -12f;

    [Header("Jump")]
    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpMultiplier;
    [SerializeField] KeyCode jumpKey;

    [Header("Crouch")]
    [SerializeField] float standHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] KeyCode crouchKey;

    [Header("Animation")]
    [SerializeField] AnimStates animStates;

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
        moveSpeed = walkSpeed;
        animStates.ChangeAnim(AnimStates.AnimState.pistolPose);
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

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            animStates.ChangeAnim(AnimStates.AnimState.walk);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            animStates.ChangeAnim(AnimStates.AnimState.idle);
        }
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
            animStates.ChangeAnim(AnimStates.AnimState.jump);
            yield return null;
        }
        while (!player.isGrounded
                && player.collisionFlags != CollisionFlags.Above /* fall down if touch ceiling*/);
        isJumping = false;
        animStates.ChangeAnim(AnimStates.AnimState.idle);
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(sprintKey) && !isSprinting)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
            animStates.ChangeAnim(AnimStates.AnimState.sprint);
        }

        if (Input.GetKeyUp(sprintKey) && isSprinting)
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
            animStates.ChangeAnim(AnimStates.AnimState.idle);
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(crouchKey) && !isCrouching)
        {
            isCrouching = true;
            player.height = crouchHeight;
        }

        if (Input.GetKeyUp(crouchKey) && isCrouching)
        {
            isCrouching = false;
            player.height = standHeight;
        }
    }
}
