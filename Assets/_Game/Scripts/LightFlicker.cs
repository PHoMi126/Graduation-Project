using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] MeshRenderer lightMaterial;
    [SerializeField] List<Material> lightStateList;
    [SerializeField] float flickTimeDelay;
    bool isFlickering;

    private void Start()
    {
        LightState(0);
    }
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
        LightState(0);
        flickTimeDelay = Random.Range(0.01f, 0.5f);
        yield return new WaitForSeconds(flickTimeDelay);

        gameObject.GetComponent<Light>().enabled = true;
        LightState(1);
        flickTimeDelay = Random.Range(0.01f, 0.5f);
        yield return new WaitForSeconds(flickTimeDelay);
        isFlickering = false;
    }

    private void LightState(int lightID)
    {
        if (lightID < lightStateList.Count)
        {
            lightMaterial.material = lightStateList[lightID];
        }
    }
}
