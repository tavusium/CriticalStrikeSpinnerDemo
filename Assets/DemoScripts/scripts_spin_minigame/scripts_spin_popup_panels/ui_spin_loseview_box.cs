using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui_spin_loseview_box : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;

    [SerializeField] private Button ref_button_loseview_giveup;
    [SerializeField] private Button ref_button_loseview_revive_gold;
    [SerializeField] private Button ref_button_loseview_revive_ad;

    [SerializeField] private TMP_Text ref_text_loseview_revive_cost_text;

    [SerializeField] private GameObject ref_loseview_text_box;
    [SerializeField] private GameObject ref_loseview_card;



    private void OnValidate()
    {
        ref_button_loseview_giveup = GameObject.Find("UI_loseview_button_giveup").GetComponent<Button>();
        ref_button_loseview_revive_gold = GameObject.Find("UI_loseview_button_revive_gold").GetComponent<Button>();
        ref_button_loseview_revive_ad = GameObject.Find("UI_loseview_button_revive_ad").GetComponent<Button>();
        ref_text_loseview_revive_cost_text = GameObject.Find("UI_loseview_button_revive_gold_value").GetComponent<TMP_Text>();
        ref_loseview_text_box = GameObject.Find("UI_loseview_text_box");
        ref_loseview_card = GameObject.Find("UI_loseview_center_image_box");
    }



    private void Awake()
    {
        ref_button_loseview_giveup.onClick.AddListener(OnGiveUpClicked);
        ref_button_loseview_revive_gold.onClick.AddListener(OnReviveGoldClicked);
        ref_button_loseview_revive_ad.onClick.AddListener(OnReviveAdClicked);
        ref_text_loseview_revive_cost_text.text = ref_spin_minigame_manager
                                                    .CalculateReviveCost().ToString();

    }

    private void AnimateShow()
    {
        GetComponent<Image>().DOFade(1f, 0.25f)
            .SetEase(Ease.Linear);

        ref_loseview_card.GetComponent<Image>().DOFade(1f, 0.10f)
            .SetEase(Ease.Linear);

        ref_loseview_text_box.transform.SetLocalPositionAndRotation(new Vector3(0, 1600, 0), Quaternion.identity);
        ref_loseview_text_box.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -120f, 0), 0.75f)
            .SetEase(Ease.OutSine);

        ref_loseview_card.transform.SetLocalPositionAndRotation(new Vector3(0, -1600f, 0), Quaternion.identity);
        ref_loseview_card.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -30f, 0), 0.15f)
            .SetEase(Ease.Linear);
    }

    public void AnimateHide()
    {
        GetComponent<Image>().DOFade(0f, 0.35f)
            .SetEase(Ease.Linear);

        ref_loseview_text_box.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 1600, 0), 0.75f)
            .SetEase(Ease.InSine);

        ref_loseview_card.GetComponent<Image>().DOFade(0f, 0.10f)
            .SetEase(Ease.Linear);
        ref_loseview_card.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -1600f, 0), 0.15f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                UpdateReviveCostText();
            });
    }



    public void ShowLoseView()
    {
        gameObject.SetActive(true);
        AnimateShow();
        UpdateReviveCostText();
    }


    public void UpdateReviveCostText()
    {
        ref_text_loseview_revive_cost_text.text = ref_spin_minigame_manager
                                                    .CalculateReviveCost().ToString();
    }

    private void OnGiveUpClicked()
    {
        ref_spin_minigame_manager.HandleGiveUp();
    }

    private void OnReviveGoldClicked()
    {
        ref_spin_minigame_manager.HandleReviveWithGold();
    }

    private void OnReviveAdClicked()
    {
        ref_spin_minigame_manager.HandleReviveWithAd();
    }

}
