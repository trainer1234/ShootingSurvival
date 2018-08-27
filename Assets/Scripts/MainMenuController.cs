using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

public class MainMenuController : MonoBehaviour {
    public GameObject menuPlay;
    public Text txtUsername;

	// Use this for initialization
	void Start () {
        menuPlay.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameHighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    private void SubmitName(string username)
    {
        //Debug.Log(username);
        Scores.currentUsername = username;
    }

    public void OpenPlayPanel()
    {
        menuPlay.SetActive(true);
    }

    public void ClosePlayPanel()
    {
        menuPlay.SetActive(false);
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
                if(Leaderboard.changeExistNameCounter.ContainsKey(entry.name)){
                    Leaderboard.changeExistNameCounter[entry.name]++;
                }
                else{
                    Leaderboard.changeExistNameCounter.Add(entry.name, 0);
                }
                //Debug.Log(entry.name + " " + Leaderboard.changeExistNameCounter[entry.name]);
                username.Append(Leaderboard.changeExistNameCounter[entry.name].ToString());
                break;
            }
        }
        return username.ToString();
    }

    public void PlayGame()
    {
        string name = AutoChangeName(txtUsername.text.ToString());
        SubmitName(name);
        menuPlay.SetActive(false);
        SceneManager.LoadScene(1);
    }
}
