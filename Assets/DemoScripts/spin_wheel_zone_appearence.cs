using UnityEngine;


[CreateAssetMenu(fileName = "spin_wheel_zone_appearence", menuName = "Scriptable Objects/spin_wheel_zone_appearence")]
public class spin_wheel_zone_appearence : ScriptableObject
{
    public Sprite risk_zone_wheel_sprite;
    public Sprite risk_zone_indicator_sprite;

    public string risk_zone_title_text = "Bronze Spin";
    public string risk_zone_description_text = "Earn Rewards";

    public Sprite safe_zone_wheel_sprite;
    public Sprite safe_zone_indicator_sprite;

    public string safe_zone_title_text = "Silver Spin";
    public string safe_zone_description_text = "Earn x3 Rewards";

    public Sprite super_zone_wheel_sprite;
    public Sprite super_zone_indicator_sprite;

    public string super_zone_title_text = "Golden Spin";
    public string super_zone_description_text = "Earn x10 Rewards";
}
