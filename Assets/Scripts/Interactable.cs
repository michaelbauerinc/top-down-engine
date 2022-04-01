using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableName = "sign";
    public bool canInteract = true;
    public string toSay = "Hello";
    public bool canPickUp = false;
    public Sprite image;
    // Start is called before the first frame update
    void Awake()
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
