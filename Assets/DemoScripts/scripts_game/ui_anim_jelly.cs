using DG.Tweening;
using UnityEngine;

public class ui_anim_jelly : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 20f;


    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }



}



