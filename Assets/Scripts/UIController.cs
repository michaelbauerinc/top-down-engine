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
    // List<Item> inventoryContent = new List<Item>();

    // {
    //     0: {
    //         'item': <Item>,
    //         'slot': <Button>
    //     }
    // }
    Dictionary<int, Dictionary<string, dynamic>> inventoryContent = new Dictionary<int, Dictionary<string, dynamic>>();
    public bool inventoryOpen = false;
    public bool weaponEquipped = false;
    public GameObject ammo;


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
        inventoryContent.Add(inventoryContent.Count, new Dictionary<string, dynamic>(){
        {"item", item}});
        MapInventory();
        item.pickedUp = true;
    }

    public void UseItem(int indexToEquip)
    {
        var entry = inventoryContent[indexToEquip];
        Item toUse = entry["item"];
        VisualElement slot = entry["slot"];
        toUse.isEquipped = !toUse.isEquipped;
        weaponEquipped = toUse.category == "weapon" ? !weaponEquipped : false;
        slot.style.unityBackgroundImageTintColor = toUse.isEquipped ? new Color(255, 250, 0, 230) : new Color(0, 0, 0, 0);
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        gameObject.transform.position = playerPos;
        toUse.gameObject.SetActive(true);
    }

    private void MapInventory()
    {
        VisualElement inventoryContainer = inventory.Q<VisualElement>("InventoryContainer");
        int i = 0;
        int j = 0;
        foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in inventoryContent)
        {
            VisualElement row = inventoryContainer.ElementAt(i);
            Button button = row.ElementAt(j).Q<Button>("InventoryContent");
            VisualElement slotToFill = row.ElementAt(j);

            // Only delegate the clickhandler when a new item is added to the inventory
            if (!entry.Value.ContainsKey("slot"))
            {
                button.clicked += delegate { UseItem(entry.Key); };
            }
            entry.Value["slot"] = slotToFill;
            // Button backgroundToModify = entry.Value["slot"];
            button.style.backgroundImage = new StyleBackground(entry.Value["item"].image);
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
