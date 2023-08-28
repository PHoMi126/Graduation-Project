using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float gravity = -12f;
    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpMultiplier;
    [SerializeField] KeyCode jumpKey;
    [SerializeField][Range(0f, 0.5f)] float moveSmoothDamp = 0.25f;

    CharacterController characterController = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    float velocityY = 0f; //keep track downward speed
    bool isJumping;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Jump();
    }

    private void Movement()
    {
        Vector2 targetDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); //avoid change in move speed when moving diagonally

        //changes a vector to desired goal overtime for smooth transition
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothDamp);

        if (characterController.isGrounded) //check if player touch the ground or not
            velocityY = 0f;
        //defind downward acceleration
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        characterController.Move(velocity * Time.deltaTime);
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
            characterController.Move(jumpForce * jumpMultiplier * Time.deltaTime * Vector3.up);
            timeInAir += Time.deltaTime;
            yield return null;
        }
        while (!characterController.isGrounded
                && characterController.collisionFlags != CollisionFlags.Above /* fall down if touch ceiling*/);
        isJumping = false;
    }
}
