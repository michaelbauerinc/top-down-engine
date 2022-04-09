using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Items.Weapons.Melee
{
    public class MeleeWeapon : Weapon
    {
        static Dictionary<string, float[]> idleSideMappings = new Dictionary<string, float[]>(){
        {"21", new float[2]{.15f, .65f}},
        {"22", new float[2]{.15f, .65f}},
        {"23", new float[2]{.15f, .65f}},
        {"24", new float[2]{.15f, .65f}},
        {"25", new float[2]{.15f, .64f}},
        {"26", new float[2]{.15f, .63f}},
        {"27", new float[2]{.15f, .62f}},
        {"28", new float[2]{.15f, .61f}},
        {"29", new float[2]{.15f, .60f}},
        {"30", new float[2]{.15f, .60f}},
        {"31", new float[2]{.15f, .60f}},
        {"32", new float[2]{.15f, .60f}},
        {"33", new float[2]{.15f, .61f}},
        {"34", new float[2]{.15f, .61f}},
        {"35", new float[2]{.15f, .62f}},
        {"36", new float[2]{.15f, .63f}},
        {"37", new float[2]{.15f, .64f}},
        {"38", new float[2]{.15f, .65f}},
        {"39", new float[2]{.15f, .65f}},
        {"40", new float[2]{.15f, .65f}},
        {"41", new float[2]{.15f, .65f}}};

        static Dictionary<string, float[]> idleDownMappings = new Dictionary<string, float[]>(){
        {"_0", new float[2]{-0.48f, 0.82f}},
        {"_1", new float[2]{-0.48f, 0.82f}},
        {"_2", new float[2]{-0.48f, 0.82f}},
        {"_3", new float[2]{-0.48f, 0.82f}},
        {"_4", new float[2]{-0.48f, 0.81f}},
        {"_5", new float[2]{-0.48f, 0.80f}},
        {"_6", new float[2]{-0.48f, 0.79f}},
        {"_7", new float[2]{-0.48f, 0.78f}},
        {"_8", new float[2]{-0.48f, 0.77f}},
        {"_9", new float[2]{-0.48f, 0.77f}},
        {"10", new float[2]{-0.48f, 0.77f}},
        {"11", new float[2]{-0.48f, 0.77f}},
        {"12", new float[2]{-0.48f, 0.77f}},
        {"13", new float[2]{-0.48f, 0.78f}},
        {"14", new float[2]{-0.48f, 0.78f}},
        {"15", new float[2]{-0.48f, 0.79f}},
        {"16", new float[2]{-0.48f, 0.80f}},
        {"17", new float[2]{-0.48f, 0.82f}},
        {"18", new float[2]{-0.48f, 0.82f}},
        {"19", new float[2]{-0.48f, 0.82f}},
        {"20", new float[2]{-0.48f, 0.82f}}};

        static Dictionary<string, float[]> idleUpMappings = new Dictionary<string, float[]>(){
        {"42", new float[2]{0.41f, 0.8f}},
        {"43", new float[2]{0.41f, 0.8f}},
        {"44", new float[2]{0.41f, 0.8f}},
        {"45", new float[2]{0.41f, 0.79f}},
        {"46", new float[2]{0.41f, 0.78f}},
        {"47", new float[2]{0.41f, 0.76f}},
        {"48", new float[2]{0.41f, 0.75f}},
        {"49", new float[2]{0.41f, 0.74f}},
        {"50", new float[2]{0.41f, 0.74f}},
        {"51", new float[2]{0.41f, 0.74f}},
        {"52", new float[2]{0.41f, 0.74f}},
        {"53", new float[2]{0.41f, 0.74f}},
        {"54", new float[2]{0.41f, 0.74f}},
        {"55", new float[2]{0.41f, 0.75f}},
        {"56", new float[2]{0.41f, 0.76f}},
        {"57", new float[2]{0.41f, 0.77f}},
        {"58", new float[2]{0.41f, 0.79f}},
        {"59", new float[2]{0.41f, 0.8f}},
        {"60", new float[2]{0.41f, 0.8f}},
        {"61", new float[2]{0.41f, 0.8f}},
        {"62", new float[2]{0.41f, 0.8f}}};

        static Dictionary<string, Dictionary<string, float[]>> idleMappings = new Dictionary<string, Dictionary<string, float[]>>(){
        {"side", idleSideMappings},
        {"up", idleUpMappings},
        {"down", idleDownMappings}};

        static Dictionary<string, float[]> walkSideMappings = new Dictionary<string, float[]>(){
        {"14", new float[2]{0.03f, 0.75f}},
        {"15", new float[2]{0.16f, 0.9f}},
        {"16", new float[2]{0.25f, 1f}},
        {"17", new float[2]{0.25f, 1.05f}},
        {"18", new float[2]{0.2f, 1f}},
        {"19", new float[2]{0f, 0.9f}},
        {"20", new float[2]{-0.3f, 0.9f}},
        {"21", new float[2]{-0.3f, 1.0f}},
        {"22", new float[2]{-0.4f, 1.1f}},
        {"23", new float[2]{-0.4f, 1.15f}},
        {"24", new float[2]{-0.4f, 1.18f}},
        {"25", new float[2]{-0.4f, 1.15f}},
        {"26", new float[2]{-0.3f, .95f}},
        {"27", new float[2]{0f, 0.75f}}};

        static Dictionary<string, float[]> walkDownMappings = new Dictionary<string, float[]>(){
        {"_0", new float[2]{-0.25f, 0.7f}},
        {"_1", new float[2]{0f, 0.85f}},
        {"_2", new float[2]{0.05f, 0.9f}},
        {"_3", new float[2]{0.05f, 0.95f}},
        {"_4", new float[2]{0.05f, 0.9f}},
        {"_5", new float[2]{-0.35f, 0.7f}},
        {"_6", new float[2]{-0.45f, 0.7f}},
        {"_7", new float[2]{-0.55f, 1f}},
        {"_8", new float[2]{-0.65f, 1.25f}},
        {"_9", new float[2]{-0.65f, 1.4f}},
        {"10", new float[2]{-0.65f, 1.45f}},
        {"11", new float[2]{-0.65f, 1.35f}},
        {"12", new float[2]{-0.65f, 1.1f}},
        {"13", new float[2]{-0.5f, 0.7f}}};

        static Dictionary<string, float[]> walkUpMappings = new Dictionary<string, float[]>(){
        {"28", new float[2]{0.41f, 0.85f}},
        {"29", new float[2]{0.65f, 1.3f}},
        {"30", new float[2]{0.65f, 1.55f}},
        {"31", new float[2]{0.65f, 1.6f}},
        {"32", new float[2]{0.65f, 1.4f}},
        {"33", new float[2]{0.41f, 0.85f}},
        {"34", new float[2]{0.35f, 0.75f}},
        {"35", new float[2]{0.41f, 1.1f}},
        {"36", new float[2]{0.41f, 1.2f}},
        {"37", new float[2]{0.35f, 1.3f}},
        {"38", new float[2]{0.35f, 1.4f}},
        {"39", new float[2]{0.35f, 1.3f}},
        {"40", new float[2]{0.41f, 1.2f}},
        {"41", new float[2]{0.41f, 0.75f}}};


        static Dictionary<string, Dictionary<string, float[]>> walkMappings = new Dictionary<string, Dictionary<string, float[]>>(){
        {"side", walkSideMappings},
        {"up", walkUpMappings},
        {"down", walkDownMappings}};



        float h;
        float v;
        Dictionary<string, Dictionary<string, Dictionary<string, float[]>>> animationMappings = new Dictionary<string, Dictionary<string, Dictionary<string, float[]>>>(){
        {"walking", walkMappings},
        {"idle", idleMappings}};

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
            // AnimateWeapon();
            if (isEquipped)
            {
                AnimateWeapon();
            }
            else if (pickedUp)
            {
                itemRenderer.enabled = false;
            }
        }

        void FixedUpdate()
        {

        }

        public void AnimateWeapon()
        {
            string playerAction = playerController.playerAction;

            if (!playerController.isJumping() && !playerController.isSliding() && !playerController.isShooting())
            {
                string currentDirection = playerController.currentDirection;
                Vector3 playerPos = playerController.gameObject.transform.position;

                string currentAnim = playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name.Substring(
                playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name.Length - 2);

                try
                {
                    h = animationMappings[playerAction][currentDirection][currentAnim][0];
                    v = animationMappings[playerAction][currentDirection][currentAnim][1];
                }
                catch (KeyNotFoundException)
                {
                    Debug.Log(playerAction);
                    Debug.Log(currentDirection);
                    Debug.Log(currentAnim);
                    Debug.Log(playerController.gameObject.GetComponent<SpriteRenderer>().sprite.name);
                }

                if (playerController.gameObject.GetComponent<SpriteRenderer>().flipX && currentDirection == "side")
                {
                    h = h * -1;
                }

                if (currentDirection == "up")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, -30);

                }
                else if (currentDirection == "down")
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 30);

                }
                else
                {
                    gameObject.transform.eulerAngles = new Vector3(0, 0, 0);

                }

                gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, currentDirection != "up" ? playerPos.z - 1 : playerPos.z + 1);
                itemRenderer.enabled = true;

                if (currentDirection == "down")
                {
                    itemRenderer.flipX = true;
                }
                else if (currentDirection == "up")
                {
                    itemRenderer.flipX = false;
                }
                else
                {
                    itemRenderer.flipX = playerController.gameObject.GetComponent<SpriteRenderer>().flipX;

                }
            }
            else
            {
                itemRenderer.enabled = false;
            }
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
