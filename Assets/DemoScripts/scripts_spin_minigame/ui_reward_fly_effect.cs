
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ui_reward_fly_effect : MonoBehaviour
{
    [SerializeField] private Image iconPrefab;
    [SerializeField] private RectTransform effectLayer;

    public void StartFlyEffect(Sprite sprite, Vector2 start, RectTransform target, int numIcons = 6)
    {
        // TODO: Fine Tune This

        for (int i = 0; i < numIcons; i++)
        {
            Image icon = Instantiate(iconPrefab, effectLayer);
            RectTransform rect = icon.rectTransform;

            icon.sprite = sprite;
            rect.anchoredPosition = start;
            rect.localScale = Vector3.zero;

            Vector2 burst_vector = start + Random.insideUnitCircle * 200f;

            Sequence seq = DOTween.Sequence();

            seq.Append(rect.DOAnchorPos(burst_vector, 0.5f).SetEase(Ease.OutSine));
            seq.Join(rect.DOScale(1.25f, 0.3f).SetEase(Ease.OutSine));

            seq.AppendInterval(i * 0.04f); // Each icon will have a slight delay before moving to the target

            seq.Append(rect.DOAnchorPos(effectLayer.InverseTransformPoint(target.position), 0.8f).SetEase(Ease.InOutSine));
            seq.Join(rect.DOScale(0.4f, 0.5f).SetDelay(0.2f));
            seq.Join(icon.DOFade(0f, 0.5f).SetDelay(0.2f));

            seq.OnComplete(() => Destroy(icon.gameObject));
        }
    }
}
