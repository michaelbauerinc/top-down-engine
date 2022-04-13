using System.Collections;
using System.Collections.Generic;
using Core.Physics;
using UnityEngine;

namespace Core.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public int health = 10;
        public Animator animator;
        public Rigidbody2D body;
        public SpriteRenderer sprite;

        public string deathSprite = "vanish_31";
        public int hitStun = 40;


        // Start is called before the first frame update
        void Start()
        {
            body = gameObject.GetComponent<Rigidbody2D>();
            sprite = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            if (health <= 0 && sprite.sprite.name == deathSprite)
            {
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            if (hitStun < 40)
            {
            }
            if (hitStun < 40)
            {
                animator.Play("hit_side");
                hitStun++;
                sprite.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
            }
            else if (health <= 0 && hitStun == 40)
            {
                sprite.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                animator.Play("vanish");
            }
            else
            {
                animator.Play("idle_down");
                body.velocity = new Vector2(0, 0);
                sprite.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
