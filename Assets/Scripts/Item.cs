using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public bool pickedUp = false;
    public bool isEquipped = false;

    // Start is called before the first frame update
    void Start()
    {
        canPickUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            Destroy(gameObject);
        }
    }
}
