using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public float speed;

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (body != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                body.transform.localPosition += speed * Time.deltaTime * Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                body.transform.localPosition += speed * Time.deltaTime * Vector3.left;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                body.transform.localPosition += speed * Time.deltaTime * Vector3.back;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                body.transform.localPosition += speed * Time.deltaTime * Vector3.right;
            }
        }
    }
}
