using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spin_minigame_manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject ref_content_panel;
    [SerializeField] private GameObject ref_loseview_popup;
    [SerializeField] private GameObject ref_lastchance_popup;
    [SerializeField] private GameObject ref_leave_popup;
    [SerializeField] private TMP_Text ref_player_golds_text;
    [SerializeField] private Spin_Manager ref_spin_manager;
    [SerializeField] private GameObject ref_ui_minigame;

    [Header("Prefab References")]
    [SerializeField] private ui_comp_award_entry prefab_content_entry;


    [Header("Award Lists")]
    [SerializeField]
    private spin_wheel_zone_reward_list bronze_wheel_rewards;

    [SerializeField]
    private spin_wheel_zone_reward_list silver_wheel_rewards;

    [SerializeField]
    private spin_wheel_zone_reward_list gold_wheel_rewards;


    [Header("Minigame Settings")]
    [SerializeField]
    private int base_revive_cost = 25;

    [SerializeField]
    private int revive_cost_multiplier = 2;

    [Header("Audio")]

    [SerializeField] private AudioSource ref_audio_source;

    [SerializeField] private AudioClip sfx_revive;
    [SerializeField] private AudioClip sfx_giveup;
    [SerializeField] private AudioClip sfx_leave;
    [SerializeField] private AudioClip sfx_goback;
    [SerializeField] private AudioClip sfx_lastchance_panel;




    [Header("External Data")]
    // These values can be used from an external source, 
    // such as a player profile or game manager.
    [SerializeField] private int player_currency_cash = 8000;
    [SerializeField] private int player_currency_gold = 500;





    // Minigame variables
    private SpinWheelZoneType current_zone_type = SpinWheelZoneType.RiskZone;
    private int current_spin_round = 0;
    public spin_wheel_zone_reward_list current_rewards;
    private int num_revives = 0;
    private int safe_per_rounds = 4; // Every 5 rounds, the player enters a safe zone
    private int super_per_rounds = 8; // Every 30 rounds, the player enters a super zone

    private List<RewardEntry> collected_items = new List<RewardEntry>();



    private void OnValidate()
    {
        if (ref_audio_source == null)
        {
            ref_audio_source = GameObject.Find("audiosource_sfx").GetComponent<AudioSource>();
        }
    }



    private void Awake()
    {
        current_rewards = bronze_wheel_rewards; // Default to bronze awards

        ref_loseview_popup.SetActive(false);
        ref_lastchance_popup.SetActive(false);
        ref_leave_popup.SetActive(false);
        ref_ui_minigame.SetActive(false);
    }


    public void IncreaseSpinRound()
    {
        current_spin_round++;

        SpinWheelZoneType next_zone_type;

        if ((current_spin_round + 1) % safe_per_rounds == 0 && (current_spin_round + 1) % super_per_rounds != 0)
        {
            // Safe Zone
            next_zone_type = SpinWheelZoneType.SafeZone;
            current_rewards = silver_wheel_rewards;
        }
        else if ((current_spin_round + 1) % super_per_rounds == 0)
        {
            // Super Zone
            next_zone_type = SpinWheelZoneType.SuperZone;
            current_rewards = gold_wheel_rewards;
        }
        else
        {
            // Risk Zone
            next_zone_type = SpinWheelZoneType.RiskZone;
            current_rewards = bronze_wheel_rewards;
        }

        SetZone(next_zone_type);
    }

    public ui_comp_award_entry AcquireRewardAtIndex(int index)
    {
        RewardEntry reward = current_rewards.reward_list[index];
        ui_comp_award_entry entry = null;

        // We should check if the same award already exists in the collected_items list. If it does, we can just increase the amount.
        int possible_index = collected_items.FindIndex(a => a.item_data.item_id == reward.item_data.item_id); // Returns -1 if not found.

        if (possible_index != -1)
        {
            // Item exists at collected_items[index]
            collected_items[possible_index].amount += reward.amount;

            // Update the UI for that specific entry. Triggers the animation for the amount text !!
            entry = ref_content_panel.transform.GetChild(possible_index).GetComponent<ui_comp_award_entry>();
            entry.UpdateAmount(collected_items[possible_index].amount);
            return entry;
        }

        // If the item doesn't exist, we add it to the list and create a new UI entry.
        collected_items.Add(new RewardEntry { item_data = reward.item_data, amount = reward.amount });
        entry = Instantiate(prefab_content_entry, ref_content_panel.transform);

        // Setup triggers the animation for the amount text !!
        entry.Setup(current_rewards.reward_list[index].item_data, current_rewards.reward_list[index].amount);


        LayoutRebuilder.ForceRebuildLayoutImmediate(
            ref_content_panel.GetComponent<RectTransform>());


        return entry.GetComponent<ui_comp_award_entry>();
    }




    private void SetZone(SpinWheelZoneType zone_type, bool forceSet = false)
    {
        if (current_zone_type == zone_type && !forceSet) return;

        current_zone_type = zone_type;
        ref_spin_manager.SetZoneType(zone_type, current_rewards);
        //Debug.Log("Zone changed to: " + current_zone_type.ToString());

    }

    public void TriggerLose()
    {
        // Handle the lose condition here
        Debug.Log("Player has lost the round.");
        // We can add additional logic for losing, such as showing a UI panel or deducting currency.
        ref_loseview_popup.GetComponent<ui_spin_loseview_box>().ShowLoseView();
    }


    public int CalculateReviveCost()
    {
        return base_revive_cost * (int)Mathf.Pow(revive_cost_multiplier, num_revives);
    }



    // Popup handling methods
    public void HandleReviveWithGold()
    {
        Debug.Log("Player chose to revive with gold.");
        int revive_cost = CalculateReviveCost();
        Debug.Log("Revive cost is: " + revive_cost + ". Player currency is: " + player_currency_gold);
        if (player_currency_gold >= revive_cost)
        {
            player_currency_gold -= revive_cost;
            num_revives++;
            ref_player_golds_text.text = player_currency_gold.ToString("N0");


            ref_loseview_popup.GetComponent<ui_spin_loseview_box>().AnimateHide();

            ref_audio_source.PlayOneShot(sfx_revive, 1f);

            ref_spin_manager.EnableSpinButton();
            ref_spin_manager.EnableLeaveButton();

            // Allow the player to continue spinning
        }
        else
        {
            // TODO: show a message to the player indicating they don't have enough currency.
        }
    }

    public void HandleReviveWithAd()
    {
        // To be implemented.

        ref_audio_source.PlayOneShot(sfx_revive, 1f);

        // Just revive for free for now.
        ref_loseview_popup.SetActive(false);
        ref_spin_manager.EnableSpinButton();
        ref_spin_manager.EnableLeaveButton();

    }

    public void HandleGiveUp()
    {
        ref_audio_source.PlayOneShot(sfx_lastchance_panel, 1f);
        ref_lastchance_popup.SetActive(true);
    }

    public void HandleLastChanceGiveUp()
    {
        // Exit the minigame.

        // Lose effects can be handled here if needed.
        ref_audio_source.PlayOneShot(sfx_giveup, 1f);
        ExitMinigame();
    }

    public void HandleLeave()
    {
        ref_audio_source.PlayOneShot(sfx_leave, 1f);
        ref_leave_popup.SetActive(true);
    }

    public void HandleLeaveGoBack()
    {
        ref_audio_source.PlayOneShot(sfx_goback, 1f);
        ref_leave_popup.SetActive(false);
    }

    public void HandleLastChanceGoBack()
    {
        ref_audio_source.PlayOneShot(sfx_goback, 1f);
        ref_lastchance_popup.SetActive(false);
    }

    public void HandleClaimRewardsAndExit() // Leave button used.
    {
        // Here we handle the logic for claiming the rewards.
        foreach (var item in collected_items)
        {
            Debug.Log($"Claimed {item.amount} of {item.item_data.item_id}");
            // Add logic to add the item to the player's inventory or currency.
        }

        ref_audio_source.PlayOneShot(sfx_leave, 1f);
        ExitMinigame();

    }

    private void ExitMinigame()
    {
        // Clear the collected items and reset the UI
        collected_items.Clear();
        foreach (Transform child in ref_content_panel.transform)
        {
            Destroy(child.gameObject);
        }
        ref_ui_minigame.SetActive(false);
        ref_loseview_popup.SetActive(false);
        ref_lastchance_popup.SetActive(false);
        ref_leave_popup.SetActive(false);
        ref_spin_manager.DisableLeaveButton();
    }

    public void HandleStartMinigame()
    {
        // Reset the minigame state
        current_spin_round = 0;
        num_revives = 0;
        current_zone_type = SpinWheelZoneType.RiskZone;
        current_rewards = bronze_wheel_rewards;

        // Clear any previous collected items and UI entries. NOT ACTUALLY NECESSARY
        collected_items.Clear();
        foreach (Transform child in ref_content_panel.transform)
        {
            Destroy(child.gameObject);
        }

        ref_ui_minigame.SetActive(true);
        SetZone(SpinWheelZoneType.RiskZone, true);
        ref_spin_manager.EnableLeaveButton();

    }

}
