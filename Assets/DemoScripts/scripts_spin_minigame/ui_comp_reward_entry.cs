
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui_comp_reward_entry : MonoBehaviour
{
    [SerializeField] private Image icon;
    //[SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amount_text;

    private int amount_value = 0;

    public void Setup(spin_reward_item_data data, int amount)
    {
        icon.sprite = data.item_icon;
        amount_text.text = "0";

        DOVirtual.Int(0, amount, 1f,
        value => amount_text.text = value.ToString("N0", System.Globalization.CultureInfo.InvariantCulture))
        .SetEase(Ease.InOutSine)
        .SetDelay(1f);

        amount_value = amount;

    }

    public void UpdateAmount(int new_amount)
    {
        DOVirtual.Int(amount_value, new_amount, 1f,
        value => amount_text.text = value.ToString("N0", System.Globalization.CultureInfo.InvariantCulture))
        .SetEase(Ease.InOutSine)
        .SetDelay(1f);

        amount_value = new_amount;
    }





}
