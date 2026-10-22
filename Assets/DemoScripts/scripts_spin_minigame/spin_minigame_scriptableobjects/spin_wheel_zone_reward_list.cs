using UnityEngine;



[System.Serializable]
public class RewardEntry
{
    public spin_reward_item_data item_data;
    public int amount;
}




[CreateAssetMenu(fileName = "spin_wheel_zone_reward_list", menuName = "Scriptable Objects/spin_wheel_zone_reward_list")]
public class spin_wheel_zone_reward_list : ScriptableObject
{

    public RewardEntry[] reward_list = new RewardEntry[8];

}
