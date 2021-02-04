using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHealth : MonoBehaviour
{
    [SerializeField] private Sprite damageSprite;

    private int health = 2;

    // Update is called once per frame
    void Update()
    {
        if (this.GetHealth() < 2)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = damageSprite;
        }
        if (this.GetHealth() < 1)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            this.SetHealth(1);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int damage)
    {
        health = health - damage;
    }
}
