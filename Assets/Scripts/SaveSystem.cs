using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    const string SAVE_KEY = "CardMatch_Save";

    public void SaveGame(BoardState boardState, int score, int combo)
    {
        SaveData d = new SaveData();
        d.boardState = boardState;
        d.score = score;
        d.combo = combo;
        string json = JsonUtility.ToJson(d);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        Debug.Log("Game saved.");
    }

    public SaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        try
        {
            SaveData d = JsonUtility.FromJson<SaveData>(json);
            return d;
        }
        catch
        {
            Debug.Log("Failed to load save.");
            return null;
        }
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class SaveData
{
    public BoardState boardState;
    public int score;
    public int combo;
}