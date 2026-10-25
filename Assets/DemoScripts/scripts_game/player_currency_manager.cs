using DG.Tweening;
using TMPro;
using UnityEngine;


public class player_currency_manager : MonoBehaviour
{

    [Header("Player Currency")]
    [SerializeField] private int player_currency_cash = 8000;
    [SerializeField] private int player_currency_gold = 500;

    [Header("UI References")]
    [SerializeField] private TMP_Text ref_player_cash_text;
    [SerializeField] private TMP_Text ref_player_golds_text;

    [Header("Audio")]
    [SerializeField] private AudioSource ref_audio_source;
    [SerializeField] private AudioClip sfx_update_cash;
    [SerializeField] private AudioClip sfx_update_gold;



    private void OnValidate()
    {
        if (ref_player_cash_text == null)
            ref_player_cash_text = transform.Find("UI_player_currency_cash_value").GetComponent<TMP_Text>();

        if (ref_player_golds_text == null)
            ref_player_golds_text = transform.Find("UI_player_currency_gold_value").GetComponent<TMP_Text>();

        if (ref_audio_source == null)
            ref_audio_source = GameObject.Find("audiosource_sfx").GetComponent<AudioSource>();
    }



    public void UpdatePlayerCash(int new_cash_amount)
    {
        ref_audio_source.PlayOneShot(sfx_update_cash, 1f);

        DOVirtual.Int(player_currency_cash, new_cash_amount, 1f,
        value => ref_player_cash_text.text = value.ToString("N0", System.Globalization.CultureInfo.InvariantCulture))
        .SetEase(Ease.InOutSine);

        player_currency_cash = new_cash_amount;


    }
    public void UpdatePlayerGold(int new_gold_amount)
    {
        ref_audio_source.PlayOneShot(sfx_update_gold, 1f);

        DOVirtual.Int(player_currency_gold, new_gold_amount, 1f,
        value => ref_player_golds_text.text = value.ToString("N0", System.Globalization.CultureInfo.InvariantCulture))
        .SetEase(Ease.InOutSine);

        player_currency_gold = new_gold_amount;
    }

    public bool CheckPlayerCash(int amount)
    {
        return player_currency_cash >= amount;
    }

    public bool CheckPlayerGold(int amount)
    {
        return player_currency_gold >= amount;
    }

    public int GetPlayerCash()
    {
        return player_currency_cash;
    }

    public int GetPlayerGold()
    {
        return player_currency_gold;
    }




}
