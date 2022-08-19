using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Core.Items;
using Core.Items.Weapons;
using Core.Items.Weapons.Melee;
using Core.Items.Weapons.Ranged;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Controllers
{
    public class UIController : MonoBehaviour
    {
        VisualElement rootContainer;
        VisualElement playerUi;
        VisualElement interactionWindow;
        VisualElement inventoryUi;
        VisualElement defaultUi;
        VisualElement playerHealthBar;
        Label playerCurrencyValue;

        public struct Inventory
        {
            public int totalHeldItems;
            public Dictionary<int, Dictionary<string, dynamic>> content;
        }
        public Inventory inventory;
        public int selectedItemIndex = 0;
        private int selectItemInputBuffer = 10;
        private int selectItemInputBufferMax = 10;
        public bool inventoryOpen = false;
        public MeleeWeapon equippedMeleeWeapon;
        public RangedWeapon equippedRangedWeapon;
        PlayerController playerController;
        Camera cam;

        private void Awake()
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            cam = GameObject.Find("Camera").GetComponent<Camera>();
            playerUi = gameObject.transform.GetChild(0).gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");
            defaultUi = playerUi.Q<VisualElement>("Default");
            inventoryUi = playerUi.Q<VisualElement>("Items");

            VisualElement playerPortraitContent = playerUi.Q<VisualElement>("PlayerPortraitContent");
            playerPortraitContent.style.backgroundImage = new StyleBackground(playerController.gameObject.GetComponent<SpriteRenderer>().sprite);

            interactionWindow = gameObject.transform.GetChild(1).gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Root");

            playerHealthBar = playerUi.Q<VisualElement>("PlayerHealthBar");
            playerCurrencyValue = playerUi.Q<Label>("PlayerMoneyValue");

            UpdateDefaultWindow();
            InitInventory();
            GetSelectedItemIndex();
        }

        void Start()
        {
        }

        public void ToggleUi()
        {
            defaultUi.ToggleInClassList("hidden");
            inventoryUi.ToggleInClassList("hidden");
        }

        public void ToggleInteractionBox(
            string interactionText = "",
            Sprite interactionImage = null
        )
        {
            interactionWindow.ElementAt(0).style.backgroundImage =
                new StyleBackground(interactionImage);
            interactionWindow.ElementAt(1).Q<Label>("Text").text =
                interactionText;
            interactionWindow.ToggleInClassList("hidden");
        }

        public void UpdateDefaultWindow()
        {
            UpdatePlayerHealthBar();
            UpdatePlayerCurrency();
        }

        public void UpdatePlayerCurrency()
        {
            playerCurrencyValue.text = playerController.currency.ToString();

        }

        public void UpdatePlayerHealthBar()
        {
            float healthPercent = (float)playerController.health / playerController.maxHealth * 80;
            playerHealthBar.style.width = new Length(healthPercent, LengthUnit.Percent);
        }

        public void PickUpItem(Item item)
        {
            if (item.canPickUp)
            {
                Weapon weapon = item.gameObject.GetComponent<Weapon>();
                if (weapon != null)
                {
                    item.PickUpItem();
                    EquipWeapon(weapon);
                }
                else if (inventory.totalHeldItems < inventory.content.Count)
                {
                    item.PickUpItem();
                    inventory.totalHeldItems++;
                    for (int i = 0; i < inventory.content.Count; i++)
                    {
                        if (inventory.content[i]["item"] == null)
                        {
                            inventory.content[i]["item"] = item;
                            break;
                        }
                    }
                }
            }
            MapInventory();
        }

        public void EquipWeapon(Weapon weapon)
        {
            weapon.PickUpItem();
            weapon.isEquipped = true;
            weapon.gameObject.transform.SetParent(playerController.gameObject.transform);
            MeleeWeapon meleeWeapon = weapon.gameObject.GetComponent<MeleeWeapon>();
            RangedWeapon rangedWeapon = weapon.gameObject.GetComponent<RangedWeapon>();

            if (meleeWeapon != null)
            {
                equippedMeleeWeapon =
                    weapon.gameObject.GetComponent<MeleeWeapon>();
            }
            else if (rangedWeapon != null)
            {
                equippedRangedWeapon = rangedWeapon;
            }
        }

        private void UseItem(int indexToDrop)
        {
            Item toUse = inventory.content[indexToDrop]["item"];
            if (toUse.destroyedOnUse == true)
            {
                inventory.content[indexToDrop]["item"] = null;
                GoToNextNextAvailableItem();
                inventory.totalHeldItems--;
            }
            toUse.UseItem();
            UpdateDefaultWindow();

        }

        private void DropItem(int indexToDrop)
        {
            Item itemToDrop = inventory.content[indexToDrop]["item"];
            Dictionary<string, dynamic> toDrop = inventory.content[indexToDrop];
            itemToDrop.DropItem();
            toDrop["item"] = null;
            inventory.totalHeldItems--;
            GoToNextNextAvailableItem();
        }

        private void MapInventory()
        {
            for (int i = 0; i < inventory.content.Count; i++)
            {
                Dictionary<string, dynamic> toMap = inventory.content[i];
                VisualElement slot = toMap["itemSlot"];
                VisualElement slotContent = toMap["itemSlotContent"];

                if (toMap["item"] != null)
                {
                    slotContent.style.backgroundImage = new StyleBackground(toMap["item"].itemRenderer.sprite);
                }
                else
                {
                    slotContent.style.backgroundImage = null;

                }

                if (i == selectedItemIndex)
                {
                    slot.AddToClassList("item-slot-selected");
                }
                else
                {
                    slot.RemoveFromClassList("item-slot-selected");
                }
            }
        }

        private void InitInventory()
        {
            Dictionary<int, Dictionary<string, dynamic>> content = new Dictionary<int, Dictionary<string, dynamic>>();
            for (int i = 0; i < 3; i++)
            {
                VisualElement slot = inventoryUi.Q<VisualElement>($"ItemSlot{i}");
                VisualElement slotContent = slot.Q<VisualElement>("ItemSlotContent");
                Dictionary<string, dynamic> toAdd = new Dictionary<string, dynamic>(){
                    {"item", null},
                    {"itemSlot", slot},
                    {"itemSlotContent", slotContent}
                };
                content[i] = toAdd;
            }
            inventory.content = content;
        }

        private void GetSelectedItemIndex()
        {
            if (Mathf.Abs(playerController.rHorizontal) + Mathf.Abs(playerController.rVertical) != 0)
            {
                if (selectItemInputBuffer == selectItemInputBufferMax || selectItemInputBuffer < 0)
                {
                    if (Mathf.Abs(playerController.rVertical) > 0 && inventory.content[selectedItemIndex]["item"] != null)
                    {
                        if (playerController.rVertical > 0)
                        {
                            UseItem(selectedItemIndex);
                        }
                        else if (playerController.rVertical < 0)
                        {
                            DropItem(selectedItemIndex);
                        }
                        MapInventory();
                    }
                    selectedItemIndex += (int)playerController.rHorizontal;
                    if (selectedItemIndex < 0)
                    {
                        selectedItemIndex = inventory.content.Count - 1;
                    }
                    else if (selectedItemIndex > inventory.content.Count - 1)
                    {
                        selectedItemIndex = 0;
                    }
                    MapInventory();
                }

                selectItemInputBuffer--;
            }
            else
            {
                selectItemInputBuffer = selectItemInputBufferMax;
            }
        }

        private void GoToNextNextAvailableItem()
        {
            selectedItemIndex = 0;
            for (int i = 0; i < inventory.content.Count; i++)
            {
                if (inventory.content[i]["item"] != null)
                {
                    selectedItemIndex = i;
                    break;
                }
            }
        }
        void FixedUpdate()
        {
            GetSelectedItemIndex();
        }
        void Update()
        {
        }
    }
}
