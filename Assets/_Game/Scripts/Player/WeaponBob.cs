using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public PlayerAction movement; //Check if grounded

    [Header("Sway")]
    [SerializeField] float step = 0.01f;
    [SerializeField] float maxStepDistance = 0.06f; //Max distance from local origin
    Vector3 swayPos;

    [Header("Sway Rotation")]
    [SerializeField] float rotationStep = 4f;
    [SerializeField] float maxRotationStep = 5f; //Max rotation from local identity rotation
    Vector3 swayEulerRot; //Store value

    [SerializeField] float smooth = 10f;
    float smoothRot = 12f;

    [Header("Bobbing")]
    [SerializeField] float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); } //Getter for sin of curve
    float curveCos { get => Mathf.Cos(speedCurve); } //Getter for cos of curve

    [SerializeField] Vector3 travelLimit = Vector3.one * 0.025f; //Limit of travel from move input
    [SerializeField] Vector3 bobLimit = Vector3.one * 0.01f; // Bob travel limit over time
    Vector3 bobPosition;

    [SerializeField] float bobExaggeration;

    [Header("Bob Rotation")]
    [SerializeField] Vector3 multiplier;
    Vector3 bobEulerRotation;

    // Update is called once per frame
    void Update()
    {
        //Player input
        GetInput();

        //Movement and rotation components
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        //Apply all movement and rotation components
        CompositePositionRotation();
    }

    //Inputs
    Vector2 walkInput; //Keyboard
    Vector2 lookInput; //Mouse

    void GetInput()
    {
        walkInput.x = Input.GetAxis("Horizontal");
        walkInput.y = Input.GetAxis("Vertical");
        walkInput = walkInput.normalized;

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
    }


    void Sway()
    {
        Vector3 invertLook = lookInput * -step;
        //Clamps the value of a Float or an Integer between the given range
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook; //Stored value
    }

    void SwayRotation() //Roll, pitch, yaw change as result of moving the mouse
    {
        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);
        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x); //x, y, z
    }

    void CompositePositionRotation()
    {
        //Lerp: Find a point between 2 numbers          (a                        b)                       t
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        //Euler: x, y, z
        //Quaternion: x, y, z, w
        //Slerp: Create a rotation which smoothly interpolates between a to b
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset() // x, y, z change as result of wasd
    {
        //Generate sin and cos waves
        speedCurve += Time.deltaTime * (movement.player.isGrounded ? (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) * bobExaggeration : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (movement.player.isGrounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (Input.GetAxis("Vertical") * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    void BobRotation() //Roll, pitch, yaw change as result of wasd
    {
        /* Pitch */
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        /* Yaw */
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        /* Roll */
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }
}
