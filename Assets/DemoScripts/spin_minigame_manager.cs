using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class spin_minigame_manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject ref_content_panel;
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

    private SpinWheelZoneType current_zone_type = SpinWheelZoneType.RiskZone;
    private int current_spin_round = 0;
    public spin_wheel_zone_award_list current_awards;

    private int num_revives = 0;

    private int safe_per_rounds = 3; // Every 5 rounds, the player enters a safe zone
    private int super_per_rounds = 6; // Every 30 rounds, the player enters a super zone

    private List<AwardEntry> collected_items = new List<AwardEntry>();


    private void OnValidate()
    {
        current_awards = bronze_wheel_awards; // Default to bronze awards
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
        collected_items.Add(current_awards.award_list[index]);
        ui_comp_award_entry new_entry = Instantiate(prefab_content_entry, ref_content_panel.transform);
        new_entry.Setup(current_awards.award_list[index].item_data, current_awards.award_list[index].amount);
    }

    private int CalculateReviveCost()
    {
        return base_revive_cost * (int)Mathf.Pow(revive_cost_multiplier, num_revives);
    }


    private void SetZone(SpinWheelZoneType zone_type, bool forceSet = false)
    {
        if (current_zone_type == zone_type && !forceSet) return;

        current_zone_type = zone_type;
        ref_spin_manager.SetZoneType(zone_type, current_awards);
        Debug.Log("Zone changed to: " + current_zone_type.ToString());

    }

    public void TriggerLose()
    {
        // Handle the lose condition here
        Debug.Log("Player has lost the round.");
        // You can add additional logic for losing, such as showing a UI panel or deducting currency.
    }


}
