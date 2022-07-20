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

        public List<Item> inventoryContent = new List<Item>();
        public int selectedItem = 0;
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
            float healthPercent = (float)playerController.health / playerController.maxHealth * 80;
            playerHealthBar.style.width = new Length(healthPercent, LengthUnit.Percent);

            playerCurrencyValue = playerUi.Q<Label>("PlayerMoneyValue");
            playerCurrencyValue.text = playerController.currency.ToString();
            GetSelectedItem();
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

        public void PickUpItem(Item item)
        {
            if (item.canPickUp)
            {
                Weapon weapon = item.gameObject.GetComponent<Weapon>();
                if (weapon != null)
                {
                    EquipWeapon(weapon);
                }
                else if (inventoryContent.Count < 3)
                {
                    item.PickUpItem();
                    inventoryContent.Add(item);
                }
            }
            MapInventory();
        }

        public void EquipWeapon(Weapon weapon)
        {
            weapon.PickUpItem();
            weapon.isEquipped = true;
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

        private void MapInventory()
        {
            for (int i = 0; i < inventoryContent.Count; i++)
            {
                VisualElement toHighlight = inventoryUi.Q<VisualElement>($"ItemSlot{i}");
                VisualElement toMap = toHighlight.Q<VisualElement>("ItemSlotContent");
                toMap.style.backgroundImage = new StyleBackground(inventoryContent[i].itemRenderer.sprite);
                if (i == selectedItem)
                {
                    toHighlight.AddToClassList("item-slot-selected");

                }
                else
                {
                    toHighlight.RemoveFromClassList("item-slot-selected");
                }
            }
        }

        private void GetSelectedItem()
        {
            if (Mathf.Abs(playerController.rHorizontal) > 0)
            {
                if (selectItemInputBuffer == selectItemInputBufferMax || selectItemInputBuffer < 0)
                {
                    selectedItem += (int)playerController.rHorizontal;
                    if (selectedItem < 0)
                    {
                        selectedItem = 2;
                    }
                    else if (selectedItem > 2)
                    {
                        selectedItem = 0;
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
            GetSelectedItem();
        }
        void Update()
        {


            // GetSelectedItem()
            // Vector3 screenPos = cam.WorldToScreenPoint(playerController.gameObject.transform.position);
            // bool playerUnderUiX = Screen.width - screenPos.x < playerUi.localBound.width && Screen.width - screenPos.x > Screen.width * .1;
            // // Buffer frames on top cuz player env collider is on feet
            // bool playerUnderUiY = Screen.height - screenPos.y < Screen.height * .9 + Screen.height * .06 && Screen.height - screenPos.y > playerUi.localBound.y - Screen.height * .1;
            // if (playerUnderUiX && playerUnderUiY)
            // {
            //     playerUi.AddToClassList("faded");
            // }
            // else
            // {
            //     playerUi.RemoveFromClassList("faded");
            // }
        }
    }
}
