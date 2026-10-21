
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui_comp_award_entry : MonoBehaviour
{
    [SerializeField] private Image icon;
    //[SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amountText;

    public void Setup(spin_award_item_data data, int amount)
    {
        icon.sprite = data.item_icon;
        //nameText.text = data.display_name;
        amountText.text = amount.ToString();
    }
}
