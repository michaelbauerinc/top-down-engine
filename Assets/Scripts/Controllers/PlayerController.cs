using System.Collections;
using System.Collections.Generic;
using Core.Environment;
using Core.Items;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Core.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D body;
        public Animator animator;
        UIController uiController;
        SpriteRenderer playerRenderer;
        BoxCollider2D playerCollider;
        Interactable interactionTarget = null;
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
        public string playerAction = "idle";
        public string currentDirection = "down";
        public string deathSprite = "vanish_31";

        // Movement
        public bool canMove = true;
        public bool hasInvincibilityFrames = false;
        public float horizontalInput;
        public float verticalInput;
        public float horizontal;
        public float vertical;
        public float rHorizontal;
        public float rVertical;
        public float runSpeed = 5.0f;
        // Action durations
        public int jumpFrames = 35;
        public int jumpFramesMax = 35;
        public int slideFrames = 45;
        public int slideFramesMax = 45;

        public int shootFramesMax = 35;
        public int shootFrames = 35;
        public int meleeFrames = 35;
        public int meleeFramesMax = 30;

        // stats
        public int health = 10;
        public int maxHealth = 10;
        public int currency = 100;
        public int hitStun = 40;
        public int hitStunMax = 40;
        public int invincibilityFrames = 40;
        public int invincibilityFramesMax = 40;


        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            uiController = GameObject.Find("UI").GetComponent<UIController>();
            playerRenderer = gameObject.GetComponent<SpriteRenderer>();
            BoxCollider2D playerCollider = gameObject.GetComponent<BoxCollider2D>();
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
                    playerRenderer.flipX = false;
                }
                else
                {
                    playerRenderer.flipX = true;
                }
            }
            // we will only pivot direction on the first frame of jump
            if (isJumping() && jumpFrames < 35 || isSliding() || IsInHitstun())
            {
                nextDirection = currentDirection;
            }
            else if (horizontal > 0 && !isMeleeing() && !isShooting())
            {
                if (jumpFrames > 30)
                {
                    playerRenderer.flipX = true;
                }
                nextDirection = "side";
            }
            else if (horizontal < 0 && !isMeleeing() && !isShooting())
            {
                if (jumpFrames > 30)
                {
                    playerRenderer.flipX = false;
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
            if (hasInvincibilityFrames)
            {
                playerRenderer.color = new UnityEngine.Color(1f, 1f, 1f, .75f);
            }
            else if (prefix == "hit")
            {
                playerRenderer.color = new UnityEngine.Color(0.97f, 0.02f, 0.02f, 1f);
            }
            else
            {
                playerRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 1f);
            }
            if (prefix == "dead")
            {
                animator.Play("vanish");
                return;
            }
            currentDirection = nextDirection;
            animator.Play(prefix + "_" + currentDirection);
        }

        private void setPlayerAction()
        {
            if (health <= 0 && hitStun == hitStunMax)
            {
                playerAction = "dead";
            }
            else if (hitStun < hitStunMax)
            {
                playerAction = "hit";
            }
            else if (!isInteracting() && !isMeleeing() && !isShooting() && !isJumping() && !isSliding())
            {
                if (canMove && isMoving())
                {
                    playerAction = "walking";
                }
                else if (!isMoving())
                {
                    playerAction = "idle";
                }
            }
        }

        void Update()
        {
            setPlayerAction();
            if (!isInteracting())
            {
                uiController.ToggleInteractionBox("", null, true);
            }
            switch (playerAction)
            {
                case "interacting":
                    bool isItem = interactionTarget != null && interactionTarget.GetComponent<Item>() != null;
                    if (interactionTarget != null && interactionTarget.canInteract)
                    {
                        if (isItem && interactionTarget.canPickUp)
                        {
                            uiController.PickUpItem(interactionTarget.GetComponent<Item>());
                        }
                        canMove = false;
                        horizontal = 0;
                        vertical = 0;
                    }
                    break;
                case "shooting":
                    break;
                case "jumping":
                    break;
                case "sliding":
                    break;
                case "meleeing":
                    break;
                case "idle":
                    break;
                case "hit":
                    break;
                case "dead":
                    break;
                default:
                    break;
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

        public void Jump()
        {
            if (!isJumping() && !isMeleeing())
            {
                playerAction = "jumping";

            }
        }

        public void Slide()
        {
            if (!isSliding() && !isMeleeing() && !isJumping() && !isShooting())
            {
                playerAction = "sliding";

            }
        }

        public void Interact(InputAction.CallbackContext value)
        {
            if (value.started)
            {
                if (isInteracting())
                {
                    uiController.ToggleInteractionBox("");
                    playerAction = "idle";
                }
                else if (interactionTarget)
                {
                    if (interactionTarget.canInteract)
                    {
                        uiController.ToggleInteractionBox(interactionTarget.toSay, interactionTarget.itemRenderer.sprite);
                        playerAction = "interacting";
                    }
                }
            }

        }

        public void Melee()
        {
            if (!isMeleeing() && !isShooting() && !isJumping() && uiController.equippedMeleeWeapon != null)
            {
                playerAction = "meleeing";
            }
        }

        public void ToggleUi()
        {
            uiController.ToggleUi();
        }

        public void Shoot()
        {
            if (shootFrames == shootFramesMax && !isShooting() && !isMeleeing() && !isJumping() && uiController.equippedRangedWeapon != null)
            {
                playerAction = "shooting";
            }
        }

        public void GetMovement(InputAction.CallbackContext value)
        {
            if (value.canceled)
            {
                verticalInput = 0;
                horizontalInput = 0;
            }
            else if (canMove)
            {
                Vector2 vector = value.ReadValue<Vector2>();
                horizontalInput = vector.x;
                verticalInput = vector.y;
            }
        }

        public void GetRightStick(InputAction.CallbackContext value)
        {
            if (canMove)
            {
                Vector2 vector = value.ReadValue<Vector2>();
                rHorizontal = vector.x;
                rVertical = vector.y;
            }
        }

        public void ReloadScene(InputAction.CallbackContext value)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        private void FixedUpdate()
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

            // Sliding has inherent momentum is we're not moving, only apply on first frame
            if (isSliding() && slideFrames == slideFramesMax && !isMoving())
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
                        horizontal = playerRenderer.flipX ? 1f : -1f;
                        break;
                }
            }
            else if (isJumping() || isMeleeing() || isShooting() || IsInHitstun())
            {
                // log taper speed 
                if (isJumping())
                {
                    horizontal = horizontalInput;
                    vertical = verticalInput;
                }
                double modifier = 1 - System.Math.Log(jumpFrames, jumpFramesMax) * .95;
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
            else if (!isInteracting())
            {
                horizontal = horizontalInput;
                vertical = verticalInput;
            }
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            switch (playerAction)
            {
                case "shooting":
                    shootFrames = playerAction == "shooting" ? shootFrames -= 1 : shootFramesMax;
                    playerAction = shootFrames >= 0 ? "shooting" : "idle";
                    break;
                case "jumping":
                    // canMove = true;
                    slideFrames = 45;
                    jumpFrames = jumpFrames > 0 ? jumpFrames -= 1 : jumpFramesMax;
                    playerAction = jumpFrames == 0 ? "idle" : playerAction;
                    break;
                case "sliding":
                    // canMove = false;
                    slideFrames = slideFrames > 0 ? slideFrames -= 1 : slideFramesMax;
                    playerAction = slideFrames == 0 ? "idle" : playerAction;

                    break;
                case "meleeing":
                    // canMove = false;
                    meleeFrames = meleeFrames > 0 ? meleeFrames -= 1 : meleeFramesMax;
                    playerAction = meleeFrames == 0 ? "idle" : playerAction;
                    break;
                case "interacting":
                    canMove = false;
                    break;
                case "hit":
                    hitStun++;
                    // canMove = false;
                    break;
                case "dead":
                    canMove = false;
                    if (playerRenderer.sprite.name == deathSprite)
                    {
                        Destroy(gameObject);
                    }
                    break;
                default:
                    canMove = true;
                    slideFrames = slideFramesMax;
                    jumpFrames = jumpFramesMax;
                    shootFrames = shootFramesMax;
                    meleeFrames = meleeFramesMax;
                    hitStun = hitStunMax;
                    break;
            }
        }

        // Optional arg to get direction
        public bool isMoving(string direction = "")
        {
            float v = Mathf.Abs(verticalInput);
            float h = Mathf.Abs(horizontalInput);
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

        public bool IsInHitstun()
        {
            return playerAction == "hit";
        }

        public bool IsFacingRight()
        {
            return playerRenderer.flipX == true;
        }

        public bool IsFacingLeft()
        {
            return playerRenderer.flipX == false;
        }
    }
}