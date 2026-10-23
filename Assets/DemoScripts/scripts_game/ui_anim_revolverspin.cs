using DG.Tweening;
using UnityEngine;

public class ui_anim_revolverspin : MonoBehaviour
{




    private void Start()
    {
        AnimateLoop();
    }


    private void AnimateLoop()
    {
        transform.DORotate(new Vector3(0, 0, 720), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine)
            .SetDelay(Random.Range(0.3f, 1.3f)) // Random delay to make the animation feel more dynamic
            .OnComplete(() =>
            {
                AnimateLoop(); // Repeat the animation
            });
    }

}



