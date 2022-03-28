using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnv : MonoBehaviour
{
    public BoxCollider2D collider;

    // // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "env" | collision.gameObject.tag == "Player") {
            Physics2D.IgnoreCollision(collision.collider, collider);
        }
    }

}
