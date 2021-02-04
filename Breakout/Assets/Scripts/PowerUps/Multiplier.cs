using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    public float speed;

    private static Multiplier multiPU;

    //Make sure there is only ever one multiplier powerup on screen at a time
    private void Awake()
    {
        if (multiPU != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            multiPU = this;
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
