using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Items.Weapons.Ranged
{
    public class RangedWeapon : Weapon
    {

        public Ammo ammo;

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
