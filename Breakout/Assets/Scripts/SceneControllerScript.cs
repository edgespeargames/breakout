using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public enum PowerUpEnum { None, Fire, Multiply }
public class SceneControllerScript : MonoBehaviour
{
    private static SceneControllerScript _instance;
    public static SceneControllerScript Instance { get { return _instance; } }
    [SerializeField] private GameObject ball;
    private GameObject ballObj;
    [SerializeField] private GameObject paddle;
    private GameObject paddleObj;
    public bool countdown = false;
    public bool multCountdown = false;

    [SerializeField] private GameObject explosion;

    [SerializeField] private List<GameObject> powerUpList;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;

    private Color normal;
    private Color faded;

    private bool begin = false;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Text gameOverLevel;
    [SerializeField] private Text gameOverScore;
    [SerializeField] private GameObject campaignCanvas;
    [SerializeField] private Text campaignLevel;
    [SerializeField] private Text campaignScore;

    private int oldScore = 0;

    public Text inputText;

    private void Awake()
    {
        if (_instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        normal = new Color(255, 255, 255, 1);
        faded = new Color(0, 0, 0, 0.7f);

        paddleObj = Instantiate(paddle, new Vector2(0, -3), Quaternion.identity);
        AllowInput(false);
        Physics2D.IgnoreLayerCollision(8, 9, true); //ignore collision between ball and powerups
        Physics2D.IgnoreLayerCollision(11, 9, true); //ignore collision between bricks and powerups
    }

    //Called by StartScript when the start area has been clicked
    public void Begin()
    {
        begin = true;
        AllowInput(true);
    }

    void Initialize(int numLives)
    {
        Brick.ResetMultiplier();
        UpdateLives();
        levelText.text = "Level: " + GameManagerScript.level.ToString();
        DestroyPowerUps();
        if (GetComponent<BrickCreator>().BricksCreated())
        {
            paddleObj.GetComponent<PaddleControllerScript>().ResetPosition();
            ballObj = Instantiate(ball, Vector3.zero, Quaternion.identity);

            EndPowerUp();
            paddleObj.gameObject.GetComponent<PaddleControllerScript>().LivesTrigger(numLives);
        }
        AllowInput(false);
    }

    public void IncreaseLevel()
    {
        if (GetComponent<BrickCreator>().BricksCreated())
        {
            DestroyPowerUps();
            Brick.ResetMultiplier();
            DestroyAll("Brick");
            Destroy(ballObj);
            paddleObj.GetComponent<PaddleControllerScript>().ResetPosition();
            SetLevel(GameManagerScript.level + 1);
            GetComponent<BrickCreator>().Init();
            levelText.text = "Level: " + GameManagerScript.level.ToString();
            oldScore = GameManagerScript.score;
            if (GameManagerScript.gameType == GameType.Campaign)
            {
                SaveSystem.SavePlayer();
            }
        }

    }

    public void DecreaseLevel()
    {
        if (GetComponent<BrickCreator>().BricksCreated())
        {
            if (GameManagerScript.level > 1)
            {
                DestroyPowerUps();
                Brick.ResetMultiplier();
                DestroyAll("Brick");
                Destroy(ballObj);
                paddleObj.GetComponent<PaddleControllerScript>().ResetPosition();
                SetLevel(GameManagerScript.level - 1);
                GetComponent<BrickCreator>().Init();
                levelText.text = "Level: " + GameManagerScript.level.ToString();
                oldScore = GameManagerScript.score;
                if (GameManagerScript.gameType == GameType.Campaign)
                {
                    SaveSystem.SavePlayer();
                }
            }
        }
    }

    void Update()
    {
        if (begin && ballObj)
        {
            ballObj.gameObject.GetComponent<BallMovementScript>().BeginMove();
        }
        //If there are no balls in the scene, instantiate one
        if (GameObject.FindGameObjectsWithTag("Ball").Length < 1)
        {
            begin = false;
            Initialize(GameManagerScript.lives);
        }
        if (GetComponent<BrickCreator>().BricksCreated())
        {
            // If there are no more bricks, increment the level
            if (GetComponent<BrickCreator>().brickList.Count < 1)
            {
                DestroyPowerUps();
                DestroyAll("Brick");
                Destroy(ballObj);
                paddleObj.GetComponent<PaddleControllerScript>().ResetPosition();
                SetLevel(GameManagerScript.level + 1);
                GetComponent<BrickCreator>().Init();
                levelText.text = "Level: " + GameManagerScript.level.ToString();
                oldScore = GameManagerScript.score;
                if (GameManagerScript.gameType == GameType.Campaign)
                {
                    SaveSystem.SavePlayer();
                }
            }
            //// DEBUGGING LEVEL UPPER
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                IncreaseLevel();
            }
            // DEBUGGING LEVEL DOWNER
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                DecreaseLevel();
            }
        }


        if (countdown)
        {
            StartCoroutine(Countdown());
        }

        if (multCountdown)
        {
            StartCoroutine(MultiplierCountdown());
        }

        scoreText.text = GetScore().ToString();
    }


    public void AllowInput(bool input)
    {
        if (ballObj)
        {
            ballObj.gameObject.GetComponent<BallMovementScript>().enabled = input;
        }

        paddleObj.gameObject.GetComponent<PaddleMovementScript>().AllowInput(input);
    }
    public void SetLevel(int num)
    {
        GameManagerScript.level = num;
    }
    public int GetLevel()
    {
        return GameManagerScript.level;
    }

    public int GetOldScore()
    {
        return oldScore;
    }
    public void BeginPowerUp(PowerUpEnum powerUpType)
    {
        if (powerUpType == PowerUpEnum.Fire)
        {
            //Activate paddle animation - not sure if it's better doing this directly in the paddle controller script
            paddleObj.GetComponent<PaddleControllerScript>().FirePowerUp();
            //change the ball to be on fire
            ballObj.GetComponent<BallMovementScript>().FirePowerUp();
            //change the bricks to have trigger colliders
            List<GameObject> TempBrickList = GetComponent<BrickCreator>().brickList;
            for (int i = 0; i < TempBrickList.Count; i++)
            {
                TempBrickList[i].gameObject.GetComponent<Brick>().FirePowerUp();
            }
            countdown = true;
        }
    }
    public void EndPowerUp()
    {
        paddleObj.GetComponent<PaddleControllerScript>().NoPowerUp();
        if (ballObj)
        {
            ballObj.GetComponent<BallMovementScript>().NoPowerUp();
        }
        Brick.ResetMultiplier();
        //Turn off fire
        //change the bricks to have solid colliders
        List<GameObject> TempBrickList = GetComponent<BrickCreator>().brickList;
        for (int i = 0; i < TempBrickList.Count; i++)
        {
            TempBrickList[i].gameObject.GetComponent<Brick>().NoPowerUp();
        }
        countdown = false;
        multCountdown = false;
    }
    public int GetScore()
    {
        return GameManagerScript.score;
    }

    public int GetLives()
    {
        return GameManagerScript.lives;
    }
    public int IncrementScore(int value)
    {
        GameManagerScript.score = GameManagerScript.score + value;
        return GameManagerScript.score;
    }
    public void SetMultiplier()
    {
        multCountdown = true;
    }

    public void LoseLife()
    {
        GameManagerScript.lives -= 1;

        UpdateLives();

        if (GameManagerScript.gameType == GameType.Campaign)
        {
            if (GameManagerScript.lives == 2 || GameManagerScript.lives == 1)
            {
                SaveSystem.SavePlayer();
            }
        }


        if (GameManagerScript.lives < 1)
        {

            AllowInput(false);
            if (GameManagerScript.gameType == GameType.Campaign)
            {
                campaignCanvas.SetActive(true);
                campaignLevel.text = "Level: " + GameManagerScript.level;
                campaignScore.text = "Score: " + GameManagerScript.score;
                GameManagerScript.Instance.LoseGame();
            }

            if (GameManagerScript.gameType == GameType.Endurance)
            {
                SaveSystem.DeleteEndurance();
                gameOverCanvas.SetActive(true);
                gameOverLevel.text = "Level: " + GameManagerScript.level;
                gameOverScore.text = "Score: " + GameManagerScript.score;
                
                GameManagerScript.Instance.LoseGame();
            }
        }
    }

    public void NewHighScore()
    {
        if(inputText.text != "")
        {
            HighScore.AddScore(inputText.text, GameManagerScript.score);
        }
        else
        {
            HighScore.AddScore("AAA", GameManagerScript.score);
        }
        
        SaveSystem.SaveHighScores();
    }

    public void UpdateLives()
    {
        if (GameManagerScript.lives == 1)
        {
            life1.gameObject.GetComponent<Image>().color = normal;
            life2.gameObject.GetComponent<Image>().color = faded;
            life3.gameObject.GetComponent<Image>().color = faded;
        }

        if (GameManagerScript.lives == 2)
        {
            life1.gameObject.GetComponent<Image>().color = normal;
            life2.gameObject.GetComponent<Image>().color = normal;
            life3.gameObject.GetComponent<Image>().color = faded;
        }

        if (GameManagerScript.lives == 3)
        {
            life1.gameObject.GetComponent<Image>().color = normal;
            life2.gameObject.GetComponent<Image>().color = normal;
            life3.gameObject.GetComponent<Image>().color = normal;
        }
    }

    public void AddLife()
    {
        if(GameManagerScript.lives < 3)
        {
            GameManagerScript.lives ++;
            UpdateLives();
        }
        SaveSystem.SavePlayer();
    }
    public void RemoveBrick(GameObject brick)
    {
        GetComponent<BrickCreator>().brickList.Remove(brick);
        Destroy(brick.gameObject);
    }
    public void AddBrick(GameObject brick)
    {
        GetComponent<BrickCreator>().brickList.Add(brick);
    }
    void DestroyAll(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < objects.Length; i++)
        {
            Instantiate(explosion, objects[i].gameObject.transform);
            Destroy(objects[i]);
        }
    }

    IEnumerator Countdown()
    {
        float duration = 5f; 
        float totalTime = 0;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;

            yield return null;
        }
        
        //change the bricks to have solid colliders
        List<GameObject> TempBrickList = GetComponent<BrickCreator>().brickList;
        for (int i = 0; i < TempBrickList.Count; i++)
        {
            TempBrickList[i].gameObject.GetComponent<Brick>().NoPowerUp();
        }

        if (ballObj)
        {
            ballObj.GetComponent<BallMovementScript>().NoPowerUp();
        }
        paddleObj.GetComponent<PaddleControllerScript>().NoPowerUp();
        countdown = false;
    }
    
    IEnumerator MultiplierCountdown()
    {
        float duration = 7f; 
        float totalTime = 0;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;

            yield return null;
        }
        Brick.ResetMultiplier();
        multCountdown = false;
    }

    public void AddPowerUp(GameObject powerUp)
    {
        powerUpList.Add(powerUp);
    }

    public void RemovePowerUp(GameObject powerUp)
    {
        powerUpList.Remove(powerUp);
    }

    void DestroyPowerUps()
    {
        for(int i = 0; i < powerUpList.Count; i++)
        {
            Destroy(powerUpList[i]);
        }
        powerUpList.Clear();
    }
}
