 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("LeaderboardEntryContainer");
        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        AddLeaderboardEntry(1000, "AAA");

        string jsonString = PlayerPrefs.GetString("leaderboard");
        Leaderboards leaderboards = JsonUtility.FromJson<Leaderboards>(jsonString);

        //sort list by score
        for(int i=0; i<leaderboards.leaderboardEntryList.Count; i++){
            for(int j=i+1; j<leaderboards.leaderboardEntryList.Count; j++){
                if(leaderboards.leaderboardEntryList[i].score < leaderboards.leaderboardEntryList[j].score){
                    //swap
                    LeaderboardEntry temp = leaderboards.leaderboardEntryList[i];
                    leaderboards.leaderboardEntryList[i] = leaderboards.leaderboardEntryList[j];
                    leaderboards.leaderboardEntryList[j] = temp;
                }
            }
        }

        leaderboardEntryTransformList = new List<Transform>();
        foreach(LeaderboardEntry leaderboardEntry in leaderboards.leaderboardEntryList){
            CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);
        }
    }

    private void CreateLeaderboardEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 0.5f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -1-templateHeight * transformList.Count);
        entryRectTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        int points = leaderboardEntry.score;
        string user = leaderboardEntry.name;
        entryTransform.Find("Ranking").GetComponent<Text>().text = rank.ToString();
        entryTransform.Find("User").GetComponent<Text>().text = user;
        entryTransform.Find("Points").GetComponent<Text>().text = points.ToString();

        transformList.Add(entryTransform);
    }

    private void AddLeaderboardEntry(int score, string name){
        //create leaderboard entry
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry {score = score, name = name};
        
        //load saved leaderboard
        string jsonString = PlayerPrefs.GetString("leaderboard");
        Leaderboards leaderboards = JsonUtility.FromJson<Leaderboards>(jsonString);
        
        //add new entry
        leaderboards.leaderboardEntryList.Add(leaderboardEntry);

        //save updated leaderboard
        string json = JsonUtility.ToJson(leaderboards);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
    }

    private class Leaderboards
    {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    /*
    *   Represents a single leaderboard entry
    */
    [System.Serializable]
    private class LeaderboardEntry
    {
        public int score;
        public string name; 
    }
}
