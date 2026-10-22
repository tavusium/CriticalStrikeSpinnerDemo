using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class spin_minigame_manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject ref_content_panel;
    [SerializeField] private GameObject ref_loseview_popup;
    [SerializeField] private GameObject ref_lastchance_popup;
    [SerializeField] private TMP_Text ref_num_spins_text;
    [SerializeField] private TMP_Text ref_player_golds_text;
    [SerializeField] private Spin_Manager ref_spin_manager;

    [Header("Prefab References")]
    [SerializeField] private ui_comp_award_entry prefab_content_entry;


    [Header("Award Lists")]
    [SerializeField]
    private spin_wheel_zone_award_list bronze_wheel_awards;

    [SerializeField]
    private spin_wheel_zone_award_list silver_wheel_awards;

    [SerializeField]
    private spin_wheel_zone_award_list gold_wheel_awards;


    [Header("Minigame Settings")]
    [SerializeField]
    private int base_revive_cost = 25;

    [SerializeField]
    private int revive_cost_multiplier = 2;

    [Header("External Data")]
    [SerializeField] private int player_currency = 100; // Starting currency





    // Minigame variables
    private SpinWheelZoneType current_zone_type = SpinWheelZoneType.RiskZone;
    private int current_spin_round = 0;
    public spin_wheel_zone_award_list current_awards;
    private int num_revives = 0;
    private int safe_per_rounds = 3; // Every 5 rounds, the player enters a safe zone
    private int super_per_rounds = 6; // Every 30 rounds, the player enters a super zone

    private List<AwardEntry> collected_items = new List<AwardEntry>();



    private void Awake()
    {
        current_awards = bronze_wheel_awards; // Default to bronze awards

        ref_loseview_popup.SetActive(false);
        ref_lastchance_popup.SetActive(false);
    }
    private void Start()
    {
        SetZone(SpinWheelZoneType.RiskZone, true);
    }

    public void IncreaseSpinRound()
    {
        current_spin_round++;
        ref_num_spins_text.text = "Round: " + current_spin_round + "Next: " + (current_spin_round + 1);

        SpinWheelZoneType next_zone_type;

        if ((current_spin_round + 1) % safe_per_rounds == 0 && (current_spin_round + 1) % super_per_rounds != 0)
        {
            // Safe Zone
            next_zone_type = SpinWheelZoneType.SafeZone;
            current_awards = silver_wheel_awards;
        }
        else if ((current_spin_round + 1) % super_per_rounds == 0)
        {
            // Super Zone
            next_zone_type = SpinWheelZoneType.SuperZone;
            current_awards = gold_wheel_awards;
        }
        else
        {
            // Risk Zone
            next_zone_type = SpinWheelZoneType.RiskZone;
            current_awards = bronze_wheel_awards;
        }

        SetZone(next_zone_type);
    }

    public void AcquireAwardAtIndex(int index)
    {
        AwardEntry award = current_awards.award_list[index];

        // We should check if the same award already exists in the collected_items list. If it does, we can just increase the amount.
        int possible_index = collected_items.FindIndex(a => a.item_data.item_id == award.item_data.item_id); // Returns -1 if not found.

        if (possible_index != -1)
        {
            // Item exists at collected_items[index]
            collected_items[possible_index].amount += award.amount;

            // Update the UI for that specific entry
            ref_content_panel.transform.GetChild(possible_index).GetComponent<ui_comp_award_entry>().UpdateAmount(collected_items[possible_index].amount);

            return;
        }

        // If the item doesn't exist, we add it to the list and create a new UI entry.
        collected_items.Add(new AwardEntry { item_data = award.item_data, amount = award.amount });
        ui_comp_award_entry new_entry = Instantiate(prefab_content_entry, ref_content_panel.transform);
        new_entry.Setup(current_awards.award_list[index].item_data, current_awards.award_list[index].amount);
    }




    private void SetZone(SpinWheelZoneType zone_type, bool forceSet = false)
    {
        if (current_zone_type == zone_type && !forceSet) return;

        current_zone_type = zone_type;
        ref_spin_manager.SetZoneType(zone_type, current_awards);
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
        Debug.Log("Revive cost is: " + revive_cost + ". Player currency is: " + player_currency);
        if (player_currency >= revive_cost)
        {
            player_currency -= revive_cost;
            num_revives++;
            ref_player_golds_text.text = player_currency.ToString("N0");
            ref_loseview_popup.SetActive(false);
            ref_loseview_popup.GetComponent<ui_spin_loseview_box>().UpdateReviveCostText();
            // Allow the player to continue spinning
            Debug.Log("Player revived with gold. Remaining currency: " + player_currency);
        }
        else
        {
            Debug.Log("Not enough currency to revive. Required: " + revive_cost + ", Available: " + player_currency);
            // Optionally, you can show a message to the player indicating they don't have enough currency.
        }
    }

    public void HandleReviveWithAd()
    {
        // To be implemented.
    }

    public void HandleGiveUp()
    {
        ref_lastchance_popup.SetActive(true);
    }

    public void HandleLastChanceLose()
    {
        // Stop the game.
    }

    public void HandleLastChanceGoBack()
    {
        ref_lastchance_popup.SetActive(false);
    }



}
