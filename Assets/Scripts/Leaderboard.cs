﻿using UnityEngine;

using System.Collections.Generic;

public static class Leaderboard
{
    public const int EntryCount = 10;
    public static Dictionary<string, int> changeExistNameCounter;

    public struct ScoreEntry
    {
        public string name;
        public int score;

        public ScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    private static List<ScoreEntry> s_Entries;

    private static List<ScoreEntry> Entries
    {
        get
        {
            if (s_Entries == null)
            {
                s_Entries = new List<ScoreEntry>();
                changeExistNameCounter = new Dictionary<string, int>();
                LoadScores();
            }
            return s_Entries;
        }
    }

    private const string PlayerPrefsBaseKey = "leaderboard";

    private static void SortScores()
    {
        s_Entries.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private static void LoadScores()
    {
        s_Entries.Clear();

        for (int i = 0; i < EntryCount; ++i)
        {
            ScoreEntry entry;
            entry.name = PlayerPrefs.GetString(PlayerPrefsBaseKey + "[" + i + "].name", "");
            entry.score = PlayerPrefs.GetInt(PlayerPrefsBaseKey + "[" + i + "].score", 0);
            changeExistNameCounter[entry.name] = entry.score;
            s_Entries.Add(entry);
        }

        SortScores();
    }

    private static void SaveScores()
    {
        for (int i = 0; i < EntryCount; ++i)
        {
            var entry = s_Entries[i];
            PlayerPrefs.SetString(PlayerPrefsBaseKey + "[" + i + "].name", entry.name);
            PlayerPrefs.SetInt(PlayerPrefsBaseKey + "[" + i + "].score", entry.score);
        }
    }

    public static void ResetScores()
    {
        changeExistNameCounter.Clear();
        s_Entries.Clear();
        PlayerPrefs.DeleteAll();
        LoadScores();
    }

    public static ScoreEntry GetEntry(int index)
    {
        return Entries[index];
    }

    public static void UpdateScore(string name, int amount)
    {
        for (int i = 0; i < EntryCount; i++)
        {
            if (name == Entries[i].name)
            {
                int oldScore = Entries[i].score;
                Entries.Remove(Entries[i]);
                Entries.Add(new ScoreEntry(name, oldScore + amount));
                break;
            }
        }
        SortScores();
        SaveScores();
    }

    public static void Record(string name, int score)
    {
        Entries.Add(new ScoreEntry(name, score));
        SortScores();
        Entries.RemoveAt(Entries.Count - 1);
        SaveScores();
    }
}