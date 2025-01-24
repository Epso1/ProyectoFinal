using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private float templateHeight = 40f;

    private List<HighScore> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {      
        DataManager.Instance.LoadHighScores();
        highscoreEntryList = DataManager.Instance.highScores;

        //Sort list by score
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    //Swap
                    HighScore tmp = highscoreEntryList[i];
                    highscoreEntryList[i]= highscoreEntryList[j];
                    highscoreEntryList[j]= tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighScore highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighScore highScoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;

            case 1:
                rankString = "1ST";
                break;

            case 2:
                rankString = "2ND";
                break;

            case 3:
                rankString = "3RD";
                break;

        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string initials = highScoreEntry.initials;
        entryTransform.Find("nameText").GetComponent<Text>().text = initials;

        transformList.Add(entryTransform);
    }
}
