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



    private void OnValidate()
    {
        ref_button_loseview_giveup = GameObject.Find("UI_loseview_button_giveup").GetComponent<Button>();
        ref_button_loseview_revive_gold = GameObject.Find("UI_loseview_button_revive_gold").GetComponent<Button>();
        ref_button_loseview_revive_ad = GameObject.Find("UI_loseview_button_revive_ad").GetComponent<Button>();
        ref_text_loseview_revive_cost_text = GameObject.Find("UI_loseview_button_revive_gold_value").GetComponent<TMP_Text>();
    }



    private void Awake()
    {
        ref_button_loseview_giveup.onClick.AddListener(OnGiveUpClicked);
        ref_button_loseview_revive_gold.onClick.AddListener(OnReviveGoldClicked);
        ref_button_loseview_revive_ad.onClick.AddListener(OnReviveAdClicked);
        ref_text_loseview_revive_cost_text.text = ref_spin_minigame_manager
                                                    .CalculateReviveCost().ToString();

    }


    public void ShowLoseView()
    {
        gameObject.SetActive(true);
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
