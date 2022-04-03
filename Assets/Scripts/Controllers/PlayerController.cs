using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    UIController uiController;
    Interactable interactionTarget = null;
    Dictionary<string, string> animationMappings = new Dictionary<string, string>(){
        {"idle", "idle"},
        {"walking", "walk"},
        {"jumping", "jump"},
        {"shooting", "player_bow"},
        {"sliding", "slide"},
        {"interacting", "idle"}
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
        if (horizontal == 1f)
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
        if (jumpFrames < 30)
        {
            nextDirection = currentDirection;
        }
        currentDirection = nextDirection;
        animator.Play(prefix + "_" + currentDirection);
    }

    private void setPlayerAction()
    {
        if (canMove)
        {
            if (!isJumping())
            {
                if (Input.GetKeyDown("space"))
                {
                    playerAction = "jumping";
                }
                else if (!isSliding() && !isJumping() && Input.GetKeyDown("m"))
                {
                    playerAction = "sliding";
                }
                else if (isSliding() && slideFrames == 0)
                {
                    playerAction = "idle";
                }
                else if (!isInteracting() && Input.GetKeyDown("n") && interactionTarget)
                {
                    playerAction = "interacting";
                }
                else if (Input.GetKeyDown("1") && !isShooting() && uiController.weaponEquipped)
                {
                    playerAction = "shooting";
                }
                else if (isShooting())
                {
                    playerAction = shootFrames >= 0 ? "shooting" : "idle";

                }
                else if (isMoving() && !isSliding() && !isShooting())
                {
                    playerAction = "walking";

                }
                else if (!isMoving() && !isSliding() && !isShooting())
                {
                    playerAction = "idle";
                }

            }
            else if (isJumping() && jumpFrames == 0)
            {
                playerAction = "idle";
            }
        }
        else if (!canMove && isInteracting() && Input.GetKeyDown("n"))
        {
            playerAction = "idle";
        }
    }

    void ShootWeapon()
    {
        // var test = uiController.currentWeapon.ammo;
    }

    void Update()
    {
        // Inventory is not open and we are not interacting
        if (canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            switch (playerAction)
            {
                case "interacting":
                    bool isItem = interactionTarget.GetComponent<Item>() != null;
                    if (interactionTarget.canInteract)
                    {
                        uiController.ToggleInteractionBox(interactionTarget.toSay, interactionTarget.image);
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
                    slideFrames = 45;
                    break;
                case "sliding":
                    break;
                default:
                    break;
            }
        }
        else if (!canMove)
        {
            if (isInteracting() && Input.GetKeyDown("n"))
            {
                uiController.ToggleInteractionBox("");
                canMove = true;
            }
        }
        if (Input.GetKeyDown("return"))
        {
            uiController.inventoryOpen = !uiController.inventoryOpen;
            uiController.ToggleUi();
        }
        if (slideFrames >= 40)
        {
            Animate();
        }
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

        if (canMove)
        {
            setPlayerAction();
        }
        if (isMoving("diagonally"))
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        if (isJumping())
        {
            // Reduce speed by log 
            double modifier = 1 - System.Math.Log(jumpFrames, 35) * .95;
            float v = Mathf.Sign(vertical) * (float)modifier;
            float h = Mathf.Sign(horizontal) * (float)modifier;

            vertical -= Mathf.Abs(vertical) > 0 ? v : 0;
            horizontal -= Mathf.Abs(horizontal) > 0 ? h : 0;
        }
        if (slideFrames >= 44)
        {
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
        switch (playerAction)
        {
            case "shooting":
                shootFrames = playerAction == "shooting" ? shootFrames -= 1 : 35;
                break;
            case "jumping":
                slideFrames = 45;
                jumpFrames = jumpFrames > 0 ? jumpFrames -= 1 : 35;
                break;
            case "sliding":
                slideFrames = slideFrames > 0 ? slideFrames -= 1 : 45;
                break;
            default:
                slideFrames = 45;
                jumpFrames = 35;
                shootFrames = 35;
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
}
