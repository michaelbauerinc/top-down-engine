using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public bool pickedUp = false;
    public bool isEquipped = false;
    public string category;
    Animator animator;

    GameObject player;

    PlayerControls playerControls;

    SpriteRenderer itemRenderer;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        itemRenderer = GetComponent<SpriteRenderer>();
        canPickUp = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isEquipped == true)
        {
            string currentDirection = playerControls.currentDirection;
            Vector3 playerPos = player.transform.position;

            // Position bow vertically
            float v = currentDirection == "down" ? .2f : .5f;
            // if player is not facing the side, move item 0, if they are, move it .5f based on flipx
            float h = currentDirection == "side" ? player.GetComponent<SpriteRenderer>().flipX ? .5f : -.5f : 0;

            gameObject.transform.position = new Vector3(playerPos.x + h, playerPos.y + v, currentDirection != "up" ? playerPos.z - 1 : playerPos.z + 1);
            itemRenderer.flipX = player.GetComponent<SpriteRenderer>().flipX;
            if (playerControls.isShooting())
            {
                itemRenderer.enabled = true;
                animator.Play("weapon_bow_shoot_" + currentDirection);
            }
            else
            {
                itemRenderer.enabled = false;
            }
        }
        else if (pickedUp)
        {
            canPickUp = false;
            canInteract = false;
            gameObject.SetActive(false);
            itemRenderer.enabled = false;
            player = GameObject.Find("Player");
            playerControls = GameObject.Find("Player").GetComponent<PlayerControls>();
        }
    }
}
