using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public GameObject highScoreCanvas;
    public GameObject campaignCanvas;
    public GameObject noCampaign;
    [SerializeField]private GameObject gameManager;

    public void ShowHighScores()
    {
        SaveSystem.LoadHighScores();
        gameManager.GetComponent<HighScore>().ShowHighScores();
        highScoreCanvas.SetActive(true);
    }

    public void CloseHighScores()
    {
        highScoreCanvas.SetActive(false);
    }

    public void ShowCampaignConfirm()
    {
        campaignCanvas.SetActive(true);
    }

    public void CloseCampaignConfirm()
    {
        campaignCanvas.SetActive(false);
    }

    public void ShowNoCampaign()
    {
        if (SaveSystem.LoadPlayer())
        {
            GameManagerScript.Instance.ResumeGame();
        }
        else
        {
            noCampaign.SetActive(true);
        }
    }

    public void CloseNoCampaign()
    {
        noCampaign.SetActive(false);
    }

    public void DeleteCampaign()
    {
        SaveSystem.DeletePlayer();
        Debug.Log("Campaign data deleted.");
    }

    public void DeleteHighScoreInfo()
    {
        HighScore.ClearList();
        SaveSystem.DeleteHighScores();
        Debug.Log("High score data deleted.");
    }
}
