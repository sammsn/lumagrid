using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Queue<Card> flipQueue = new Queue<Card>();

    public int score { get; private set; }
    private int basePoints = 100;

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

    public void StartGame()
    {
        score = 0;
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

        }
        else
        {
            yield return new WaitForSeconds(0.45f);
            a.StartCoroutine(a.Flip(false));
            b.StartCoroutine(b.Flip(false));
        }
    }

    void ApplyMatchScore()
    {
        score += basePoints;
    }
}