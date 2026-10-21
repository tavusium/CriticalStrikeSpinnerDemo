using UnityEngine;



[System.Serializable]
public class AwardEntry
{
    public spin_award_item_data data;
    public int amount;
}




[CreateAssetMenu(fileName = "spin_wheel_zone_award_list", menuName = "Scriptable Objects/spin_wheel_zone_award_list")]
public class spin_wheel_zone_award_list : ScriptableObject
{

    public AwardEntry[] award_list = new AwardEntry[8];


    /*
    public spin_award_item_data slot_0;
    public int slot_0_amount;

    public spin_award_item_data slot_1;
    public int slot_1_amount;

    public spin_award_item_data slot_2;
    public int slot_2_amount;

    public spin_award_item_data slot_3;
    public int slot_3_amount;

    public spin_award_item_data slot_4;
    public int slot_4_amount;

    public spin_award_item_data slot_5;
    public int slot_5_amount;

    public spin_award_item_data slot_6;
    public int slot_6_amount;

    public spin_award_item_data slot_7;
    public int slot_7_amount;
    */
}
