using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Core.Items;
using Core.Items.Weapons;
using Core.Items.Weapons.Melee;
using Core.Items.Weapons.Ranged;

namespace Core.Controllers
{
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
        public MeleeWeapon equippedMeleeWeapon;
        public RangedWeapon equippedRangedWeapon;

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
            inventoryContent.Add(inventoryContent.Count, new Dictionary<string, dynamic>() { { "item", item } });
            if (item.canPickUp)
            {
                item.PickUpItem();

            }
            MapInventory();
        }

        public void UseItem(int indexToEquip)
        {
            Dictionary<string, dynamic> entry = inventoryContent[indexToEquip];
            dynamic toUse = entry["item"];
            toUse.UseItem();

            VisualElement slot = entry["slot"];
            slot.style.unityBackgroundImageTintColor = toUse.isEquipped ? new Color(255, 250, 0, 230) : new Color(0, 0, 0, 0);

            if (toUse.gameObject.GetComponent<Equippable>() != null)
            {
                if (toUse.gameObject.GetComponent<Weapon>() != null)
                {
                    if (toUse.gameObject.GetComponent<MeleeWeapon>() != null)
                    {
                        if (equippedMeleeWeapon == toUse)
                        {
                            equippedMeleeWeapon = null;
                        }
                        else
                        {
                            if (equippedMeleeWeapon)
                            {
                                // unequip weapon and toggle background
                                equippedMeleeWeapon.isEquipped = false;
                                VisualElement equippedMeleeWeaponSlot = inventoryContent[equippedMeleeWeapon.inventoryIndex]["slot"];
                                equippedMeleeWeaponSlot.style.unityBackgroundImageTintColor = new Color(0, 0, 0, 0);
                            }
                            equippedMeleeWeapon = toUse;
                        }
                    }
                    else if (toUse.gameObject.GetComponent<RangedWeapon>() != null)
                    {
                        if (equippedRangedWeapon == toUse)
                        {
                            equippedRangedWeapon = null;
                        }
                        else
                        {
                            if (equippedRangedWeapon)
                            {
                                // unequip weapon and toggle background
                                equippedRangedWeapon.isEquipped = false;
                                VisualElement equippedRangedWeaponSlot = inventoryContent[equippedRangedWeapon.inventoryIndex]["slot"];
                                equippedRangedWeaponSlot.style.unityBackgroundImageTintColor = new Color(0, 0, 0, 0);
                            }
                            equippedRangedWeapon = toUse;
                        }
                    }
                }
            }
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
                Item item = entry.Value["item"];
                item.inventoryIndex = entry.Key;
                button.style.backgroundImage = new StyleBackground(item.itemRenderer.sprite);

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
}
