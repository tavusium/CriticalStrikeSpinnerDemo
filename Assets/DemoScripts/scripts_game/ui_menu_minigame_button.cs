using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ui_menu_minigame_button : MonoBehaviour
{

    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;
    [SerializeField] private AudioSource ref_audio_source;
    [SerializeField] private AudioClip sfx_button_click;
    [SerializeField] private int safe_per_rounds = 3;
    [SerializeField] private int super_per_rounds = 9;

    private Button ref_minigame_start_button;

    private void OnValidate()
    {
        if (ref_spin_minigame_manager == null)
            ref_spin_minigame_manager = GameObject.Find("spin_minigame_manager").GetComponent<spin_minigame_manager>();


        if (ref_audio_source == null)
            ref_audio_source = GameObject.Find("audiosource_sfx").GetComponent<AudioSource>();


    }

    private void Awake()
    {
        ref_minigame_start_button = GetComponent<Button>();
        ref_minigame_start_button.onClick.AddListener(OnClickStartButton);
    }


    private void OnClickStartButton()
    {
        ref_spin_minigame_manager.SetPeriod(safe_per_rounds, super_per_rounds);
        ref_audio_source.PlayOneShot(sfx_button_click);
        ref_spin_minigame_manager.HandleStartMinigame();
    }
}
