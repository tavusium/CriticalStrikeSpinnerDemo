using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Spin_Manager : MonoBehaviour
{
    [SerializeField]
    private spin_minigame_manager minigame_manager;

    [Header("Spin Settings")]
    [SerializeField] private float spin_duration = 2.3f;

    [Header("Appearance")]
    [SerializeField]
    private spin_wheel_zone_appearence wheel_sprites;

    [Header("Audio")]
    [SerializeField]
    private AudioSource spin_audio_source;

    [SerializeField]
    private AudioClip snd_barrel_spin;
    [SerializeField]
    private AudioClip snd_revolver_clicks;
    [SerializeField]
    private AudioClip snd_gunshot;
    [SerializeField]
    private AudioClip snd_gold_collect;

    private GameObject ref_wheel_collection;
    private GameObject ref_wheel_barrel;
    private spin_wheel_barrel ref_wheel_barrel_component;
    private TMP_Text ref_wheel_title_tmp;
    private TMP_Text ref_wheel_description_tmp;
    private Image ref_wheel_indicator_image;





    private Button ref_spin_button; // offload this to minigame manager



    private bool is_spinning = false;
    private int landed_index = 0;




    // Editor-only validation method to get the button references
    private void OnValidate()
    {
        ref_spin_button = GameObject.Find("UI_button_spin").GetComponent<Button>();
        ref_wheel_collection = GameObject.Find("UI_wheel_collection");
        ref_wheel_barrel = GameObject.Find("UI_wheel_barrel");
        ref_wheel_barrel_component = ref_wheel_barrel.GetComponent<spin_wheel_barrel>();
        ref_wheel_title_tmp = GameObject.Find("UI_wheel_title_value").GetComponent<TMP_Text>();
        ref_wheel_description_tmp = GameObject.Find("UI_wheel_description_value").GetComponent<TMP_Text>();
        ref_wheel_indicator_image = GameObject.Find("UI_wheel_indicator").GetComponent<Image>();

    }

    private void Awake()
    {
        ref_spin_button.onClick.AddListener(SpinWheel);
    }

    public void SetZoneType(SpinWheelZoneType zone_type, spin_wheel_zone_award_list awards)
    {
        // Update the wheel's appearance and data based on the zone type
        DisableSpinButton();
        ref_wheel_barrel_component.ReconstructBarrel(awards);

        spin_audio_source.PlayOneShot(snd_revolver_clicks, 0.5f);


        ref_wheel_collection.transform.DOLocalRotate(new Vector3(0, 180f, 0), 0.5f, RotateMode.FastBeyond360)
        .SetRelative()
        .SetEase(Ease.InSine)
        .OnComplete(() =>
        {
            SwitchWheelZoneUI(zone_type);

            ref_wheel_collection.transform.DOLocalRotate(new Vector3(0, 180f, 0), 0.5f, RotateMode.FastBeyond360)
            .SetRelative()
            .SetEase(Ease.OutSine)
            .OnComplete(() => EnableSpinButton());

        });



    }

    public void SpinWheel()
    {
        if (is_spinning) return;
        is_spinning = true;


        // Generate a random number of steps to move forward
        int forward_index = Random.Range(0, 8); // How many steps from the current index to move forward
        landed_index = (forward_index + landed_index) % 8;

        // Spin to the current index plus 3-4 full rotations to cause the wheel to spin multiple times.
        float spin_angle = 45f * forward_index + 1080f; // Each index spans 45 degrees.

        DisableSpinButton();

        // Play the spin sound effect
        spin_audio_source.PlayOneShot(snd_barrel_spin);

        // Animate the wheel spinning with a shake effect
        ref_wheel_barrel.transform.DOShakeScale(1f, 0.025f, 10, 90, false);
        ref_wheel_barrel.transform.DORotate(new Vector3(0, 0, spin_angle), spin_duration, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            is_spinning = false;

            EnableSpinButton();

            // Get the landed item based on the landed index and the current awards from the minigame manager
            AwardEntry landed_award = minigame_manager.current_awards.award_list[landed_index];

            Debug.Log("Landed on item: " + landed_award.item_data.display_name + " with value: " + landed_award.amount);

            ProcessSpinRoundOutcome(landed_award);

        });


        // Check if the landed award is a bomb

    }

    private void ProcessSpinRoundOutcome(AwardEntry landed_award)
    {

        if (landed_award.item_data.item_id != "bomb")
        {
            // Update the round.
            minigame_manager.AcquireAwardAtIndex(landed_index);
            minigame_manager.IncreaseSpinRound();
        }
        else
        {
            // Handle bomb outcome (e.g., reset round, deduct points, etc.)
            spin_audio_source.PlayOneShot(snd_gunshot, 1f);
            minigame_manager.TriggerLose();

        }
    }

    private void SwitchWheelZoneUI(SpinWheelZoneType zone_type)
    {
        switch (zone_type)
        {
            case SpinWheelZoneType.RiskZone:
                ref_wheel_barrel.GetComponent<Image>().sprite = wheel_sprites.risk_zone_wheel_sprite;
                ref_wheel_indicator_image.sprite = wheel_sprites.risk_zone_indicator_sprite;
                ref_wheel_title_tmp.text = wheel_sprites.risk_zone_title_text;
                ref_wheel_title_tmp.color = wheel_sprites.risk_zone_title_color;
                ref_wheel_description_tmp.text = wheel_sprites.risk_zone_description_text;
                ref_wheel_description_tmp.color = wheel_sprites.risk_zone_description_color;
                break;
            case SpinWheelZoneType.SafeZone:
                ref_wheel_barrel.GetComponent<Image>().sprite = wheel_sprites.safe_zone_wheel_sprite;
                ref_wheel_indicator_image.sprite = wheel_sprites.safe_zone_indicator_sprite;
                ref_wheel_title_tmp.text = wheel_sprites.safe_zone_title_text;
                ref_wheel_title_tmp.color = wheel_sprites.safe_zone_title_color;
                ref_wheel_description_tmp.text = wheel_sprites.safe_zone_description_text;
                ref_wheel_description_tmp.color = wheel_sprites.safe_zone_description_color;
                break;
            case SpinWheelZoneType.SuperZone:
                ref_wheel_barrel.GetComponent<Image>().sprite = wheel_sprites.super_zone_wheel_sprite;
                ref_wheel_indicator_image.sprite = wheel_sprites.super_zone_indicator_sprite;
                ref_wheel_title_tmp.text = wheel_sprites.super_zone_title_text;
                ref_wheel_title_tmp.color = wheel_sprites.super_zone_title_color;
                ref_wheel_description_tmp.text = wheel_sprites.super_zone_description_text;
                ref_wheel_description_tmp.color = wheel_sprites.super_zone_description_color;
                break;
        }
    }


    private void DisableSpinButton()
    {
        ref_spin_button.interactable = false;
        ref_spin_button.image.DOFade(0.5f, 0.2f);
    }

    private void EnableSpinButton()
    {
        ref_spin_button.interactable = true;
        ref_spin_button.image.DOFade(1f, 0.2f);
    }

}
