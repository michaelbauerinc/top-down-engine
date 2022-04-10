using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Items.Weapons.Ranged;
using Core.Controllers;

namespace Core.Physics
{
    public class CollisionBasic : MonoBehaviour
    {
        public Rigidbody2D body;
        public Animator animator;
        public SpriteRenderer sprite;
        public int hitStun = 40;
        List<GameObject> collisions = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();
            body = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (GameObject collision in collisions)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player.isSliding())
                {
                    hitStun = 30;
                }
                else if (player.isMeleeing())
                {
                    hitStun = 30;
                }
            }
        }

        private void FixedUpdate()
        {
            if (hitStun < 40)
            {
                animator.Play("hit_side");
                hitStun++;
                sprite.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
            }
            else
            {
                animator.Play("idle_down");
                body.velocity = new Vector2(0, 0);
                sprite.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collisions.Add(collision.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isAmmo = other.gameObject.GetComponent<Ammo>() != null;
            if (isAmmo)
            {
                hitStun = 0;

            }
        }

        // void OnTriggerEnter2D(Collision2D collision) {
        //     if (collision.gameObject.tag == "env") {
        //         return;
        //         // collisions.Remove(collision.gameObject);
        //     }
        // }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collisions.Remove(collision.gameObject);
            }
        }
    }
}
