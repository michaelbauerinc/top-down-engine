using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Environment
{
    public class Interactable : MonoBehaviour
    {
        public string interactableName = "sign";
        public bool canInteract = true;
        public string toSay = "Hello";
        public bool canPickUp = false;
        public Sprite image;
        // Start is called before the first frame update
        public virtual void Awake()
        {
            image = gameObject.GetComponent<SpriteRenderer>().sprite;
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

