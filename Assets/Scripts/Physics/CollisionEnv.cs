using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnv : MonoBehaviour
{
    public BoxCollider2D envCollider;

    // // Start is called before the first frame update
    void Start()
    {
        envCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "env" | collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, envCollider);
        }
    }

}
