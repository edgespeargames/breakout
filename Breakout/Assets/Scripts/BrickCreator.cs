using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCreator : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    private GameObject currentBrick;
    private float brickWidth;
    private float brickHeight;
    private int maxBricks;
    private bool inset = false;
    private Vector2 nextLoc; //next spawn location
    private int totalRows;
    public List<GameObject> bricks = new List<GameObject>();//List of possible bricks to instantiate
    public List<GameObject> brickList = new List<GameObject>();//List of all bricks created/currently on screen

    private bool bricksCreated = false;

    private int maxBrickColor;
    private int brickColor = 0;
    int numFirstColor; //level 7 = red (5)

    void Start()
    {
        Init();
    }

    public void Init()
    {
        brickWidth = brick.gameObject.GetComponent<SpriteRenderer>().bounds.size.x + 0.03f; //Get the width of a brick
        brickHeight = brick.gameObject.GetComponent<SpriteRenderer>().bounds.size.y + 0.025f; //Get the height of a brick

        nextLoc = new Vector2(XPosition(), 3f); //Starting location of the first brick

        ClearBricks();

        GetMaxBricks();

        if (inset)
        {
            nextLoc = new Vector2(XPosition() + (brickWidth / 2), nextLoc.y);
        }
        else
        {
            nextLoc = new Vector2(XPosition(), nextLoc.y);
        }

        if (GameManagerScript.gameType == GameType.Endurance)
        {
            StartCoroutine(PopulateBricksEndurance());
        }

        if (GameManagerScript.gameType == GameType.Campaign)
        {
            StartCoroutine(PopulateBricksCampaign());
        }

    }

    private float XPosition()
    {
        
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float indentation = (width - (CalculateMaxBricks() * brickWidth)) / 2;
        return (((width/2)-(brickWidth/2))-indentation)*-1;
    }

    private int CalculateMaxBricks()
    {
        int maximumBricks;
        brickWidth = brick.gameObject.GetComponent<SpriteRenderer>().bounds.size.x + 0.03f; //Get the width of a brick
        //Get the width of the camera//
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        float calculation = (width - 1f) / brickWidth; // Calculate how many bricks can fill the width of the screen as a float
        maximumBricks = (int)Mathf.Ceil(calculation); // Round the float up
        return maximumBricks;
    }

    // Random number 0 or 1, 0 == full row, 1 == inset row with 1 less brick//
    public int GetMaxBricks()
    {
        int randomNum = Random.Range(0, 2);
        if(randomNum < 1)
        {
            //maxBricks = 6;
            maxBricks = CalculateMaxBricks();
            inset = false;
        }
        else
        {
            //maxBricks = 5;
            maxBricks = CalculateMaxBricks() - 1;
            inset = true;
        }
        return maxBricks;
    }

    public void ClearBricks()
    {
        for(int i = 0; i < brickList.Count; i++)
        {
            Destroy(brickList[i].gameObject);
        }
        brickList.Clear();
        bricksCreated = false;
    }

    IEnumerator PopulateBricksCampaign()
    {
        int currentLevel = GameManagerScript.level;

        maxBrickColor = bricks.Count - 1;

        int numTakeaway;

        float remainder = (float)currentLevel / 6;

        if (remainder % 1 != 0)
        {
            numTakeaway = Mathf.CeilToInt(currentLevel / 6);
        }
        else
        {
            numTakeaway = Mathf.CeilToInt(currentLevel / 6) - 1;
        }

        int finalNum = (currentLevel - (6 * numTakeaway));

        if (currentLevel <= 6)
        {
            totalRows = currentLevel;
            numFirstColor = 6;
        }
        else
        {
            totalRows = 6;
            numFirstColor = totalRows - finalNum;
        }


        if (numTakeaway > 1)
        {
            brickColor = numTakeaway - 1;
        }
        else
        {
            brickColor = 0;
        }

        print("Brick Color " + brickColor);
        print("Max Brick Color " + maxBrickColor);

        
        

        for (int row = 0; row < totalRows; row++) //iterate through the total number of rows needed to display
        {
            
            if (row == numFirstColor)
            {
                brickColor++;
                //if(brickColor == maxBrickColor)
                //{
                //    brickColor = 0;
                //}
                //else
                //{
                //    brickColor++;
                //}
            }

            

            for (int i = 0; i < maxBricks; i++) //Brick by Brick
            {
                currentBrick = Instantiate(bricks[brickColor], nextLoc, Quaternion.identity);
                nextLoc = new Vector2(nextLoc.x + brickWidth, nextLoc.y);
                brickList.Add(currentBrick);
                yield return new WaitForSeconds(0.01f);
            }

            GetMaxBricks();

            // Determine next brick location
            if (inset)
            {
                nextLoc = new Vector2(XPosition() + (brickWidth / 2), nextLoc.y - brickHeight);
            }
            else
            {
                nextLoc = new Vector2(XPosition(), nextLoc.y - brickHeight);
            }
        }

        bricksCreated = true;
    }

    IEnumerator PopulateBricksEndurance()
    {
        totalRows = 6;
        for (int row = 0; row < totalRows; row++) //Row by Row
        {
            // Populate bricks
            for (int i = 0; i < maxBricks; i++) //Brick by Brick
            {
                int randBrick = Random.Range(0, bricks.Count);
                currentBrick = Instantiate(bricks[randBrick], nextLoc, Quaternion.identity);
                nextLoc = new Vector2(nextLoc.x + brickWidth, nextLoc.y);
                brickList.Add(currentBrick);
                yield return new WaitForSeconds(0.01f);
            }

            GetMaxBricks();

            // Determine next brick location
            if (inset)
            {
                nextLoc = new Vector2(XPosition() + (brickWidth / 2), nextLoc.y - brickHeight);
            }
            else
            {
                nextLoc = new Vector2(XPosition(), nextLoc.y - brickHeight);
            }
            
        }
        bricksCreated = true;
    }

    public bool BricksCreated()
    {
        return bricksCreated;
    }
}