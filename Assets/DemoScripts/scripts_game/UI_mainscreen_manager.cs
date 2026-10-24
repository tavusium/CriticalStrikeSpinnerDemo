using UnityEngine;

public class UI_mainscreen_manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void ShowMainScreen()
    {
        gameObject.SetActive(true);
    }

    public void HideMainScreen()
    {
        gameObject.SetActive(false);
    }


}
