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
    bool jumping = false;
    public bool sliding = false;
    bool interacting = false;
    public bool inventoryOpen = false;
    public bool shooting = false;
    Interactable interactionTarget = null;

    public string currentDirection = "down";

    public float runSpeed = 20.0f;
    int jumpTimer = 35;
    float slideTimer = 45f;
    int shootTimer = 50;



    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiController = GameObject.Find("UI").GetComponent<UIController>();
    }

    void Animate()
    {
        string prefix = "idle";
        string nextDirection = currentDirection;
        if (jumping)
        {
            prefix = "jump";
        }
        else if (shooting)
        {
            prefix = "player_bow";
        }
        else if (sliding)
        {
            prefix = "slide";
        }
        else if (!jumping && (Mathf.Abs(horizontal) > 0) | (!jumping && Mathf.Abs(vertical) > 0))
        {
            prefix = "walk";
        }
        if (horizontal == 1f)
        {

            if (!jumping)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            nextDirection = "side";
        }
        else if (horizontal == -1f)
        {
            if (!jumping)
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
        if (jumping)
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
            if (!jumping)
            {
                if (!interacting && Input.GetKeyDown("n"))
                {
                    if (interactionTarget)
                    {
                        bool isItem = interactionTarget.GetComponent<Item>() != null;
                        if (isItem)
                        {
                            uiController.AddItemToInventory(interactionTarget.GetComponent<Item>());
                        }
                        uiController.ToggleInteractionBox(interactionTarget.toSay, interactionTarget.image);
                        interacting = true;
                        canMove = false;
                        horizontal = 0;
                        vertical = 0;
                        animator.Play("idle_" + currentDirection);
                    }
                }
                else if (Input.GetKeyDown("1"))
                {
                    shooting = true;
                }
                else if (shooting)
                {
                    shooting = shootTimer > 0;
                    shootTimer = shooting == true ? shootTimer -= 1 : 50;
                }
                else if (Input.GetKeyDown("space"))
                {
                    jumping = true;
                    sliding = false;
                    slideTimer = 45;
                }
                else if (!sliding && Input.GetKeyDown("m"))
                {
                    sliding = true;
                }
                else if (sliding && slideTimer == 0)
                {
                    sliding = false;
                    slideTimer = 45;
                }
            }
            else if (jumping && jumpTimer == 0)
            {
                jumping = false;
                jumpTimer = 35;
            }
            Animate();
        }
        else if (!canMove && interacting && Input.GetKeyDown("n"))
        {
            uiController.ToggleInteractionBox("");
            interacting = false;
            canMove = true;
        }
        // else if (interacting)
        // {
        // }
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
        interacting = false;
    }
    private void FixedUpdate()
    {
        if (!sliding && Input.GetKeyDown("m"))
        {
            sliding = true;
        }
        else if (sliding)
        {
            if (slideTimer == 0)
            {
                sliding = false;
                slideTimer = 45;
            }
            else
            {
                slideTimer--;
            }
        }
        else if (jumping)
        {
            if (jumpTimer == 0)
            {
                jumping = false;
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
}
