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

        // public List<Item> inventoryContent = new List<Item>();
        // List<Item> inventoryContent = new List<Item>();

        // {
        //     0: {
        //         'item': <Item>,
        //         'slot': <VisualElement>,
        //         'slotContent': <VisualElement>
        //     }
        // }
        Dictionary<int, Dictionary<string, dynamic>> inventoryContent = new Dictionary<int, Dictionary<string, dynamic>>();
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
                    EquipWeapon(weapon);
                }
                else
                {
                    for (int i = 0; i < inventoryContent.Count; i++)
                    {
                        item.PickUpItem();
                        if (inventoryContent[i]["item"] == null)
                        {
                            inventoryContent[i]["item"] = item;
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
            Item toUse = inventoryContent[indexToDrop]["item"];
            if (toUse.destroyedOnUse == true)
            {
                inventoryContent[indexToDrop]["item"] = null;
            }
            toUse.UseItem();
            UpdateDefaultWindow();

        }

        private void DropItem(int indexToDrop)
        {
            Item itemToDrop = inventoryContent[indexToDrop]["item"];
            Dictionary<string, dynamic> toDrop = inventoryContent[indexToDrop];
            itemToDrop.DropItem();
            toDrop["item"] = null;
        }

        private void MapInventory()
        {
            for (int i = 0; i < inventoryContent.Count; i++)
            {
                Dictionary<string, dynamic> toMap = inventoryContent[i];

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
            for (int i = 0; i < 3; i++)
            {
                VisualElement slot = inventoryUi.Q<VisualElement>($"ItemSlot{i}");
                VisualElement slotContent = slot.Q<VisualElement>("ItemSlotContent");
                Dictionary<string, dynamic> toAdd = new Dictionary<string, dynamic>(){
                    {"item", null},
                    {"itemSlot", slot},
                    {"itemSlotContent", slotContent}
                };
                inventoryContent[i] = toAdd;
            }
        }

        private void GetSelectedItemIndex()
        {
            if (Mathf.Abs(playerController.rVertical) > 0 && inventoryContent.Count == 3 && inventoryContent[selectedItemIndex]["item"] != null)
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
            else if (Mathf.Abs(playerController.rHorizontal) > 0)
            {
                if (selectItemInputBuffer == selectItemInputBufferMax || selectItemInputBuffer < 0)
                {
                    selectedItemIndex += (int)playerController.rHorizontal;
                    if (selectedItemIndex < 0)
                    {
                        selectedItemIndex = inventoryContent.Count - 1;
                    }
                    else if (selectedItemIndex > inventoryContent.Count - 1)
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
        void FixedUpdate()
        {
            GetSelectedItemIndex();
        }
        void Update()
        {
        }
    }
}
