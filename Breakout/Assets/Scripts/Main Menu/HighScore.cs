using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighScore : MonoBehaviour
{
    public static List<PlayerScore> highScoreList = new List<PlayerScore>();

    public GameObject leftPanel;
    public GameObject rightPanel;
    public Text textPrefab;

    public static void AddScore(string newName, int newScore)
    {
        PlayerScore newPlayerScore = new PlayerScore
        {
            name = newName,
            finalScore = newScore
        };
        highScoreList.Add(newPlayerScore);
    }

    public static void ClearList()
    {
        highScoreList.Clear();
    }

    public static List<PlayerScore> GetList()
    {
        return highScoreList;
    }
    private void ClearObjects(GameObject panel)
    {
        foreach (Transform text in panel.transform)
        {
            Destroy(text.gameObject);
        }
    }

    public void ClearPanels()
    {
        ClearObjects(leftPanel);
        ClearObjects(rightPanel);
    }

    public void ShowHighScores()
    {
        ClearPanels();

        highScoreList.Sort();
        highScoreList.Reverse();
        for(int i = 0; i < 9; i++)
        {
            Text tempText = Instantiate(textPrefab);
            tempText.transform.SetParent(leftPanel.transform);
            tempText.transform.localScale = new Vector3(1, 1, 1);
            if (i < highScoreList.Count)
                tempText.text = highScoreList[i].name;
            else
                tempText.text = "AAA";

            Text tempScoreText = Instantiate(textPrefab);
            tempScoreText.transform.SetParent(rightPanel.transform);
            tempScoreText.transform.localScale = new Vector3(1, 1, 1);
            if (i < highScoreList.Count)
                tempScoreText.text = highScoreList[i].finalScore.ToString();
            else
                tempScoreText.text = "0";
        }
    }

}
