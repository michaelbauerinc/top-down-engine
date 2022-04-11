using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utils;

namespace Core.Physics
{
    public class CollisionBasic : MonoBehaviour
    {
        public Rigidbody2D body;
        public Animator animator;
        public SpriteRenderer sprite;
        // PlayerController player;
        public int hitStun = 40;
        // List<GameObject> collisions = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();
            body = gameObject.GetComponent<Rigidbody2D>();
            // player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {

            // Debug.Log(collisions.AsEnumerable().OfType<GameObject>().Any());
            // if (collisions.AsEnumerable().OfType<Weapon>().Any())
            // {
            //     Debug.Log("hit");
            // }
            // foreach (GameObject collision in collisions)
            // {
            //     MultiTag tags = collision.gameObject.GetComponent<MultiTag>();
            //     if (tags.HasTag("hurtbox"))
            //     {
            //         hitStun = 30;
            //     }

            //     // PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            //     // if (player.isSliding())
            //     // {
            //     //     hitStun = 30;
            //     // }
            //     // else if ()
            //     // {
            //     //     hitStun = 30;
            //     // }
            // }
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

        // void OnCollisionEnter2D(Collision2D collision)
        // {
        //     if (collision.gameObject.tag == "Player")
        //     {
        //         collisions.Add(collision.gameObject);
        //     }
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MultiTag tags = other.gameObject.GetComponent<MultiTag>();
            if (tags.HasTag("hurtbox"))
            {
                hitStun = 0;
            }
        }

        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (collisions.Contains(other.gameObject))
        //     {
        //         collisions.Remove(other.gameObject);
        //     }
        // }

        // void OnCollisionExit2D(Collision2D collision)
        // {
        //     if (collisions.Contains(collision.gameObject))
        //     {
        //         collisions.Remove(collision.gameObject);
        //     }
        // }
    }
}
