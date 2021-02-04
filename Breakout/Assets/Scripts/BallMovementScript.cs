using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallMovementScript : MonoBehaviour, IPowerUp
{
    private Rigidbody2D rigi;
    private bool started = false;
    private int randNum;
    private Vector3 dir;
    [SerializeField] private float force;
    [SerializeField] private float randomFactor = 1f;
    TrailRenderer myTrailRenderer;
    Color originalColor;
    Color startColor;
    Color endColor;
    private bool beginMove = false;

    public float maxSpeed = 10f;//Maximum speed

    // Start is called before the first frame update
    void Awake()
    {
        rigi = gameObject.GetComponent<Rigidbody2D>();
        randNum = Random.Range(0, 2);
        originalColor = GetComponent<SpriteRenderer>().color;

        myTrailRenderer = GetComponent<TrailRenderer>();
        startColor = myTrailRenderer.startColor;
        endColor = myTrailRenderer.endColor;
    }

    private void Start()
    {
        beginMove = false;
    }

    private void FixedUpdate()
    {
        if (rigi.velocity.magnitude > maxSpeed)
        {
            rigi.velocity = rigi.velocity.normalized * maxSpeed;
        }
        if(rigi.velocity.magnitude < 5f)
        {
            rigi.velocity = rigi.velocity.normalized * 5f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (beginMove && !started)
        {
            SetDir(randNum);
            rigi.velocity = Vector3.zero;
            rigi.AddForce(dir * force);
            started = true;
        }
    }

    public void BeginMove()
    {
        beginMove = true;
    }

    public Vector3 SetDir(int num)
    {
        if(num == 0)
        {
            dir = new Vector3(Random.Range(-10, -30), -30, 0); // Move diagonally left
        }
        else
        {
            dir = new Vector3(Random.Range(10, 30), -30, 0); // Move diagonally right
        }
        return dir;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Destroy"))
        {
            NoPowerUp();
            SceneControllerScript.Instance.EndPowerUp();
            SceneControllerScript.Instance.LoseLife();
            Destroy(gameObject);
        }
    }
    //ball object collision - Taken from Roger @ https://gamedev.stackexchange.com/questions/125417/infinite-ball-bouncing-problem-unity/125440
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RandomVelocityTweak();
    }
    private void RandomVelocityTweak()
    {
        //create random velocity tweak from 0 - 0.3f
        Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
        //add velocity tweak to velocity
        rigi.velocity += velocityTweak;
    }

    public void NoPowerUp()
    {
        originalColor = GetComponent<SpriteRenderer>().color = originalColor;
        myTrailRenderer.startColor = startColor;
        myTrailRenderer.endColor = endColor;
    }

    public void FirePowerUp()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f, 0.5f); //Colour the ball red
        //Make the trail red to yellow gradient
        myTrailRenderer.startColor = Color.yellow;
        myTrailRenderer.endColor = new Color(1f, 0.2f, 0.2f, 0.1f);
    }

    public void MultiplierPowerUp(float multiplier)
    {
    }
}
