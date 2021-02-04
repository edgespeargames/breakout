using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public float speed;

    private static LifeScript lifePU;

    //Make sure there is only ever one flame powerup on screen at a time
    private void Awake()
    {
        if (lifePU != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            lifePU = this;
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
