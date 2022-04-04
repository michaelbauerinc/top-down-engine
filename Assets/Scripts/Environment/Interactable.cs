using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Controllers;


namespace Core.Environment
{
    public class Interactable : MonoBehaviour
    {
        public Animator animator;
        public PlayerController playerController;
        public SpriteRenderer itemRenderer;
        public string interactableName = "interactable";
        public bool canInteract = true;
        public bool canPickUp = false;

        public string toSay = "Hello World";
        // Start is called before the first frame update
        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
            itemRenderer = GetComponent<SpriteRenderer>();
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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

