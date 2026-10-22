using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ui_menu_minigame_button : MonoBehaviour
{

    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;

    private Button ref_minigame_start_button;

    private void OnValidate()
    {
        if (ref_spin_minigame_manager == null)
        {
            ref_spin_minigame_manager = GameObject.Find("spin_minigame_manager").GetComponent<spin_minigame_manager>();
        }

    }

    private void Awake()
    {
        ref_minigame_start_button = GetComponent<Button>();
        ref_minigame_start_button.onClick.AddListener(OnClickStartButton);
    }


    private void OnClickStartButton()
    {
        ref_spin_minigame_manager.HandleStartMinigame();
    }
}
