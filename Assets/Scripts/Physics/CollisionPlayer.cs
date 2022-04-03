using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Controllers;

namespace Core.Physics
{
    public class CollisionPlayer : MonoBehaviour
    {
        public Rigidbody2D body;
        public Animator animator;
        public SpriteRenderer sprite;
        public int hitStun = 10;
        List<GameObject> collisions = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (GameObject collision in collisions)
            {
                if (collision.gameObject.GetComponent<PlayerController>().isSliding())
                {
                    hitStun = 0;
                }
            }
        }

        private void FixedUpdate()
        {
            if (hitStun < 10)
            {
                animator.Play("hit_side");
                hitStun++;
                sprite.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
                if (hitStun == 10)
                {
                    animator.Play("idle_down");

                }

            }
            else
            {
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

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collisions.Remove(collision.gameObject);
            }
        }
    }
}
