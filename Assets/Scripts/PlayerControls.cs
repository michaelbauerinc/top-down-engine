using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;
    UIController uiController;

    public bool canMove = true;

    float moveLimiter = 0.7f;

    float horizontal;
    float vertical;
    Dictionary<string, string> animationMappings = new Dictionary<string, string>(){
        {"idle", "idle"},
        {"walking", "walk"},
        {"jumping", "jump"},
        {"shooting", "player_bow"},
        {"sliding", "slide"},
        {"interacting", "idle"}
    };


    string playerAction = "idle";
    public bool inventoryOpen = false;
    Interactable interactionTarget = null;

    public string currentDirection = "down";

    public float runSpeed = 20.0f;
    int jumpTimer = 35;
    float slideTimer = 45f;
    int shootTimer = 35;



    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiController = GameObject.Find("UI").GetComponent<UIController>();
    }

    void Animate()
    {
        Debug.Log(playerAction);
        string prefix = animationMappings[playerAction];
        string nextDirection = currentDirection;
        if (horizontal == 1f)
        {
            if (!isJumping())
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            nextDirection = "side";
        }
        else if (horizontal == -1f)
        {
            if (!isJumping())
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
        if (isJumping())
        {
            nextDirection = currentDirection;
        }
        currentDirection = nextDirection;
        animator.Play(prefix + "_" + currentDirection);
    }

    void Update()
    {
        // Inventory is not open and we are not interacting
        if (canMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            if (isMoving() && !isSliding() && !isJumping())
            {
                playerAction = "walking";
            }
            else if (!isMoving() && !isSliding() && !isJumping())
            {
                playerAction = "idle";
            }
            if (!isJumping())
            {
                if (!isInteracting() && Input.GetKeyDown("n"))
                {
                    if (interactionTarget)
                    {
                        bool isItem = interactionTarget.GetComponent<Item>() != null;
                        if (isItem)
                        {
                            uiController.AddItemToInventory(interactionTarget.GetComponent<Item>());
                        }
                        uiController.ToggleInteractionBox(interactionTarget.toSay, interactionTarget.image);
                        playerAction = "interacting";
                        canMove = false;
                        horizontal = 0;
                        vertical = 0;
                        animator.Play("idle_" + currentDirection);
                    }
                }
                else if (Input.GetKeyDown("1"))
                {
                    playerAction = "shooting";
                }
                else if (isShooting())
                {
                    playerAction = shootTimer > 0 ? "shooting" : playerAction;
                    shootTimer = playerAction == "shooting" ? shootTimer -= 1 : 35;
                }
                else if (Input.GetKeyDown("space"))
                {
                    playerAction = "jumping";
                    slideTimer = 45;
                }
                else if (!isSliding() && Input.GetKeyDown("m"))
                {
                    playerAction = "sliding";
                }
                else if (isSliding() && slideTimer == 0)
                {
                    playerAction = "idle";
                    slideTimer = 45;
                }
            }
            else if (isJumping() && jumpTimer == 0)
            {
                playerAction = "idle";
                jumpTimer = 35;
            }
            Animate();
        }
        else if (!canMove && isInteracting() && Input.GetKeyDown("n"))
        {
            uiController.ToggleInteractionBox("");
            playerAction = "idle";
            canMove = true;
        }
        if (Input.GetKeyDown("return"))
        {
            inventoryOpen = !inventoryOpen;
            uiController.ToggleUi();
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
        if (!isSliding() && Input.GetKeyDown("m"))
        {
            playerAction = "sliding";
        }
        else if (isSliding())
        {
            if (slideTimer == 0)
            {
                playerAction = "idle";
                slideTimer = 45;
            }
            else
            {
                slideTimer--;
            }
        }
        else if (isJumping())
        {
            if (jumpTimer == 0)
            {
                playerAction = "idle";
                jumpTimer = 35;
            }
            else
            {
                jumpTimer--;
            }
        }
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public bool isMoving()
    {
        return Mathf.Abs(horizontal) > 0 | Mathf.Abs(vertical) > 0;
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
