using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text;

public class PlayerScoreList : MonoBehaviour
{

    public GameObject playerScoreEntryPrefab;

    //ScoresManager scoresManager;

    //int lastChangeCounter;

    // Use this for initialization
    void Start()
    {
        GameObject btnReturn = GameObject.Find("ButtonReturn");
        if (Scores.currentUsername == null)
        {
            btnReturn.GetComponent<Button>().interactable = false;
        }
        else
        {
            btnReturn.GetComponent<Button>().interactable = true;
        }
        //scoresManager = GameObject.FindObjectOfType<ScoresManager>();
        
        //lastChangeCounter = scoresManager.GetChangeCounter();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (scoresManager == null)
        {
            Debug.LogError("You forgot to add the score manager component to a game object!");
            return;
        }

        if (scoresManager.GetChangeCounter() == lastChangeCounter)
        {
            // No change since last update!
            return;
        }

        lastChangeCounter = scoresManager.GetChangeCounter();*/

        while (this.transform.childCount > 0)
        {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            //Destroy(c.gameObject);
        }

        // Display high scores!
        for (int i = 0; i < Leaderboard.EntryCount; ++i)
        {
            var entry = Leaderboard.GetEntry(i);
            //Debug.Log(entry.name + " " + entry.score.ToString());
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.Find("Username").GetComponent<Text>().text = entry.name;
            go.transform.Find("Kill").GetComponent<Text>().text = entry.score.ToString();
        }

        /*string[] names = scoresManager.GetPlayerNames("kills");

        foreach (string name in names)
        {
            GameObject go = (GameObject)Instantiate(playerScoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.Find("Username").GetComponent<Text>().text = name;
            go.transform.Find("Kills").GetComponent<Text>().text = scoresManager.GetScore(name, "kills").ToString();
        }*/
    }

    public string AutoChangeName(string name)
    {
        StringBuilder username = new StringBuilder(name);
        for (int i = 0; i < Leaderboard.EntryCount; ++i)
        {
            var entry = Leaderboard.GetEntry(i);
            if (username.ToString() == entry.name)
            {
                //string tmp = username.ToString() + Leaderboard.changeExistNameCounter[entry.name];
                //while(Leaderboard.changeExistNameCounter[)
                if (Leaderboard.changeExistNameCounter.ContainsKey(entry.name))
                {
                    Leaderboard.changeExistNameCounter[entry.name]++;
                }
                else
                {
                    Leaderboard.changeExistNameCounter.Add(entry.name, 0);
                }
                //Debug.Log(entry.name + " " + Leaderboard.changeExistNameCounter[entry.name]);
                username.Append(Leaderboard.changeExistNameCounter[entry.name].ToString());
                break;
            }
        }
        return username.ToString();
    }


    public void Reset()
    {
        Leaderboard.ResetScores();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void ReturnToGame()
    {
        if (Scores.currentUsername != null)
        {
            Scores.currentUsername = AutoChangeName(Scores.currentUsername);
            SceneManager.LoadScene(1);
        }
        
    }

    public void ReturnToMenu()
    {
        Scores.currentUsername = null;
        SceneManager.LoadScene("Menu");
    }
}
