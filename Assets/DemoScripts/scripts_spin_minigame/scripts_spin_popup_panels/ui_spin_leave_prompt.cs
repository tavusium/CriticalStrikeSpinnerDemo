using UnityEngine;
using UnityEngine.UI;

public class ui_spin_leave_prompt : MonoBehaviour
{
    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;
    [SerializeField] private Button ref_button_leave_claim;
    [SerializeField] private Button ref_button_leave_goback;


    private void OnValidate()
    {
        ref_button_leave_claim = GameObject.Find("UI_leave_prompt_button_claim").GetComponent<Button>();
        ref_button_leave_goback = GameObject.Find("UI_leave_prompt_button_goback").GetComponent<Button>();
    }



    private void Awake()
    {
        ref_button_leave_claim.onClick.AddListener(OnClaimClicked);
        ref_button_leave_goback.onClick.AddListener(OnGoBackClicked);
    }

    private void OnClaimClicked()
    {
        ref_spin_minigame_manager.HandleClaimRewardsAndExit();
    }

    private void OnGoBackClicked()
    {
        ref_spin_minigame_manager.HandleLeaveGoBack();
    }
}
