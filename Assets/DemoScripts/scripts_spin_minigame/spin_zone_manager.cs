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


    //private SpinWheelZoneType current_zone_type = SpinWheelZoneType.RiskZone;


    public void UpdateZoneBoxes(
        SpinWheelZoneType zone_type,
        int current_round,
        int safe_per_rounds,
        int super_per_rounds)
    {
        int next_safe_round =
            ((current_round - 1) / safe_per_rounds + 1) * safe_per_rounds;

        int next_super_round =
            ((current_round - 1) / super_per_rounds + 1) * super_per_rounds;

        if (next_safe_round == next_super_round)
            next_safe_round += safe_per_rounds;

        DOTween.To(() => int.Parse(ref_safe_zone_value.text),
            x => ref_safe_zone_value.text = x.ToString(),
            next_safe_round, 3f);

        DOTween.To(() => int.Parse(ref_super_zone_value.text),
            x => ref_super_zone_value.text = x.ToString(),
            next_super_round, 3f);

        //current_zone_type = zone_type;
    }

}
