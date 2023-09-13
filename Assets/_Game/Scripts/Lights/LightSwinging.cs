using DG.Tweening;
using UnityEngine;

public class LightSwinging : MonoBehaviour
{
    [SerializeField] Transform roofLight;
    [SerializeField] float cycleLength = 2f;
    // Start is called before the first frame update
    void Start()
    {
        roofLight.DORotate(new Vector3(15, 0, 0), cycleLength * 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}
