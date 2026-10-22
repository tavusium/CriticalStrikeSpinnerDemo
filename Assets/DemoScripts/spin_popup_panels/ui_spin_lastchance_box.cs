using UnityEngine;
using UnityEngine.UI;

public class ui_spin_lastchance_box : MonoBehaviour
{
    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;
    [SerializeField] private Button ref_button_lastchance_lose;
    [SerializeField] private Button ref_button_lastchance_goback;


    private void OnValidate()
    {
        ref_button_lastchance_lose = GameObject.Find("UI_lastchance_button_lose").GetComponent<Button>();
        ref_button_lastchance_goback = GameObject.Find("UI_lastchance_button_goback").GetComponent<Button>();
    }



    private void Awake()
    {
        ref_button_lastchance_lose.onClick.AddListener(OnLoseClicked);
        ref_button_lastchance_goback.onClick.AddListener(OnGoBackClicked);
    }

    private void OnLoseClicked()
    {
        ref_spin_minigame_manager.HandleLastChanceLose();
    }

    private void OnGoBackClicked()
    {
        ref_spin_minigame_manager.HandleLastChanceGoBack();
    }


}
