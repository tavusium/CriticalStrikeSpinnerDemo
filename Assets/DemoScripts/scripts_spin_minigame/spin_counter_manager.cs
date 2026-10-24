using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Length - 8 = indicated item : initial indicated = 8(itemcount) - 8 = 0th entry.
// indicated ->  size = 142, 142 at 0,0
// normal -> size = 120, 120
// second offset x = 140

public class spin_counter_manager : MonoBehaviour
{

    [Header("TEST Animation Settings")]
    // These values can be fixed.
    [SerializeField] private float ingoing_duration = 0.8f;
    [SerializeField] private float ingoing_delay = 0.3f;
    [SerializeField] private float outgoing_duration = 0.5f;
    [SerializeField] private float outgoing_delay = 0.0f;
    [SerializeField] private float ingoing_text_unfade_delay = 0.6f;
    [SerializeField] private float outgoing_text_unfade_delay = 0.4f;


    [Header("References")]
    [SerializeField] private GameObject spin_counter_item_prefab;


    [SerializeField] private GameObject prefab_counter_image;
    [SerializeField] private GameObject prefab_counter_text;
    [SerializeField] private GameObject ref_spin_counter_masked_container_images;
    [SerializeField] private GameObject ref_spin_counter_masked_container_texts;


    public void ResetSpinCounter(int safe_per_rounds, int super_per_rounds)
    {
        // Clear existing items
        foreach (Transform child in ref_spin_counter_masked_container_images.transform) Destroy(child.gameObject);
        foreach (Transform child in ref_spin_counter_masked_container_texts.transform) Destroy(child.gameObject);

        for (int i = 0; i < 8; i++)
        {
            GameObject item_image = Instantiate(
                prefab_counter_image,
                ref_spin_counter_masked_container_images.transform
            );

            GameObject item_text = Instantiate(
                prefab_counter_text,
                ref_spin_counter_masked_container_texts.transform
            );

            RectTransform rect_image = item_image.GetComponent<RectTransform>();
            RectTransform rect_text = item_text.GetComponent<RectTransform>();

            item_text.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString(); // Set the text to the round number (1-based index)

            // TODO: POSITIONING SHOULD RESPECT THE MASKED CONTAINER'S SIZE AND ANCHOR POINTS, NOT HARDCODED VALUES

            if (i == 0)
            {
                rect_image.anchoredPosition = new Vector2(0f, 0f);
                rect_text.anchoredPosition = new Vector2(0f, 0f);

                //item_image.GetComponent<Image>().enabled = true; // Enable the first item
                //item_image.GetComponent<RectTransform>().sizeDelta = new Vector2(142f, 142f); // Scale the first item to be larger
                item_text.GetComponentInChildren<TMP_Text>().color = new Color(0.05f, 0.05f, 0.05f, 1); // Darker if indicated

            }
            else
            {
                rect_image.anchoredPosition = new Vector2(140f + 120f * (i - 1), 0f);
                rect_text.anchoredPosition = new Vector2(140f + 120f * (i - 1), 0f);

                //item_image.GetComponent<Image>().enabled = false; // Disable Background

                item_text.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(i + 1, safe_per_rounds, super_per_rounds);

            }

            item_image.GetComponent<Image>().color = DetermineBackgroundColor(i + 1, safe_per_rounds, super_per_rounds);

        }

        /*
        // OLD METHOD
        // Create initial list
        for (int i = 0; i < 8; i++)
        {
            GameObject item = Instantiate(
                spin_counter_item_prefab,
                ref_spin_counter_masked_container_images.transform
            );

            RectTransform rect = item.GetComponent<RectTransform>();

            item.GetComponentInChildren<TMP_Text>().text = (i + 1).ToString(); // Set the text to the round number (1-based index)

            // TODO: POSITIONING SHOULD RESPECT THE MASKED CONTAINER'S SIZE AND ANCHOR POINTS, NOT HARDCODED VALUES

            if (i == 0)
            {
                rect.anchoredPosition = new Vector2(0f, 0f);

                item.GetComponent<Image>().enabled = true; // Enable the first item
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(142f, 142f); // Scale the first item to be larger
                item.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(i + 1, safe_per_rounds, super_per_rounds, true);

            }
            else
            {
                rect.anchoredPosition = new Vector2(140f + 120f * (i - 1), 0f);

                item.GetComponent<Image>().enabled = false; // Disable Background

                item.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(i + 1, safe_per_rounds, super_per_rounds);

            }

            item.GetComponent<Image>().color = DetermineBackgroundColor(i + 1, safe_per_rounds, super_per_rounds);

        }
        */
    }

    public void Increment(int new_round, int safe_per_rounds, int super_per_rounds)
    {
        CreateAndAddNewItem(new_round + 7, safe_per_rounds, super_per_rounds);


        for (int i = 0; i < ref_spin_counter_masked_container_images.transform.childCount; i++)
        {
            GameObject child_image = ref_spin_counter_masked_container_images.transform.GetChild(i).gameObject;
            GameObject child_text = ref_spin_counter_masked_container_texts.transform.GetChild(i).gameObject;
            RectTransform rect_item_image = child_image.GetComponent<RectTransform>();
            RectTransform rect_item_text = child_text.GetComponent<RectTransform>();

            if (ref_spin_counter_masked_container_images.transform.childCount - 8 == i) // Ingoing
            {
                // Move the ingoing item image.
                rect_item_image.DOAnchorPosX(rect_item_image.anchoredPosition.x - 140f, ingoing_duration)
                .SetEase(Ease.OutSine)
                .SetDelay(ingoing_delay);

                // Move the ingoing item text. Process is divided into two parts to allow for a slight delay before the second movement, creating a more dynamic effect.
                rect_item_text.DOAnchorPosX(rect_item_text.anchoredPosition.x - 70f, ingoing_duration / 2f)
                .OnComplete(() =>
                {
                    rect_item_text.DOAnchorPosX(rect_item_text.anchoredPosition.x - 70f, ingoing_duration / 2f)
                    .SetEase(Ease.OutSine)
                    .SetDelay(0.2f);
                });

                // Fade out the ingoing item text.
                child_text.GetComponent<TMP_Text>().DOFade(0f, 0.1f)
                .OnComplete(() =>
                {
                    // After fading out, change the color of the ingoing item text to the new indicated color.
                    Color color = DetermineTextColor(new_round, safe_per_rounds, super_per_rounds, true);
                    color.a = 0f; // Set alpha to 0 for fade-in effect
                    child_text.GetComponent<TMP_Text>().color = color;
                });

                // Unfade the ingoing item text.
                child_text.GetComponent<TMP_Text>().DOFade(1f, 0.3f).SetDelay(ingoing_text_unfade_delay);


            }
            else if (ref_spin_counter_masked_container_images.transform.childCount - 8 - 1 == i) // Outgoing
            {
                // Move the outgoing item image.
                rect_item_image.DOAnchorPosX(rect_item_image.anchoredPosition.x - 140f, outgoing_duration)
                .SetEase(Ease.Linear)
                .SetDelay(outgoing_delay);

                // Move the outgoing item text.
                rect_item_text.DOAnchorPosX(rect_item_text.anchoredPosition.x - 140f, 0.8f)
                .SetEase(Ease.OutSine)
                .SetDelay(outgoing_delay);

                // Fade out the outgoing item text and then change its color to the new indicated color.
                child_text.GetComponent<TMP_Text>().DOFade(0f, 0.2f)
                .OnComplete(() =>
                {
                    child_text.GetComponent<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f, 0);
                });

                // Unfade the outgoing item text.
                child_text.GetComponent<TMP_Text>().DOFade(1f, 0.3f).SetDelay(outgoing_text_unfade_delay);
            }
            else // Others
            {
                rect_item_image.DOAnchorPosX(rect_item_image.anchoredPosition.x - 120f, 0.8f)
                .SetEase(Ease.OutSine);
                rect_item_text.DOAnchorPosX(rect_item_text.anchoredPosition.x - 120f, 0.8f)
                .SetEase(Ease.OutSine);
            }


        }

        /*
        // OLD METHOD
        CreateAndAddNewItem(new_round + 7, safe_per_rounds, super_per_rounds);


        int index = 0;
        foreach (RectTransform child in
                 ref_spin_counter_masked_container_images.transform)
        {
            if (ref_spin_counter_masked_container_images.transform.childCount - 8 == index)
            {
                // NEW Indicated Item / Upcoming round, being indicated now
                child.DOAnchorPosX(child.anchoredPosition.x - 140f, 0.5f)
                .SetEase(Ease.InOutSine);
                child.GetComponent<Image>().enabled = true;


                child.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 142f);

                DOVirtual.Vector2(new Vector2(0f, 142f), new Vector2(142f, 142f), ingoing_duration,
                value => child.GetComponent<RectTransform>().sizeDelta = value)
                .SetEase(Ease.OutSine)
                .SetDelay(ingoing_delay).OnStart(() =>
                {
                    child.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(new_round, safe_per_rounds, super_per_rounds, true);

                });


            }
            else if (ref_spin_counter_masked_container_images.transform.childCount - 8 - 1 == index)
            {
                // Previous Indicated Item / previously indicated round, now moving to normal
                child.DOAnchorPosX(child.anchoredPosition.x - 140f, 0.5f)
                .SetEase(Ease.InOutSine);
                //child.GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);
                //child.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f, 1);


                DOVirtual.Vector2(new Vector2(142f, 142f), new Vector2(0f, 142f), outgoing_duration,
                value => child.GetComponent<RectTransform>().sizeDelta = value)
                .SetEase(Ease.InSine)
                .SetDelay(outgoing_delay).OnStart(() =>
                {
                    child.GetComponentInChildren<TMP_Text>().color = new Color(0.9f, 0.9f, 0.9f, 1);

                })
                .OnComplete(() =>
                {
                    child.GetComponent<Image>().enabled = false;

                });


            }
            else
            {
                // Others
                child.DOAnchorPosX(child.anchoredPosition.x - 120f, 0.5f)
                .SetEase(Ease.InOutSine);

            }




            index++;
        }


        DOVirtual.DelayedCall(1f, () =>
        {
            // After the animation completed, remove the first item 
            if (new_round > 7)
            {
                Destroy(ref_spin_counter_masked_container_images.transform.GetChild(0).gameObject);
            }
        });
        */

    }

    private void CreateAndAddNewItem(int index, int safe_per_rounds, int super_per_rounds)
    {

        GameObject item_image = Instantiate(
                prefab_counter_image,
                ref_spin_counter_masked_container_images.transform
            );

        GameObject item_text = Instantiate(
            prefab_counter_text,
            ref_spin_counter_masked_container_texts.transform
        );

        RectTransform rect_image = item_image.GetComponent<RectTransform>();
        RectTransform rect_text = item_text.GetComponent<RectTransform>();

        rect_image.anchoredPosition = new Vector2(140f + 120f * 7, 0f); // Always place outside the visible area to the right
        rect_text.anchoredPosition = new Vector2(140f + 120f * 7, 0f); // Always place outside the visible area to the right

        item_text.GetComponentInChildren<TMP_Text>().text = index.ToString();
        item_text.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(index, safe_per_rounds, super_per_rounds);
        item_image.GetComponent<Image>().color = DetermineBackgroundColor(index, safe_per_rounds, super_per_rounds);


        /*
        // OLD METHOD
        GameObject item = Instantiate(
            spin_counter_item_prefab,
            ref_spin_counter_masked_container_images.transform
        );

        RectTransform rect = item.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(140f + 120f * 7, 0f); // Always place outside the visible area to the right

        item.GetComponentInChildren<TMP_Text>().text = index.ToString();
        item.GetComponentInChildren<TMP_Text>().color = DetermineTextColor(index, safe_per_rounds, super_per_rounds);
        item.GetComponent<Image>().color = DetermineBackgroundColor(index, safe_per_rounds, super_per_rounds);
        item.GetComponent<Image>().enabled = false; // Disable Background
        return item;
        */
    }


    private Color DetermineTextColor(int index, int safe_per_rounds, int super_per_rounds, bool is_indicated = false)
    {

        if (index % super_per_rounds == 0) // Super Zone
        {
            return new Color(1f, 0.79f, 0.43f, 1);
        }

        if (index % safe_per_rounds == 0) // Safe Zone
        {
            return new Color(0.53f, 0.79f, 0.51f, 1);
        }

        // Risk Zone
        return is_indicated ? new Color(0.05f, 0.05f, 0.05f, 1) : new Color(0.9f, 0.9f, 0.9f, 1); // Darker if indicated
    }

    private Color DetermineBackgroundColor(int index, int safe_per_rounds, int super_per_rounds)
    {

        if (index % super_per_rounds == 0) // Super Zone
        {
            return new Color(0.7f, 0.39f, 0, 1);
        }

        if (index % safe_per_rounds == 0) // Safe Zone
        {
            return new Color(0.277f, 0.54f, 0, 1);
        }

        // Risk Zone
        return new Color(1, 1, 1, 1); // White for risk zone
    }



}
