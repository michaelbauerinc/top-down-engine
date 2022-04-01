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
        inventoryContent.Add(item);
        MapInventory();
        item.pickedUp = true;
    }

    public void EquipItem(int indexToEquip)
    {
        inventoryContent[indexToEquip].isEquipped = true;
        inventoryContent[indexToEquip].gameObject.SetActive(true);
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        gameObject.transform.position = playerPos;
        MapInventory();
    }

    private void MapInventory()
    {
        VisualElement inventoryContainer = inventory.Q<VisualElement>("InventoryContainer");
        int i = 0;
        int j = 0;
        foreach (Item item in inventoryContent)
        {

            var row = inventoryContainer.ElementAt(i);
            Button slotToFill = row.ElementAt(j).Q<Button>("InventoryContent");
            slotToFill.clicked += delegate { EquipItem(i); };
            slotToFill.style.backgroundImage = new StyleBackground(item.image);
            if (item.isEquipped == true)
            {
                slotToFill.style.backgroundColor = new Color(229, 255, 0, 0.5f);
            }
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
