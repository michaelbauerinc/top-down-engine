using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBasic : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public SpriteRenderer sprite;
    public int hitStun = 10;
    List<GameObject> collisions = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(hitStun);
        // Debug.Log(collisions);
        foreach (GameObject collision in collisions) {
            // Debug.Log(collision.gameObject);
            if (collision.gameObject.GetComponent<PlayerControls>().sliding) {
                hitStun = 0;
                //Fetch the first GameObject's position
                // var m_MyFirstVector = collision.gameObject.transform.position;
                // //Fetch the second GameObject's position
                // var m_MySecondVector = collision.gameObject.transform.position;
                // //Find the angle for the two Vectors
                // var m_Angle = Vector2.Angle(m_MyFirstVector, m_MySecondVector);
                // Debug.Log(m_Angle);
            }
        }
        // if (hitStun == 10) {
        //     hitStun = 0;
        // }
    }

    private void FixedUpdate()
    {  
        if (hitStun < 10) {
            animator.Play("hit_side");
            // body.velocity = new Vector2(2, 2);
            hitStun++;
            sprite.color = new UnityEngine.Color (0.97f, 0.02f, 0.02f, 1f);
        } else {
            animator.Play("idle_down");
            body.velocity = new Vector2(0, 0);
            sprite.color = new UnityEngine.Color (1f, 1f, 1f, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player") {
            collisions.Add(collision.gameObject);
        }
    }

    // void OnTriggerEnter2D(Collision2D collision) {
    //     if (collision.gameObject.tag == "env") {
    //         return;
    //         // collisions.Remove(collision.gameObject);
    //     }
    // }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collisions.Remove(collision.gameObject);
        }
        // Debug.Log(collision.gameObject.GetComponent<PlayerControls>().sliding);
    }
}
