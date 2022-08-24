using System.Collections;
using System.Collections.Generic;
using Core.Controllers;
using UnityEngine;


namespace Core.Environment
{
    public class Interactable : MonoBehaviour
    {
        public Animator animator;
        public PlayerController playerController;
        public SpriteRenderer itemRenderer;
        public BoxCollider2D boxCollider;
        public string interactableName = "interactable";
        public bool canInteract = true;
        public bool canPickUp = false;

        public string toSay = "Hello World";
        // Start is called before the first frame update
        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
            itemRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            playerController = GameObject.FindGameObjectsWithTag("Player1")[0].GetComponent<PlayerController>();
        }
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}

