using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public bool pickedUp = false;
    public bool isEquipped = false;

    Animator animator;

    GameObject player;

    PlayerControls playerControls;

    Renderer itemRenderer;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        itemRenderer = GetComponent<Renderer>();
        canPickUp = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isEquipped == true)
        {
            string currentDirection = playerControls.currentDirection;
            Vector3 playerPos = player.transform.position;
            gameObject.transform.position = new Vector3(playerPos.x, playerPos.y, currentDirection != "up" ? playerPos.z - 1 : playerPos.z + 1);
            gameObject.GetComponent<SpriteRenderer>().flipX = player.GetComponent<SpriteRenderer>().flipX;
            if (playerControls.isShooting())
            {
                itemRenderer.enabled = true;
                animator.Play("weapon_bow_" + currentDirection);
            }
            else
            {
                animator.Play("weapon_bow_idle");
                itemRenderer.enabled = false;
            }
        }
        else if (pickedUp)
        {
            itemRenderer.enabled = false;
            player = GameObject.Find("Player");
            playerControls = GameObject.Find("Player").GetComponent<PlayerControls>();
        }
    }
}
