
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
        amountText.text = amount.ToString();
    }

    public void UpdateAmount(int newAmount)
    {
        amountText.text = newAmount.ToString();
    }
}
