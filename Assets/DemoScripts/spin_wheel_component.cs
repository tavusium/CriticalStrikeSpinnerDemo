using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spin_wheel_component : MonoBehaviour
{


    [SerializeField] GameObject ref_spin_wheel_indicator;
    private Image wheel_indicator_image;

    [SerializeField]
    private spin_wheel_zone_award_list bronze_wheel_awards;

    [SerializeField]
    private spin_wheel_zone_award_list silver_wheel_awards;

    [SerializeField]
    private spin_wheel_zone_award_list gold_wheel_awards;

    [SerializeField]
    private spin_wheel_zone_sprites wheel_sprites;

    private SpinWheelZoneType zone_type = SpinWheelZoneType.RiskZone;






    private Image wheel_image;

    private List<Image> slot_images = new List<Image>();

    private List<TMP_Text> slot_amount_texts = new List<TMP_Text>();

    private int num_slots = 8; // Assuming there are 8 slots on the wheel



    void Awake()
    {
        wheel_image = GetComponent<Image>();
        wheel_indicator_image = ref_spin_wheel_indicator.GetComponent<Image>();
        GetSlotComponents();
        ReconstructWheel();

    }


    private void GetSlotComponents()
    {
        slot_images.Clear();
        foreach (Transform child in transform)
        {

            Image img = child.GetComponent<Image>();
            if (img != null)
            {
                slot_images.Add(img);
                TMP_Text amount_text = child.GetComponentInChildren<TMP_Text>();
                if (amount_text != null)
                {
                    slot_amount_texts.Add(amount_text);
                }
            }
        }
    }


    private void ReconstructWheel()
    {
        // This method would be responsible for reconstructing the wheel based on the award list.
        // For example, it could instantiate UI elements for each award slot and set their icons and amounts.
        // Since the implementation details are not provided, this is a placeholder for the actual logic.

        switch (zone_type)
        {
            case SpinWheelZoneType.RiskZone:
                wheel_image.sprite = wheel_sprites.risk_zone_wheel_sprite;
                wheel_indicator_image.sprite = wheel_sprites.risk_zone_indicator_sprite;
                UpdateSlotDisplays(bronze_wheel_awards);
                break;
            case SpinWheelZoneType.SafeZone:
                wheel_image.sprite = wheel_sprites.safe_zone_wheel_sprite;
                wheel_indicator_image.sprite = wheel_sprites.safe_zone_indicator_sprite;
                UpdateSlotDisplays(silver_wheel_awards);
                break;
            case SpinWheelZoneType.SuperZone:
                wheel_image.sprite = wheel_sprites.super_zone_wheel_sprite;
                wheel_indicator_image.sprite = wheel_sprites.super_zone_indicator_sprite;
                UpdateSlotDisplays(gold_wheel_awards);
                break;
        }


    }

    private void UpdateSlotDisplays(spin_wheel_zone_award_list award_list)
    {
        for (int i = 0; i < num_slots; i++)
        {
            if (i < award_list.award_list.Length)
            {
                slot_images[i].sprite = award_list.award_list[i].data.item_icon;
                slot_amount_texts[i].text = award_list.award_list[i].amount.ToString();
            }
        }
    }

    public void SetZoneType(SpinWheelZoneType new_zone_type)
    {
        zone_type = new_zone_type;
        ReconstructWheel();
    }

    public AwardEntry GetItemAtIndex(int index)
    {
        if (index < 0 || index >= num_slots)
        {
            Debug.LogError("Index out of bounds");
            return null;
        }

        switch (zone_type)
        {
            case SpinWheelZoneType.RiskZone:
                return bronze_wheel_awards.award_list[index];
            case SpinWheelZoneType.SafeZone:
                return silver_wheel_awards.award_list[index];
            case SpinWheelZoneType.SuperZone:
                return gold_wheel_awards.award_list[index];
            default:
                return null;
        }
    }

}
