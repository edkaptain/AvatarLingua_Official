using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    #region -- Unity Inspector --
    [Header("Characteristics")]
    public string title = "Example";
    public string description = "Object description";
    private Material[] originalColors;
    public Material grayColor;
    [Header("Canvas information")]
    public GameObject itemNumber;
    public GameObject canvasTitle;
    public GameObject canvasDescription;
    private GameObject arrow;
    public GameObject model;
    private bool isGray;
    private bool itemNumberCanvas;
    public bool isClicked;
    [Header("Testing")]
    public Renderer itemRenderer;
    #endregion

    //Testing
    //private void OnValidate()
    //{
    //    arrow = transform.GetChild(0).gameObject;

    //    itemRenderer = model.transform.GetComponentInChildren<Renderer>();

    //    if (itemRenderer != null)
    //    {
    //        originalColors = itemRenderer.sharedMaterials;
    //    }

    //    canvasTitle.GetComponentInChildren<Text>(true).text = title;
    //    canvasDescription.GetComponentInChildren<Text>(true).text = description;
    //}
    private void Start()
    {
        arrow = transform.GetChild(0).gameObject;

        itemRenderer = model.transform.GetComponentInChildren<Renderer>();

        // Saves the original material, then changes its color to gray.
        if (itemRenderer != null)
        {
            originalColors = itemRenderer.sharedMaterials;
            UpdateColor();
        }

        // Add this item to the List Manager Singlenton.
        if (ItemsManager.Instance != null)
        {
            ItemsManager.Instance.AddToList(this);
        }

        // Set up the canvas title and descrption.
        canvasTitle.GetComponentInChildren<Text>(true).text = title;
        canvasDescription.GetComponentInChildren<Text>(true).text = description;
    }


    /// <summary>
    /// Changes the current item's colors from their original colors to gray and vice versa.
    /// </summary>
    [ContextMenu("Changes the Item Color")]
    public void UpdateColor()
    {
        if (itemRenderer == null || grayColor == null)
        {
            Debug.Log("There is no itemrender or gray color attached");
            return;
        }

        if (isClicked == true) return;

        if (!isGray)
        {
            Material[] grayMaterials = new Material[originalColors.Length];

            for (int i = 0; i < grayMaterials.Length; i++)
            {
                grayMaterials[i] = grayColor;
            }

            itemRenderer.sharedMaterials = grayMaterials;
            isGray = true;
        }

        else
        {
            itemRenderer.sharedMaterials = originalColors;
            isGray = false;
        }

    }

    public void ShowCanvas(bool status)
    {
        if (isClicked == true && itemNumberCanvas == true)
        {
            if (canvasTitle != null)
            {
                canvasTitle.SetActive(status);

                canvasTitle.transform.LookAt(Camera.main.transform);
                canvasTitle.transform.Rotate(0f, 180f, 0f);
            }

            if (canvasDescription != null)
            {
                canvasDescription.SetActive(status);

                canvasDescription.transform.LookAt(Camera.main.transform);
                canvasDescription.transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    public void ShowArrow(bool status)
    {
        arrow.SetActive(status);
    }

    // Item is using AI Manager to play the "title" name
    public void PronunceName()
    {
        AIManager.Instance.PronunceItemName(title);
    }


    public void ItemClickSound()
    {
        AIManager.Instance.PlayClickSound();
    }

    public void VibrateOnClick()
    {

    }
    public void OnClicked(bool status)
    {
        isClicked = status;
    }

    public void ShowOutline(bool status)
    {

    }

    public void ShowItemNumber(float duration = 2)
    {
        if (isClicked == false)
        {
            // Animation
            StartCoroutine(ShowItemNumberCoroutine(duration));

            // Adds a point for the current list for example 1/15
            ItemsManager.Instance.AddScore();
            itemNumber.transform.GetComponentInChildren<Text>().text = ItemsManager.Instance.currentClicked + " / " + ItemsManager.Instance.items.Count;
        }
    }

    private IEnumerator ShowItemNumberCoroutine(float duration)
    {
        itemNumber.SetActive(true);

        yield return new WaitForSeconds(duration);

        itemNumber.SetActive(false);
        itemNumberCanvas = true;
        ShowCanvas(true);
    }
}
