using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private static DataManager instance; // Singleton para acceso global

    public static DataManager Instance { get { return instance; } }
    public int score = 0;
    public List<HighScore> highScores = new List<HighScore>();
    private int highScoreQuantity = 6;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadHighScores();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }


    public void InitializeHighScores()
    {
        highScores.Add(new HighScore("UNO", 1500));
        highScores.Add(new HighScore("SEI", 1000));
        highScores.Add(new HighScore("DOS", 1400));
        highScores.Add(new HighScore("CUA", 1200));
        highScores.Add(new HighScore("CIN", 1100));
        highScores.Add(new HighScore("TRE", 1300));

        highScores.Sort((a, b) => b.score.CompareTo(a.score));

        string json = JsonUtility.ToJson(new HighScoreList(highScores));
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save();
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        if (PlayerPrefs.HasKey("HighScores"))
        {
            string json = PlayerPrefs.GetString("HighScores");
            HighScoreList wrapper = JsonUtility.FromJson<HighScoreList>(json);
            highScores = wrapper.highScores;
            highScores.Sort((a, b) => b.score.CompareTo(a.score));
        }
        else
        {
            InitializeHighScores();
        }
    }
    public void PrintHighScores()
    {
        foreach (HighScore highScore in highScores)
        {
            Debug.Log($"Initials: {highScore.initials}, Score: {highScore.score}");
        }
    }
   
    public bool IsHighScore(int currentScore)
    {
        if (currentScore > highScores[highScores.Count - 1].score)
        {
            return true;
        }
        return false;
    }

    public void UpdateHighScores()
    {
        // Verifica si el puntaje actual califica para entrar en la lista
        if (IsHighScore(score))
        {
            highScores.Sort((a, b) => b.score.CompareTo(a.score));

            for (int i = (highScores.Count - 1); i >= 0; i--)
            {
                if (score > highScores[i].score)
                {
                    highScores[i] = new HighScore(highScores[i].initials, highScores[i].score);
                    highScores.Add(new HighScore("***", score));
                    highScores.Add(highScores[i]);

                    highScores.Sort((a, b) => b.score.CompareTo(a.score));

                    if (highScores.Count > highScoreQuantity) // Asume que quieres solo los 6 mejores puntajes
                    {
                        for (int j = 0; j < (highScores.Count - 5); j++)
                        {
                            highScores.RemoveAt(highScores.Count - 1); // Elimina el más bajo
                        }
                    }
                }
            }
            // Guarda los puntajes actualizados en PlayerPrefs
            string json = JsonUtility.ToJson(new HighScoreList(highScores));
            PlayerPrefs.SetString("HighScores", json);
            PlayerPrefs.Save();
        }
    }

  
    public void AddHighScore(HighScore newHighScore)
    {
        highScores.Add(newHighScore);

        highScores.Sort((a, b) => b.score.CompareTo(a.score));

        if (highScores.Count > highScoreQuantity)
        {
            highScores.RemoveAt(highScores.Count - 1); // Elimina el más bajo
        }

        // Guarda los puntajes actualizados en PlayerPrefs
        string json = JsonUtility.ToJson(new HighScoreList(highScores));
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save();
    }

}

[System.Serializable]
public class HighScoreList
{
    public List<HighScore> highScores;

    public HighScoreList(List<HighScore> scores)
    {
        highScores = scores;
    }
}

[System.Serializable]
public class HighScore
{
    public string initials;
    public int score;

    public HighScore(string initials, int score)
    {
        this.initials = initials;
        this.score = score;
    }
}
