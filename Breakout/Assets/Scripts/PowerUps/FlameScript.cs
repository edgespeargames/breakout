using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public float speed;

    private static FlameScript flamePU;

    private Rigidbody2D rigi;

    private float maxSpeed = 0.02f;

    //Make sure there is only ever one flame powerup on screen at a time
    private void Awake()
    {
        if (flamePU != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            flamePU = this;
        }

        rigi = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (rigi.velocity.magnitude > maxSpeed)
        {
            rigi.velocity = rigi.velocity.normalized * maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + -speed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Destroy"))
        {
            SceneControllerScript.Instance.RemovePowerUp(gameObject);
            Destroy(this.gameObject);
        }
    }
}
