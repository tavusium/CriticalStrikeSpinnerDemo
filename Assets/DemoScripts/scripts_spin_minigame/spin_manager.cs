using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Spin_Manager : MonoBehaviour
{
    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;

    [Header("Spin Settings")]
    [SerializeField] private float spin_duration = 2.3f;

    [Header("Appearance")]
    [SerializeField] private spin_wheel_zone_appearence wheel_sprites;

    [Header("Audio")]
    [SerializeField] private AudioSource ref_audio_source;

    [SerializeField] private AudioClip sfx_barrel_spin;
    [SerializeField] private AudioClip sfx_revolver_clicks;
    [SerializeField] private AudioClip sfx_gunshot;
    [SerializeField] private AudioClip sfx_collect_cash;
    [SerializeField] private AudioClip sfx_collect_gold;
    [SerializeField] private AudioClip sfx_collect_item;
    [SerializeField] private AudioClip sfx_outcome_item;
    [SerializeField] private AudioClip sfx_outcome_item_valuable;
    [SerializeField] private AudioClip sfx_zone_safe_super;


    [Header("UI References")]
    [SerializeField] private GameObject ref_wheel_collection;
    [SerializeField] private GameObject ref_wheel_barrel;
    [SerializeField] private spin_wheel_barrel ref_wheel_barrel_component;
    [SerializeField] private TMP_Text ref_wheel_title_tmp;
    [SerializeField] private TMP_Text ref_wheel_description_tmp;
    [SerializeField] private Image ref_wheel_indicator_image;
    [SerializeField] private Button ref_spin_button; // offload this to minigame manager
    [SerializeField] private Button ref_ui_rewards_leave_button;

    private ui_reward_fly_effect ref_ui_reward_fly_effect;


    // Lists of item IDs for different sound effects
    // There can be better ways to do this, but for now, this is a simple approach.
    private List<string> cash_sfx_id_list = new List<string>
    { "cash", "armor_points", "knife_points", "pistol_points", "rifle_points",
        "shotgun_points", "sniper_points", "smg_points", "vest_points", "submachine_points" };
    private List<string> gold_sfx_id_list = new List<string>
    { "gold", "grenade_m26", "grenade_m67", "molotov", "healthshot_2_regenerator", "healthshot_2_neurostim" };


    // Wheel states
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

        ref_ui_rewards_leave_button = GameObject.Find("UI_rewards_button_leave").GetComponent<Button>();

        if (ref_audio_source == null)
        {
            ref_audio_source = GameObject.Find("audiosource_sfx").GetComponent<AudioSource>();
        }

    }

    private void Awake()
    {
        ref_spin_button.onClick.AddListener(SpinWheel);
        ref_ui_reward_fly_effect = GetComponent<ui_reward_fly_effect>();
    }


    public void SetZoneType(SpinWheelZoneType zone_type, spin_wheel_zone_reward_list rewards, bool is_silent = false)
    {

        if (is_silent)
        {
            // Update the wheel's appearance and data based on the zone type
            ref_wheel_barrel_component.ReconstructBarrel(rewards);
            SwitchWheelZoneUI(zone_type);
            return;
        }



        // Update the wheel's appearance and data based on the zone type
        DisableSpinButton();
        ref_wheel_barrel_component.ReconstructBarrel(rewards);

        ref_audio_source.PlayOneShot(sfx_revolver_clicks, 0.5f);

        if (zone_type == SpinWheelZoneType.SafeZone || zone_type == SpinWheelZoneType.SuperZone)
        {
            ref_audio_source.PlayOneShot(sfx_zone_safe_super, 0.8f);
        }


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
        DisableLeaveButton();

        // Play the spin sound effect
        ref_audio_source.PlayOneShot(sfx_barrel_spin);

        // Animate the wheel spinning with a shake effect
        ref_wheel_barrel.transform.DOShakeScale(1f, 0.025f, 10, 90, false);
        ref_wheel_barrel.transform.DORotate(new Vector3(0, 0, spin_angle), spin_duration, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            is_spinning = false;


            // Get the landed item based on the landed index and the current awards from the minigame manager
            RewardEntry landed_award = ref_spin_minigame_manager.current_rewards.reward_list[landed_index];

            Debug.Log("Landed on item: " + landed_award.item_data.display_name + " with value: " + landed_award.amount);

            DOVirtual.DelayedCall(0.15f, () =>
            {
                ProcessSpinRoundOutcome(landed_award);
            });

        });


        // Check if the landed award is a bomb

    }

    private void ProcessSpinRoundOutcome(RewardEntry landed_reward)
    {

        if (landed_reward.item_data.item_id != "bomb")
        {
            // Update the round.
            ui_comp_reward_entry entry = ref_spin_minigame_manager.AcquireRewardAtIndex(landed_index); // This will be animated


            int num_of_fly_effects = landed_reward.amount < 10 ? landed_reward.amount : 15;

            if (landed_reward.amount == 1 || landed_reward.item_data.item_id == "gold")
            {
                ref_audio_source.PlayOneShot(sfx_outcome_item_valuable, 1f);
            }
            else
            {
                ref_audio_source.PlayOneShot(sfx_outcome_item, 1f);
            }


            ref_ui_reward_fly_effect.StartFlyEffect(landed_reward.item_data.item_icon,
                                                        new Vector2(0, 200),
                                                            entry.GetComponent<RectTransform>(), num_of_fly_effects);

            android_vibration.Medium();


            DOVirtual.DelayedCall(1f, () =>
            {
                if (cash_sfx_id_list.Contains(landed_reward.item_data.item_id))
                {
                    ref_audio_source.PlayOneShot(sfx_collect_cash, 1f);
                }
                else if (gold_sfx_id_list.Contains(landed_reward.item_data.item_id))
                {
                    ref_audio_source.PlayOneShot(sfx_collect_gold, 1f);
                }
                else
                {
                    ref_audio_source.PlayOneShot(sfx_collect_item, 1f);
                }


            });

            DOVirtual.DelayedCall(1.75f, () =>
            {
                EnableLeaveButton();
                EnableSpinButton();
                ref_spin_minigame_manager.IncreaseSpinRound();
            });



        }
        else
        {
            // Handle bomb outcome (e.g., reset round, deduct points, etc.)
            ref_audio_source.PlayOneShot(sfx_gunshot, 1f);
            android_vibration.Heavy();
            ref_spin_minigame_manager.TriggerLose();
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


    public void DisableSpinButton()
    {
        ref_spin_button.interactable = false;
        ref_spin_button.image.DOFade(0.5f, 0.2f);
    }

    public void EnableSpinButton()
    {
        ref_spin_button.interactable = true;
        ref_spin_button.image.DOFade(1f, 0.2f);
    }

    public void DisableLeaveButton()
    {
        ref_ui_rewards_leave_button.interactable = false;
        ref_ui_rewards_leave_button.image.DOFade(0.5f, 0.2f);
    }

    public void EnableLeaveButton()
    {
        ref_ui_rewards_leave_button.interactable = true;
        ref_ui_rewards_leave_button.image.DOFade(1f, 0.2f);
    }

}
