using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Performance : MonoBehaviour
{
    [SerializeField]
    private Sprite badSprite;
    [SerializeField]
    private Sprite goodSprite;
    [SerializeField]
    private Sprite greatSprite;
    [SerializeField]
    private Sprite perfectSprite;

    public GameObject floatingPerfPrefab;


    public void ShowFloatingPerformance(int performance)
    {
        GameObject prefab = Instantiate(floatingPerfPrefab, transform.position, Quaternion.identity, transform);
        // Get the Image component from the instantiated prefab
        Image image = prefab.GetComponent<Image>();

        if (image != null)
        {
            // Set the sprite based on the performance
            if (performance == 0)
            {
                image.sprite = badSprite;
            }
            else if (performance == 1)
            {
                image.sprite = goodSprite;
            }
            else if (performance == 2)
            {
                image.sprite = greatSprite;
            }
            else
            {
                image.sprite = perfectSprite;
            }
        }
        else
        {
            Debug.LogWarning("Image component not found on the floating performance prefab.");
        }

    }
}
