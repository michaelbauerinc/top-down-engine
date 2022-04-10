using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Items.Weapons.Melee
{
    public class MeleeWeapon : Weapon
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

        void FixedUpdate()
        {
        }

        public override void PickUpItem()
        {
            base.PickUpItem();

        }

        public override void UseItem()
        {
            base.UseItem();
        }
    }
}
