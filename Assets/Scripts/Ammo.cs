using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    Rigidbody2D body;
    public int speed = 10;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Shoot(string direction, bool flipped)
    {
        int x = 0;
        int y = 0;
        switch (direction)
        {
            case "down":
                y = -1;
                gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case "up":
                y = 1;
                gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 10);

                break;
            default:
                gameObject.GetComponent<SpriteRenderer>().flipX = flipped;
                x = flipped ? 1 : -1;
                break;
        }
        body.velocity = new Vector2(x * speed, y * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hasTags = other.gameObject.GetComponent<MultiTag>() != null;
        if (hasTags && other.gameObject.GetComponent<MultiTag>().HasTag("enemy"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
