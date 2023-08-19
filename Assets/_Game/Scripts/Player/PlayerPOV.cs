using UnityEngine;

public class PlayerPOV : MonoBehaviour
{
    //First-person view
    public float senX;
    public float senY;
    public Transform orientation;
    float xRotate;
    float yRotate;

    private void Start()
    {
        MouseLock();
    }

    private void Update()
    {
        MouseInput();
    }

    private void MouseLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MouseInput()
    {
        //mouse input
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        yRotate += mouseX;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);

        //rotate cam
        transform.rotation = Quaternion.Euler(xRotate, yRotate, 0f);
        //rotate player
        orientation.rotation = Quaternion.Euler(0f, yRotate, 0f);
    }
}
