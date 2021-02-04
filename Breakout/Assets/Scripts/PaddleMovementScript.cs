using UnityEngine;

public class PaddleMovementScript : MonoBehaviour
{
    private Vector3 mousePos;

    private Vector2 screenBounds;
    private float objectWidth;

    Rigidbody2D rb;

    Vector3 lastPosition = Vector3.zero;
    private float speed = 0;

    private bool allowInput = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    void FixedUpdate()
    {
        
        if (allowInput)
        {
            mousePos = Input.mousePosition; //Get mouse position
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); //Get mouse position in world co-ordinates

            Vector2 mousePosX = new Vector2(mousePos.x, transform.position.y); //X Position of the mouse

            //Clamp the paddle within the bounds of the screen//
            if (mousePos.x < (screenBounds.x * -1 + objectWidth))
            {
                mousePosX = new Vector2(screenBounds.x * -1 + objectWidth, transform.position.y);
            }
            if (mousePos.x > (screenBounds.x - objectWidth))
            {
                mousePosX = new Vector2(screenBounds.x - objectWidth, transform.position.y);
            }

            if ((transform.position.x - lastPosition.x) > 0)
            {
                speed = (transform.position - lastPosition).magnitude; // Speed is positive - moving right
            }
            else
            {
                speed = -(transform.position - lastPosition).magnitude; // Speed is negative - moving left
            }

            lastPosition = transform.position;

            if (Input.GetMouseButton(0))
            {
                MoveBody(rb, transform.position, mousePosX, 0.8f);
            }

            transform.rotation = Quaternion.Euler(0, 0, speed * 25);
        }
        
    }

    void MoveBody(Rigidbody2D body, Vector2 from, Vector2 to, float time)
    {
        body.MovePosition(Vector2.Lerp(from, to, time));
    }

    public void AllowInput(bool allow)
    {
        allowInput = allow;
    }
}

