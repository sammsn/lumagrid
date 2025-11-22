using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BoardController boardController;
    public UIManager uiManager;
    public SoundManager soundManager;
    public SaveSystem saveSystem;

    Queue<Card> flipQueue = new Queue<Card>();

    public int score { get; private set; }
    private int basePoints = 100;
    int comboCount = 0;
    float lastMatchTime = -10f;
    public float comboWindow = 2f;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    void Start()
    {
        StartCoroutine(ComparisonLoop());
    }

    public void StartGame(int i)
    {
        score = 0;
        comboCount = 0;
        lastMatchTime = -10f;
        uiManager.UpdateScore(score, comboCount);

        switch (i)
        {
            case 0:
                boardController.SetupBoard(2, 2);
                break;
            case 1:
                boardController.SetupBoard(2, 3);
                break;
            case 2:
                boardController.SetupBoard(3, 6);
                break;
            case 3:
                boardController.SetupBoard(5, 6);
                break;
        }
    }

    public void LoadSavedIfAny()
    {
        var data = saveSystem.LoadGame();
        if (data != null)
        {
            score = data.score;
            comboCount = data.combo;
            boardController.DeserializeState(data.boardState);
            uiManager.UpdateScore(score, comboCount);
            uiManager.LoadGameIfAny();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RegisterFlip(Card card)
    {
        soundManager.PlayFlip();
        lock (flipQueue)
        {
            flipQueue.Enqueue(card);
        }
    }

    IEnumerator ComparisonLoop()
    {
        while (true)
        {
            Card a = null, b = null;
            lock (flipQueue)
            {
                if (flipQueue.Count >= 2)
                {
                    a = flipQueue.Dequeue();
                    b = flipQueue.Dequeue();

                    if (a == b || a.IsMatched || b.IsMatched)
                    {
                        if (a != null && !a.IsMatched && a != b) flipQueue.Enqueue(a);
                        if (b != null && !b.IsMatched && b != a) flipQueue.Enqueue(b);
                        a = b = null;
                    }
                }
            }

            if (a != null && b != null)
                yield return StartCoroutine(ComparePair(a, b));
            else
                yield return null;
        }
    }

    IEnumerator ComparePair(Card a, Card b)
    {
        yield return new WaitForSeconds(0.15f);

        if (a.id == b.id)
        {
            a.SetMatched();
            b.SetMatched();
            ApplyMatchScore();
            soundManager.PlayMatch();

            if (boardController.AllMatched())
            {
                soundManager.PlayGameOver();
                uiManager.ShowGameOver(score);
                saveSystem.ClearSave();
            }
            else
                saveSystem.SaveGame(boardController.SerializeState(), score, comboCount);
        }
        else
        {
            soundManager.PlayMismatch();
            yield return new WaitForSeconds(0.45f);
            a.StartCoroutine(a.Flip(false));
            b.StartCoroutine(b.Flip(false));

            saveSystem.SaveGame(boardController.SerializeState(), score, comboCount);
        }
    }

    void ApplyMatchScore()
    {
        float now = Time.time;
        if (now - lastMatchTime <= comboWindow) 
            comboCount++; else comboCount = 1;

        lastMatchTime = now;

        int points = Mathf.RoundToInt(basePoints * (1f + (comboCount - 1) * 0.5f));
        score += points;
        uiManager.UpdateScore(score, comboCount);
    }
}