using System.Collections;
using System.Collections.Generic;
using Core.Items;
using UnityEngine;

namespace Core.Items.Carryables
{
    public class Carryable : Item
    {
        public bool beingCarried = false;

        public override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void Throw()
        {

        }

        public override void PickUpItem()
        {
            base.PickUpItem();
        }

        public override void UseItem()
        {
            beingCarried = true;
            base.UseItem();
        }

        public override void DropItem()
        {
            beingCarried = false;
            base.DropItem();
        }
    }

}
