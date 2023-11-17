using UnityEngine;

public class PlayerPOV : MonoBehaviour
{
    //[Header("Camera")]
    [SerializeField] private Transform playerCam = null;
    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField][Range(0f, 0.5f)] float mouseSmoothDamp = 0.025f;

    float cameraPitch = 0.0f; //keep track camera x rotation
    internal bool lockCursor = false; //hide cursor from screen

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private void Start()
    {
        MouseLock();
    }

    private void Update()
    {
        MouseLook();
    }

    private void MouseLock()
    {
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void MouseLook()
    {
        if (playerCam != null)
        {
            Vector2 targetMouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            //changes a vector to desired goal overtime for smooth transition
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothDamp);

            //Horizontal look
            transform.Rotate(currentMouseDelta.x * mouseSensitivity * Time.deltaTime * Vector3.up);

            //Vertical look
            cameraPitch -= currentMouseDelta.y * mouseSensitivity * Time.deltaTime; //avoid invert y axis
            cameraPitch = Mathf.Clamp(cameraPitch, -75f, 90f); //avoid camera flip upside down

            playerCam.localEulerAngles = Vector3.right * cameraPitch;
        }
    }
}
