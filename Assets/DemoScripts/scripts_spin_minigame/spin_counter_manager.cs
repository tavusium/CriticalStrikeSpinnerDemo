using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class spin_counter_manager : MonoBehaviour
{

    [SerializeField] private GameObject spin_counter_item_prefab;
    [SerializeField] private GameObject spin_counter_masked_container;


    public void ResetSpinCounter(int safe_per_rounds, int super_per_rounds)
    {
        // Clear existing items
        foreach (Transform child in spin_counter_masked_container.transform)
            Destroy(child.gameObject);


        // Create initial list
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
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(142f, 142f); // Scale the first item to be larger
            }
            else
            {
                item.GetComponent<Image>().enabled = false; // Disable Background

                item.GetComponentInChildren<TMP_Text>().color = DetermineColor(i + 1, safe_per_rounds, super_per_rounds);

            }


        }
    }

    public void Increment(int new_round, int safe_per_rounds, int super_per_rounds)
    {
        foreach (RectTransform child in
                 spin_counter_masked_container.transform)
        {
            //child.anchoredPosition += new Vector2(-160f, 0f);
            child.DOAnchorPosX(child.anchoredPosition.x - 160f, 0.5f)
            .SetEase(Ease.InOutSine);
        }

        CreateAndAddNewItem(new_round + 7, safe_per_rounds, super_per_rounds);

        DOVirtual.DelayedCall(1f, () =>
        {
            // After the animation completed, remove the first item 
            if (new_round > 7)
            {
                Destroy(spin_counter_masked_container.transform.GetChild(0).gameObject);
            }
        });

    }

    private GameObject CreateAndAddNewItem(int index, int safe_per_rounds, int super_per_rounds)
    {
        GameObject item = Instantiate(
            spin_counter_item_prefab,
            spin_counter_masked_container.transform
        );

        RectTransform rect = item.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(160f * 7, 0f); // Always place outside the visible area to the right

        item.GetComponentInChildren<TMP_Text>().text = index.ToString();
        item.GetComponentInChildren<TMP_Text>().color = DetermineColor(index, safe_per_rounds, super_per_rounds);
        item.GetComponent<Image>().enabled = false; // Disable Background
        return item;
    }


    private Color DetermineColor(int index, int safe_per_rounds, int super_per_rounds)
    {
        if (index % super_per_rounds == 0) // Super Zone
        {
            return new Color(1f, 0.79f, 0.43f, 1);
        }
        else if (index % safe_per_rounds == 0) // Safe Zone
        {
            return new Color(0.53f, 0.79f, 0.51f, 1);
        }
        else // Risk Zone
        {
            return new Color(0.9f, 0.9f, 0.9f, 1);
        }
    }

}
