using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] Transform pMovement;
    [SerializeField] float climbSpeed = 0f;
    [SerializeField] PlayerMovement moveInput;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    bool stickToLadder;

    private void Start()
    {
        moveInput = GetComponent<PlayerMovement>();
        stickToLadder = false;
    }

    private void Update()
    {
        LadderMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            moveInput.enabled = false;
            stickToLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            moveInput.enabled = true;
            stickToLadder = false;
        }
    }

    private void LadderMove()
    {
        if (stickToLadder)
        {
            if (Input.GetKey(up))
            {
                pMovement.transform.position += climbSpeed * Time.deltaTime * Vector3.up;
            }

            if (Input.GetKey(down))
            {
                pMovement.transform.position += climbSpeed * Time.deltaTime * Vector3.down;
            }
        }
    }
}
