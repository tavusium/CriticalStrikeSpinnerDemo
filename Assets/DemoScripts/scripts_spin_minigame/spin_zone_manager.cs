using UnityEngine;
using DG.Tweening;
using TMPro;

public class spin_zone_manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private spin_minigame_manager ref_spin_minigame_manager;
    [SerializeField] private GameObject ref_safe_zone_box;
    [SerializeField] private GameObject ref_super_zone_box;

    [SerializeField] private TMP_Text ref_safe_zone_value;
    [SerializeField] private TMP_Text ref_super_zone_value;


    private SpinWheelZoneType current_zone_type = SpinWheelZoneType.RiskZone;



    public void UpdateZoneBoxes(SpinWheelZoneType zone_type, int current_round, int safe_per_rounds, int super_per_rounds)
    {

        // Maybe add some animation here.

        int next_safe_round = current_round / safe_per_rounds * safe_per_rounds + safe_per_rounds + current_round / super_per_rounds;
        int next_super_round = current_round / super_per_rounds * super_per_rounds + super_per_rounds;
        if (next_super_round == next_safe_round) next_safe_round += safe_per_rounds; // Ensure that the next safe round is not the same as the next super round

        ref_safe_zone_value.text = next_safe_round.ToString();
        ref_super_zone_value.text = next_super_round.ToString();


        current_zone_type = zone_type;
    }


}
