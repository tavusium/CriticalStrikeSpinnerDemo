using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Spin_Manager : MonoBehaviour
{
    [SerializeField] private GameObject ref_spin_wheel;
    private spin_wheel_component spin_wheel_comp;
    [SerializeField] private float spin_duration = 3f;

    private Button ref_spin_button;
    private Button ref_revive_button_currency;
    private Button ref_revive_button_ad;
    private Button ref_giveup_button;
    private Button ref_claim_button;


    private bool is_spinning = false;
    private int current_index = 0;


    [SerializeField] private AudioSource spin_audio_source;
    [SerializeField] private AudioClip spin_audio_clip;


    // Editor-only validation method to get the button references
    private void OnValidate()
    {
        ref_spin_button = GameObject.Find("Button_Spin_Wheel").GetComponent<Button>();
        /*
        ref_revive_button_currency = GameObject.Find("Button_Revive_Currency").GetComponent<Button>();
        ref_revive_button_ad = GameObject.Find("Button_Revive_Ad").GetComponent<Button>();
        ref_giveup_button = GameObject.Find("Button_Giveup").GetComponent<Button>();
        ref_claim_button = GameObject.Find("Button_Claim").GetComponent<Button>();
        */
    }

    private void Awake()
    {
        ref_spin_button.onClick.AddListener(SpinWheel);
        spin_wheel_comp = ref_spin_wheel.GetComponent<spin_wheel_component>();
    }


    public void SpinWheel()
    {
        if (is_spinning) return;
        is_spinning = true;

        // Generate a random number of steps to move forward
        int forward_index = Random.Range(0, 8); // How many steps from the current index to move forward
        Debug.Log("Spinning from index: " + current_index + " to index: " + forward_index);
        current_index = (forward_index + current_index) % 8; // Setting the new current

        // Spin to the current index plus 3-4 full rotations to cause the wheel to spin multiple times.
        float spin_angle = 45f * forward_index + 1080f; // Each index spans 45 degrees, so multiply by 45

        ref_spin_button.interactable = false; // Disable the spin button while spinning
        ref_spin_button.image.DOFade(0.5f, 0.2f); // Fade the button to indicate it's disabled

        spin_audio_source.PlayOneShot(spin_audio_clip); // Play the spin sound effect

        ref_spin_wheel.transform.DOShakeScale(1f, 0.15f, 10, 90, false);
        ref_spin_wheel.transform.DORotate(new Vector3(0, 0, spin_angle), spin_duration, RotateMode.FastBeyond360)
        .SetRelative(true)
        .SetEase(Ease.OutSine)
        .OnComplete(() =>
        {
            is_spinning = false;
            ref_spin_button.image.DOFade(1f, 0.2f); // Restore the button's original appearance
            ref_spin_button.interactable = true; // Re-enable the spin button after spinning
            Debug.Log("Spin Complete! Landed on index: " + current_index);
            AwardEntry landed_item = spin_wheel_comp.GetItemAtIndex(current_index);
            if (landed_item != null)
            {
                Debug.Log("Landed on item: " + landed_item.data.display_name + " with value: " + landed_item.amount);
            }
        });
    }



}
