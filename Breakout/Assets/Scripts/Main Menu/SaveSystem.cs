using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveSystem
{
    public static void SavePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bb";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData()
        {
            level = SceneControllerScript.Instance.GetLevel(),
            score = SceneControllerScript.Instance.GetOldScore(),
            lives = SceneControllerScript.Instance.GetLives()
        };

        //Debug.Log("Level: " + data.level + " Score: " + data.score + " Lives: " + data.lives);
        formatter.Serialize(stream, data);
        stream.Close();

        //Debug.Log("Data saved");
    }

    public static bool LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.bb";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            GameManagerScript.level = data.level;
            GameManagerScript.score = data.score;
            GameManagerScript.lives = data.lives;

            Debug.Log("Level: " + data.level + " Score: " + data.score + " Lives: " + data.lives);
            return true;
        }
        else
        {
            Debug.LogError("Save file not found");
            return false;
        }
    }

    public static void DeletePlayer()
    {
        string path = Application.persistentDataPath + "/player.bb";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveMute()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playermute.bb";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData()
        {
            mute = AudioManager.instance.mute
        };

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static bool LoadMute()
    {
        string path = Application.persistentDataPath + "/playermute.bb";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            AudioManager.instance.mute = data.mute;

            return true;
        }
        else
        {
            AudioManager.instance.mute = false;
            Debug.LogWarning("No mute prefs found");
            return false;
        }
    }

    public static void DeleteMute()
    {
        string path = Application.persistentDataPath + "/playermute.bb";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static void SaveHighScores()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highscores.bb";

        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log("Name: " + HighScore.highScoreList[HighScore.highScoreList.Count-1].name + " Score: " + HighScore.highScoreList[HighScore.highScoreList.Count - 1].finalScore);
        formatter.Serialize(stream, HighScore.highScoreList);
        stream.Close();

        Debug.Log("High Scores saved");
    }

    public static bool LoadHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.bb";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<PlayerScore> scoreList = formatter.Deserialize(stream) as List<PlayerScore>;

            stream.Close();

            HighScore.highScoreList = scoreList;

            Debug.Log("HighScores Loaded, " + scoreList.Count + " entries found.");
            return true;
        }
        else
        {
            Debug.Log("HighScores not found");
            return false;
        }
    }

    public static void DeleteHighScores()
    {
        string path = Application.persistentDataPath + "/highscores.bb";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }


    public static void SaveEndurance()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/endurance.bb";

        FileStream stream = new FileStream(path, FileMode.Create);

        EnduranceData data = new EnduranceData()
        {
            level = SceneControllerScript.Instance.GetLevel(),
            score = SceneControllerScript.Instance.GetScore(),
            lives = SceneControllerScript.Instance.GetLives(),
            //brickList = SceneControllerScript.Instance.GetComponent<BrickCreator>().brickList
        };

        Debug.Log("Level: " + data.level + " Score: " + data.score + " Lives: " + data.lives);
        formatter.Serialize(stream, data);
        stream.Close();


        Debug.Log("Endurance saved");
    }

    public static bool LoadEndurance()
    {
        string path = Application.persistentDataPath + "/endurance.bb";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnduranceData data = formatter.Deserialize(stream) as EnduranceData;

            stream.Close();

            GameManagerScript.level = data.level;
            GameManagerScript.score = data.score;
            GameManagerScript.lives = data.lives;


            //SceneControllerScript.Instance.GetComponent<BrickCreator>().brickList = data.brickList;

            Debug.Log("Level: " + data.level + " Score: " + data.score + " Lives: " + data.lives);
            return true;
        }
        else
        {
            Debug.LogError("Save file not found");
            return false;
        }
    }

    public static void DeleteEndurance()
    {
        string path = Application.persistentDataPath + "/endurance.bb";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Endurance file deleted");
        }
    }
}
