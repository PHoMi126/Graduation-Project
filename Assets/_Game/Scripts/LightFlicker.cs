using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] float flickTimeDelay;
    bool isFlickering;
    void Update()
    {
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        gameObject.GetComponent<Light>().enabled = false;
        flickTimeDelay = Random.Range(0.01f, 0.5f);
        yield return new WaitForSeconds(flickTimeDelay);

        gameObject.GetComponent<Light>().enabled = true;
        flickTimeDelay = Random.Range(0.01f, 0.5f);
        yield return new WaitForSeconds(flickTimeDelay);
        isFlickering = false;
    }
}
