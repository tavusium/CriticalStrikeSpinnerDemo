using UnityEngine;


[CreateAssetMenu(fileName = "spin_wheel_zone_appearence", menuName = "Scriptable Objects/spin_wheel_zone_appearence")]
public class spin_wheel_zone_appearence : ScriptableObject
{
    public Sprite risk_zone_wheel_sprite;
    public Sprite risk_zone_indicator_sprite;
    public Color risk_zone_title_color = new Color(0.57f, 0.35f, 0.11f); // 925A1D
    public Color risk_zone_description_color = new Color(0.57f, 0.35f, 0.11f); // 925A1D

    public string risk_zone_title_text = "Bronze Spin";
    public string risk_zone_description_text = "Earn Rewards";

    public Sprite safe_zone_wheel_sprite;
    public Sprite safe_zone_indicator_sprite;
    public Color safe_zone_title_color = new Color(0.81f, 0.81f, 0.81f); // CFCFCF
    public Color safe_zone_description_color = new Color(0.81f, 0.81f, 0.81f); // CFCFCF

    public string safe_zone_title_text = "Silver Spin";
    public string safe_zone_description_text = "Earn x3 Rewards";

    public Sprite super_zone_wheel_sprite;
    public Sprite super_zone_indicator_sprite;

    public Color super_zone_title_color = new Color(1f, 0.72f, 0.26f); // FFB743
    public Color super_zone_description_color = new Color(1f, 0.72f, 0.26f); // FFB743

    public string super_zone_title_text = "Golden Spin";
    public string super_zone_description_text = "Earn x10 Rewards";
}
