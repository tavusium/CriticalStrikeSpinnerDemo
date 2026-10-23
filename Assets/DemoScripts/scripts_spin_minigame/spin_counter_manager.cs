using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spin_counter_manager : MonoBehaviour
{

    [SerializeField] private GameObject spin_counter_item_prefab;
    [SerializeField] private GameObject spin_counter_masked_container;


    private void Awake()
    {
        ResetSpinCounter();
    }
    private void ResetSpinCounter()
    {
        foreach (Transform child in spin_counter_masked_container.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < 8; i++)
        {
            GameObject item = Instantiate(
                spin_counter_item_prefab,
                spin_counter_masked_container.transform
            );

            RectTransform rect = item.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(160f * i, 0f);

            item.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString();

            if (i == 0)
            {
                item.GetComponent<Image>().enabled = true; // Enable the first item
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(140f, 140f); // Scale the first item to be larger
            }
            else
            {
                item.GetComponent<Image>().enabled = false; // Disable other items
                item.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f, 1); // Make the text of other items semi-transparent
            }


        }
    }

    public void Increment(int safe_per_rounds, int super_per_rounds)
    {
        foreach (RectTransform child in
                 spin_counter_masked_container.transform)
        {
            //child.anchoredPosition += new Vector2(-160f, 0f);
            child.DOAnchorPosX(child.anchoredPosition.x - 160f, 0.5f).SetEase(Ease.InOutSine);
        }

        GameObject item = Instantiate(
            spin_counter_item_prefab,
            spin_counter_masked_container.transform
        );

        RectTransform rect = item.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(160f * 7, 0f);
        item.GetComponentInChildren<TMP_Text>().text = spin_counter_masked_container.transform.childCount.ToString();
    }

}
