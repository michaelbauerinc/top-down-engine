using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using Core.Environment;
using UnityEngine;

namespace Core.Items.Consumables
{
    public class Consumable : Item
    {
        public override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
        }

        public override void PickUpItem()
        {
            base.PickUpItem();
        }
        public override void DropItem()
        {
            base.DropItem();
        }

        public override void UseItem()
        {
            base.UseItem();
        }
    }

}
