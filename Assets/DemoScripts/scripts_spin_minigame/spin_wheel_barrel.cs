using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spin_wheel_barrel : MonoBehaviour
{

    private List<Image> slot_images = new List<Image>();

    private List<TMP_Text> slot_amount_texts = new List<TMP_Text>();

    //private int num_slots = 8; // Assuming there are 8 slots on the wheel



    private void Awake()
    {
        GetSlotComponents();
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


    public void ReconstructBarrel(spin_wheel_zone_reward_list reward_list)
    {
        for (int i = 0; i < 8; i++) // Assuming there are 8 slots on the barrel
        {
            RewardEntry reward = reward_list.reward_list[i];
            slot_images[i].sprite = reward.item_data.item_icon;
            slot_amount_texts[i].text = reward.item_data.item_id == "bomb" ? "" : "x" + reward.amount.ToString();
        }

    }
}
