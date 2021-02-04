using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Brick : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField]Sprite healthySprite;
    [SerializeField] Sprite damagedSprite;

    int health = 2;

    [SerializeField] GameObject nextBrick;

    [SerializeField] Color explosionColor;

    [SerializeField] private GameObject explosion;

    [SerializeField] private List<GameObject> powerUpList;

    [SerializeField] private int value;

    private static int multiplier = 1;

    [SerializeField] private GameObject popUpTextObj;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = healthySprite;
    }

    void Update()
    {
        if (health == 1)
        {
            spriteRenderer.sprite = damagedSprite;
        }
        if(health < 1)
        {
            Kill();
        }
    }

    void Kill()
    {
        Vector2 newPos = gameObject.transform.position;
        
        if (nextBrick)
        {
            GameObject newBrick = Instantiate(nextBrick, newPos, Quaternion.identity);
            SceneControllerScript.Instance.RemoveBrick(gameObject);
            SceneControllerScript.Instance.AddBrick(newBrick);
        }
        else
        {
            SceneControllerScript.Instance.RemoveBrick(gameObject);
        }

        int randNum = Random.Range(0, 10);
        if (randNum < powerUpList.Count)
        {
            GameObject powerUp = Instantiate(powerUpList[randNum], transform.position, Quaternion.identity); //Potentially drop a random powerup
            SceneControllerScript.Instance.AddPowerUp(powerUp);
        }

        GameObject expl = Instantiate(explosion, newPos, Quaternion.identity);
        expl.gameObject.GetComponent<ParticleScript>().SetColor(explosionColor);

        GameObject textPopUp = Instantiate(popUpTextObj, newPos, Quaternion.identity);
        textPopUp.gameObject.GetComponent<PopUpText>().SetText(GetValue().ToString());

        SceneControllerScript.Instance.IncrementScore(GetValue());

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        { 
            SetHealth(-1);
        }
    }

    //When fire power up is active
    void OnTriggerEnter2D(Collider2D col)
    {
        Vector2 newPos = gameObject.transform.position;
        GameObject expl = Instantiate(explosion, newPos, Quaternion.identity);
        expl.gameObject.GetComponent<ParticleScript>().SetColor(explosionColor);
        GameObject textPopUp = Instantiate(popUpTextObj, newPos, Quaternion.identity);
        textPopUp.gameObject.GetComponent<PopUpText>().SetText(GetValue().ToString());
        SceneControllerScript.Instance.IncrementScore(GetValue());
        SceneControllerScript.Instance.RemoveBrick(gameObject);

        health = 0;
    }

    public int GetHealth()
    {
        return health;
    }
    public void SetHealth(int damage)
    {
        health = health + damage;
    }
    public static void SetMultiplier(int mult)
    {
        multiplier = multiplier*mult;
    }
    public static void ResetMultiplier()
    {
        multiplier = 1;
    }
    public int GetValue()
    {
        return value * multiplier;
    }
    public void NoPowerUp()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = false;
    }
    public void FirePowerUp()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }
}
