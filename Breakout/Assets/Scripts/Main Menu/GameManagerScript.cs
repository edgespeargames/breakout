using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum GameType { Campaign, Endurance, None }


public class GameManagerScript : MonoBehaviour
{
    public static GameType gameType = GameType.None;

    public static int level;
    public static int score;
    public static int lives;

    public GameObject FadeOutPrefab;

    private static GameManagerScript _instance;
    public static GameManagerScript Instance { get { return _instance; } }
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

    //PLACEHOLDER FOR TESTING
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            HighScore.ClearList();
            SaveSystem.DeleteHighScores();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveSystem.DeleteEndurance();
        }
    }

    

    public void SetCampaign()
    {
        gameType = GameType.Campaign;
        SaveSystem.DeletePlayer();
        level = 1;
        score = 0;
        lives = 3;
        Instance.StartCoroutine(FadeOut("Game Scene"));
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameType = GameType.Campaign;
        
        if (SaveSystem.LoadPlayer())
        {
            Instance.StartCoroutine(FadeOut("Game Scene"));
        }
        else
        {
            level = 1;
            score = 0;
            lives = 3;
            Instance.StartCoroutine(FadeOut("Game Scene"));
        }
    }

    public void SetEndurance()
    {
        Time.timeScale = 1f;
        gameType = GameType.Endurance;

        if (SaveSystem.LoadEndurance())
        {
            Instance.StartCoroutine(FadeOut("Game Scene"));
        }
        else
        {
            level = 1;
            score = 0;
            lives = 3;
            Instance.StartCoroutine(FadeOut("Game Scene"));
        }
        
    }

    //Give the user the option to retry Endurance mode or end the game
    public void LoseGame()
    {
        Time.timeScale = 0f;
        if(gameType == GameType.Endurance)
        {
            SaveSystem.DeleteEndurance();
        }
    }

    //When user loses at Endurance mode and decides not to continue
    public void EndGame()
    {
        Time.timeScale = 1f;
        gameType = GameType.None;
        Instance.StartCoroutine(FadeOut("Main Menu"));
    }

    public void ReturnToMenu()
    {
        if(gameType == GameType.Endurance)
        {
            SaveSystem.SaveEndurance();
        }
        gameType = GameType.None;
        Instance.StartCoroutine(FadeOut("Main Menu"));
    }

    IEnumerator FadeOut(string scene)
    {
        Instantiate(FadeOutPrefab);
        AudioManager.instance.Play("Close");
        AudioManager.instance.FadeOut("MenuMusic");
        AudioManager.instance.FadeOut("GameMusic");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
}
