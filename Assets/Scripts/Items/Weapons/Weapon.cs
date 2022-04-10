using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Core.Items.Weapons
{
    public class Weapon : Equippable
    {
        public Animator weaponAnimator;
        public string animationString;
        public float h;
        public float v;

        public override void Awake()
        {
            base.Awake();
            weaponAnimator = gameObject.GetComponent<Animator>();
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

        public override void UseItem()
        {
            base.UseItem();
        }
    }

}
