using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Items;
using Core.Environment;
using Core.Items.Weapons.Ranged;
using Core.Items.Weapons.Melee;

namespace Core.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D body;
        public Animator animator;
        UIController uiController;
        Interactable interactionTarget = null;
        Dictionary<string, string> animationMappings = new Dictionary<string, string>(){
            {"idle", "idle"},
            {"walking", "walk"},
            {"jumping", "jump"},
            {"shooting", "player_bow"},
            {"sliding", "slide"},
            {"interacting", "idle"},
            {"meleeing", "sword"}
        };
        public string playerAction = "idle";
        public string currentDirection = "down";
        // Movement
        public bool canMove = true;
        float moveLimiter = 0.7f;
        public float horizontal;
        public float vertical;
        public float runSpeed = 5.0f;
        // Action durations
        public int jumpFrames = 35;
        public int slideFrames = 45;
        public int shootFrames = 35;
        public int meleeFrames = 35;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            uiController = GameObject.Find("UI").GetComponent<UIController>();
        }
        void Start()
        {
        }

        void Animate()
        {
            string prefix = animationMappings[playerAction];
            string nextDirection = currentDirection;
            if (currentDirection != "side")
            {
                if (isMeleeing())
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;

                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;

                }
            }
            // we will only pivot direction on the first frame of jump
            if (isJumping() && jumpFrames < 35 || isSliding())
            {
                nextDirection = currentDirection;
            }
            else if (horizontal == 1f)
            {
                if (jumpFrames > 30)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
                nextDirection = "side";
            }
            else if (horizontal == -1f)
            {
                if (jumpFrames > 30)
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
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
            currentDirection = nextDirection;
            animator.Play(prefix + "_" + currentDirection);
        }

        private void setPlayerAction()
        {
            if (!isInteracting())
            {
                if (!isJumping() && Input.GetKeyDown("space") && !isMeleeing())
                {
                    playerAction = "jumping";
                }
                else if (!isSliding() && !isMeleeing() && !isJumping() && Input.GetKeyDown("m"))
                {
                    playerAction = "sliding";
                }
                else if (Input.GetKeyDown("n") && interactionTarget)
                {
                    if (interactionTarget.canInteract)
                    {
                        uiController.ToggleInteractionBox(interactionTarget.toSay, interactionTarget.itemRenderer.sprite);
                        playerAction = "interacting";
                    }
                }
                else if (Input.GetKeyDown("1") && !isShooting() && !isJumping() && uiController.currentWeapon != null)
                {
                    if (uiController.currentWeapon.isEquipped)
                    {
                        if (uiController.currentWeapon.GetComponent<Bow>() != null)
                        {
                            playerAction = "shooting";
                        }
                        else if (uiController.currentWeapon.GetComponent<MeleeWeapon>() != null)
                        {
                            playerAction = "meleeing";
                        }
                    }
                }
                else if (isMoving() && !isSliding() && !isShooting() && !isMeleeing() && !isJumping())
                {
                    playerAction = "walking";

                }
                else if (!isMoving() && !isSliding() && !isShooting() && !isMeleeing() && !isJumping())
                {
                    playerAction = "idle";
                }

            }
            else if (isInteracting() && Input.GetKeyDown("n"))
            {
                uiController.ToggleInteractionBox("");
                playerAction = "idle";
            }
        }

        void ShootWeapon()
        {
            // var test = uiController.currentWeapon.ammo;
        }

        void Update()
        {
            setPlayerAction();
            // Inventory is not open and we are not interacting
            if (canMove)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }
            switch (playerAction)
            {
                case "interacting":
                    bool isItem = interactionTarget.GetComponent<Item>() != null;
                    if (interactionTarget.canInteract)
                    {
                        if (isItem && interactionTarget.canPickUp)
                        {
                            uiController.AddItemToInventory(interactionTarget.GetComponent<Item>());
                        }
                        canMove = false;
                        horizontal = 0;
                        vertical = 0;
                    }
                    break;
                case "shooting":
                    if (shootFrames == 0)
                    {
                        ShootWeapon();
                    }
                    break;
                case "jumping":
                    break;
                case "sliding":
                    break;
                case "meleeing":
                    break;
                case "idle":
                    break;
                default:
                    break;
            }
            if (Input.GetKeyDown("return"))
            {
                uiController.inventoryOpen = !uiController.inventoryOpen;
                uiController.ToggleUi();
            }
            Animate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Interactable interactable = collision.gameObject.GetComponent<Interactable>();
            if (interactable && interactable.canInteract)
            {
                interactionTarget = interactable;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Interactable interactable = collision.gameObject.GetComponent<Interactable>();
            if (interactable && interactable.canInteract)
            {
                interactionTarget = interactable;
            }
        }


        private void OnCollisionExit2D(Collision2D collision)
        {
            interactionTarget = null;
        }
        private void FixedUpdate()
        {
            // We only want to apply this on the first frame of a melee attack
            if (isMoving("diagonally") && meleeFrames == 35 && slideFrames == 45)
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }
            // Sliding has inherent momentum is we're not moving, only apply on first frame
            if (isSliding() && slideFrames == 45 && !isMoving())
            {
                switch (currentDirection)
                {
                    case "up":
                        vertical = 1f;
                        break;
                    case "down":
                        vertical = -1f;
                        break;
                    default:
                        horizontal = gameObject.GetComponent<SpriteRenderer>().flipX ? 1f : -1f;
                        break;
                }
            }
            if (isJumping() || isMeleeing() || isShooting())
            {
                // log taper speed 
                double modifier = 1 - System.Math.Log(jumpFrames, 35) * .95;
                float v = Mathf.Sign(vertical) * (float)modifier;
                float h = Mathf.Sign(horizontal) * (float)modifier;
                horizontal -= Mathf.Abs(horizontal) > 0 ? h : 0;
                vertical -= Mathf.Abs(vertical) > 0 ? v : 0;
            }
            else if (isSliding())
            {
                // Initial speed increase with log taper
                float modifier = (float)System.Math.Log(slideFrames, 45) * 1.05f;
                float v = Mathf.Sign(vertical) * (float)modifier;
                float h = Mathf.Sign(horizontal) * (float)modifier;
                horizontal *= modifier;
                vertical *= modifier;
            }
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            switch (playerAction)
            {
                case "shooting":
                    shootFrames = playerAction == "shooting" ? shootFrames -= 1 : 35;
                    playerAction = shootFrames >= 0 ? "shooting" : "idle";
                    break;
                case "jumping":
                    canMove = true;
                    slideFrames = 45;
                    jumpFrames = jumpFrames > 0 ? jumpFrames -= 1 : 35;
                    playerAction = jumpFrames == 0 ? "idle" : playerAction;
                    break;
                case "sliding":
                    canMove = false;
                    slideFrames = slideFrames > 0 ? slideFrames -= 1 : 45;
                    playerAction = slideFrames == 0 ? "idle" : playerAction;

                    break;
                case "meleeing":
                    canMove = false;
                    meleeFrames = meleeFrames > 0 ? meleeFrames -= 1 : 35;
                    playerAction = meleeFrames == 0 ? "idle" : playerAction;
                    break;
                case "interacting":
                    canMove = false;
                    break;
                default:
                    canMove = true;
                    slideFrames = 45;
                    jumpFrames = 35;
                    shootFrames = 35;
                    meleeFrames = 35;
                    break;
            }
        }

        // Optional arg to get direction
        public bool isMoving(string direction = "")
        {
            float v = Mathf.Abs(vertical);
            float h = Mathf.Abs(horizontal);
            return direction != "diagonally" ? h > 0 | v > 0 : horizontal != 0 && vertical != 0;
        }

        public bool isJumping()
        {
            return playerAction == "jumping";
        }

        public bool isSliding()
        {
            return playerAction == "sliding";
        }

        public bool isInteracting()
        {
            return playerAction == "interacting";
        }

        public bool isShooting()
        {
            return playerAction == "shooting";
        }

        public bool isMeleeing()
        {
            return playerAction == "meleeing";
        }
    }
}