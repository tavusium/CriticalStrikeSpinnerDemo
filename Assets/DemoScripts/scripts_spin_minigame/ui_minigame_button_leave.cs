using UnityEngine;
using UnityEngine.UI;

public class ui_minigame_button_leave : MonoBehaviour
{
    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;

    [SerializeField] private Button ref_leave_button;

    private void OnValidate()
    {
        if (ref_leave_button == null)
        {
            ref_leave_button = GetComponent<Button>();
        }

        if (ref_spin_minigame_manager == null)
        {
            ref_spin_minigame_manager = GameObject.Find("spin_minigame_manager").GetComponent<spin_minigame_manager>();
        }

    }


    private void Start()
    {
        ref_leave_button.onClick.AddListener(OnClickLeaveButton);
    }

    private void OnClickLeaveButton()
    {
        Debug.Log("Leave button clicked.");
        ref_spin_minigame_manager.HandleClaimRewardsAndExit();
    }
}
