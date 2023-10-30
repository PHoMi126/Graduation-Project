using UnityEngine;

public class RayCastCheck : MonoBehaviour
{
    public LayerMask layerMask;
    RaycastHit hit;

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 8, Color.blue);
        }
    }
}
