using System.Collections;
using System.Runtime;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    VisualElement rootContainer;
    VisualElement inventory;
    VisualElement interactionWindow;
    List<Item> inventoryContent = new List<Item>();


    private void Awake()
    {
        inventory = gameObject.transform.GetChild(0).gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
        interactionWindow = gameObject.transform.GetChild(1).gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
    }

    void Start()
    {
    }

    public void ToggleUi()
    {
        inventory.ToggleInClassList("hidden");
    }
    public void ToggleInteractionBox(string interactionText = "", Sprite interactionImage = null)
    {
        interactionWindow.ElementAt(0).style.backgroundImage = new StyleBackground(interactionImage);
        interactionWindow.ElementAt(1).Q<Label>("Text").text = interactionText;
        interactionWindow.ToggleInClassList("hidden");
    }

    public void AddItemToInventory(Item item)
    {
        // gameObject.AddComponent<Item>(new Item(item));
        // UnityEditorInternal.ComponentUtility.CopyComponent(item);
        // Debug.Log(UnityEditorInternal.ComponentUtility.

        // inventoryContent.Add(gameObject.GetComponent<Item>());
        inventoryContent.Add(item);
        MapInventory();
        item.pickedUp = true;
    }

    public void EquipItem()
    {
        inventoryContent[0].isEquipped = true;
    }

    private void MapInventory()
    {
        VisualElement inventoryContainer = inventory.Q<VisualElement>("InventoryContainer");
        // int totalRows = inventoryContainer.childCount;
        // int totalItems = inventoryContent.Count;
        int i = 0;
        int j = 0;
        foreach (Item item in inventoryContent)
        {
            var row = inventoryContainer.ElementAt(i);
            Button slotToFill = row.ElementAt(j).Q<Button>("InventoryContent");
            // slotToFill.clicked += EquipItem;
            slotToFill.style.backgroundImage = new StyleBackground(item.image);
            j++;
            if (j == row.childCount)
            {
                i++;
                j = 0;
            }
        }
    }

    void Update()
    {
    }
}
