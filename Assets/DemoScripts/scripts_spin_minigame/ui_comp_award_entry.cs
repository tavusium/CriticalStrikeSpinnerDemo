
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ui_comp_award_entry : MonoBehaviour
{
    [SerializeField] private Image icon;
    //[SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amountText;


    public void Setup(spin_reward_item_data data, int amount)
    {
        icon.sprite = data.item_icon;
        amountText.text = "0";
        DOVirtual.Int(0, amount, 1f,
        value => amountText.text = value.ToString())
        .SetEase(Ease.InOutSine)
        .SetDelay(1f);
    }

    public void UpdateAmount(int newAmount)
    {
        DOVirtual.Int(int.Parse(amountText.text), newAmount, 1f,
        value => amountText.text = value.ToString())
        .SetEase(Ease.InOutSine)
        .SetDelay(1f);
    }





}
