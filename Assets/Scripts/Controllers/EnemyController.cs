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
        public SpriteRenderer enemyRenderer;

        Dictionary<string, string> animationMappings = new Dictionary<string, string>(){
            {"idle", "idle"},
            {"hit", "hit"},
            {"walking", "walk"},
            {"jumping", "jump"},
            {"shooting", "player_bow"},
            {"sliding", "slide"},
            {"interacting", "idle"},
            {"meleeing", "sword"},
            {"dead", "dead"}
        };
        public float runSpeed = 5.0f;

        public float horizontal;
        public float vertical;

        public string deathSprite = "vanish_31";
        public string enemyAction;
        public int hitStun = 40;
        public int hitStunMax = 40;
        public bool hasInvincibilityFrames;
        public int invincibilityFrames = 40;
        public int invincibilityFramesMax = 40;
        public string currentDirection = "down";
        public int actionSleepTime = 0;


        // Start is called before the first frame update
        void Start()
        {
            body = gameObject.GetComponent<Rigidbody2D>();
            enemyRenderer = gameObject.GetComponent<SpriteRenderer>();
            animator = gameObject.GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            SetEnemyAction();
            Animate();
            // if (health <= 0 && enemyRenderer.sprite.name == deathSprite)
            // {
            //     Destroy(gameObject);
            // }
        }

        void FixedUpdate()
        {
            if (hitStun == hitStunMax - 1)
            {
                hasInvincibilityFrames = true;
            }
            else if (hasInvincibilityFrames && invincibilityFrames == 0)
            {
                invincibilityFrames = invincibilityFramesMax;
                hasInvincibilityFrames = false;
            }
            else if (hasInvincibilityFrames)
            {
                invincibilityFrames--;
            }
            DoRandomMovement();
            body.velocity = new Vector2(runSpeed * horizontal, runSpeed * vertical);

            switch (enemyAction)
            {
                // case "shooting":
                //     shootFrames = playerAction == "shooting" ? shootFrames -= 1 : shootFramesMax;
                //     playerAction = shootFrames >= 0 ? "shooting" : "idle";
                //     break;
                // case "jumping":
                //     // canMove = true;
                //     slideFrames = 45;
                //     jumpFrames = jumpFrames > 0 ? jumpFrames -= 1 : jumpFramesMax;
                //     playerAction = jumpFrames == 0 ? "idle" : playerAction;
                //     break;
                // case "sliding":
                //     // canMove = false;
                //     slideFrames = slideFrames > 0 ? slideFrames -= 1 : slideFramesMax;
                //     playerAction = slideFrames == 0 ? "idle" : playerAction;

                //     break;
                // case "meleeing":
                //     // canMove = false;
                //     meleeFrames = meleeFrames > 0 ? meleeFrames -= 1 : meleeFramesMax;
                //     playerAction = meleeFrames == 0 ? "idle" : playerAction;
                //     break;
                // case "interacting":
                //     canMove = false;
                //     break;
                case "hit":
                    hitStun++;
                    // canMove = false;
                    break;
                case "dead":
                    // canMove = false;
                    if (enemyRenderer.sprite.name == deathSprite)
                    {
                        Destroy(gameObject);
                    }
                    break;
                default:
                    // canMove = true;
                    // slideFrames = slideFramesMax;
                    // jumpFrames = jumpFramesMax;
                    // shootFrames = shootFramesMax;
                    // meleeFrames = meleeFramesMax;
                    hitStun = hitStunMax;
                    break;
                    // if (hitStun < hitStunMax)
                    // {
                    // }
                    // if (hitStun < hitStunMax)
                    // {
                    //     animator.Play("hit_side");
                    //     hitStun++;
                    //     enemyRenderer.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
                    // }
                    // else if (health <= 0 && hitStun == hitStunMax)
                    // {
                    //     enemyRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                    //     animator.Play("vanish");
                    // }
                    // else
                    // {
                    //     animator.Play("idle_down");
                    //     body.velocity = new Vector2(0, 0);
                    //     enemyRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
                    // }
                    // body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);

            }
        }

        void Animate()
        {
            string prefix = animationMappings[enemyAction];
            string nextDirection = currentDirection;
            if (currentDirection != "side")
            {
                enemyRenderer.flipX = true;

                // if (isMeleeing())
                // {
                //     playerRenderer.flipX = false;
                // }
                // else
                // {
                //     playerRenderer.flipX = true;
                // }
            }
            // we will only pivot direction on the first frame of jump
            if (IsInHitstun())
            {
                nextDirection = currentDirection;
            }
            else if (horizontal > 0)
            {
                // if (jumpFrames > 30)
                // {
                //     enemyRenderer.flipX = true;
                // }
                enemyRenderer.flipX = true;

                nextDirection = "side";
            }
            else if (horizontal < 0)
            {
                // if (jumpFrames > 30)
                // {
                //     enemyRenderer.flipX = false;
                // }
                enemyRenderer.flipX = false;
                nextDirection = "side";
            }
            else if (vertical == 1f)
            {
                nextDirection = "up";
            }
            else if (vertical == -1f)
            {
                nextDirection = "down";
            }
            if (hasInvincibilityFrames)
            {
                enemyRenderer.color = new UnityEngine.Color(1f, 1f, 1f, .75f);
            }
            else if (prefix == "hit")
            {
                enemyRenderer.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
            }
            else
            {
                enemyRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
            if (prefix == "dead")
            {
                animator.Play("vanish");
                return;
            }
            currentDirection = nextDirection;
            animator.Play(prefix + "_" + currentDirection);
        }

        public bool IsInHitstun()
        {
            return hitStun < hitStunMax;
        }

        public bool isMoving(string direction = "")
        {
            float v = Mathf.Abs(vertical);
            float h = Mathf.Abs(horizontal);
            return direction != "diagonally" ? h > 0 | v > 0 : horizontal != 0 && vertical != 0;
        }

        private void DoRandomMovement()
        {

            if (actionSleepTime == 0)
            {
                vertical = Random.Range(-1, 2);
                horizontal = Random.Range(-1, 2);
                Debug.Log(vertical + " | " + horizontal);
                actionSleepTime = Random.Range(30, 120);
                if (isMoving("diagonally"))
                {
                    horizontal *= .7f;
                    vertical *= .7f;
                }

            }
            else
            {
                actionSleepTime--;
                // vertical = 0;
                // horizontal = 0;
            }


        }

        private void SetEnemyAction()
        {
            if (health <= 0 && hitStun == hitStunMax)
            {
                enemyAction = "dead";
            }
            else if (hitStun < hitStunMax)
            {
                enemyAction = "hit";
            }
            else
            {
                if (isMoving())
                {
                    enemyAction = "walking";
                }
                else
                {
                    enemyAction = "idle";
                }
            }
        }
    }
}
