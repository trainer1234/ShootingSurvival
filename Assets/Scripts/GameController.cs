using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class GameController : MonoBehaviour {
    public GameObject menuLose, menuWin, menuWinAll, menuGame;
    public Text txtPoint, txtLevel, txtTimeLeft;
    public int zombieToKill = 10, maxLevel = 10;
    public float timeLeft = 30;

    private GameObject player;

    private int currentPoint = 0;
    private int currentLevel;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Time.timeScale = 1;
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel == 1)
        {
            Scores.Init(maxLevel);
            Leaderboard.Record(Scores.currentUsername, 0);
        }

        txtPoint.text = "Zombie killed: " + currentPoint.ToString();
        txtLevel.text = "Level " + currentLevel.ToString() +
            "\nMission: Kill " + zombieToKill.ToString() +" Zombies";
        txtTimeLeft.text = string.Format("Time left: {0}s", (int)timeLeft);
        audioSource = gameObject.GetComponent<AudioSource>();
        menuLose.SetActive(false);
        menuWin.SetActive(false);
        menuWinAll.SetActive(false);
        menuGame.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKey(KeyCode.P))
        {
            if (menuLose.activeInHierarchy || menuWin.activeInHierarchy
                || menuWinAll.activeInHierarchy)
            {

            }
            else
            {
                Time.timeScale = 0;
                menuGame.SetActive(true);
            }
        }
	}

    public void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        //Debug.Log(timeLeft);
        txtTimeLeft.text = string.Format("Time left: {0}s", (int)timeLeft);
        if (timeLeft <= 0)
        {
            txtTimeLeft.text = "Time left: 0s";
            //audioSource.clip = audioClip;
            //audioSource.Play();
            player.GetComponent<PlayerController>().PlayDeadSound();

            //Scores.scores[currentLevel-1] = currentPoint;
            Leaderboard.UpdateScore(Scores.currentUsername, currentPoint);
            //scoresManager.ChangeScore(Scores.currentUsername, "kills", currentPoint);
            //Debug.Log(scoresManager.GetScore(Scores.currentUsername, "kills"));
            if (currentPoint >= zombieToKill)
            {
                if ((currentLevel + 1) > maxLevel)
                {
                    EndLastGame();
                }
                else
                {
                    EndGameWin();
                }
            }
            else
            {
                EndGameLose();
            }
        }
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


    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        menuGame.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GameHighScores()
    {
        //Scores.scores[currentLevel-1] = currentPoint;
        Leaderboard.UpdateScore(Scores.currentUsername, currentPoint);
        //scoresManager.ChangeScore(Scores.currentUsername, "kills", currentPoint);
        //Debug.Log(scoresManager.GetScore(Scores.currentUsername, "kills"));
        SceneManager.LoadScene("HighScores");
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void GetPoint(int point)
    {
        currentPoint++;
        txtPoint.text = "Zombie killed: " + currentPoint.ToString();
    }

    public void RestartGame()
    {
        currentLevel = 1;
        Scores.currentUsername = AutoChangeName(Scores.currentUsername);
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void NextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public void EndGameLose()
    {
        //Scores.scores[currentLevel-1] = currentPoint;
        Leaderboard.UpdateScore(Scores.currentUsername, currentPoint);
        menuLose.SetActive(true);
        Time.timeScale = 0; 
    }

    public void EndGameWin()
    {
        menuWin.SetActive(true);
        Time.timeScale = 0;
    }

    public void EndLastGame()
    {
        menuWinAll.SetActive(true);
        Time.timeScale = 0; 
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
