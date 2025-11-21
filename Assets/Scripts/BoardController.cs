using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public RectTransform boardContainer;
    public GameObject cardPrefab;
    public Sprite backSprite;
    public List<Sprite> frontSprites;
    public GridLayoutGroup grid;

    private List<Card> spawnedCards = new List<Card>();
    private int rows, cols;


    public void SetupBoard(int _rows, int _cols)
    {
        rows = _rows;
        cols = _cols;

        ClearBoard();

        int total = rows * cols;

        if (total % 2 != 0)
            Debug.LogWarning("unmatched pair detected");

        List<int> ids = new List<int>();
        for (int i = 0; i < total / 2; i++)
        {
            ids.Add(i); ids.Add(i);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            int r = Random.Range(i, ids.Count);
            int tmp = ids[r]; ids[r] = ids[i]; ids[i] = tmp;
        }


        for (int i = 0; i < total; i++)
        {
            var go = Instantiate(cardPrefab, boardContainer);
            var card = go.GetComponent<Card>();
            int cardId = ids[i];
            Sprite front = frontSprites[cardId % frontSprites.Count];
            card.Initialize(cardId, front, backSprite);
            spawnedCards.Add(card);
        }
    }

    void ConfigureGridCellSize(int rows, int cols)
    {
        float paddingX = grid.padding.left + grid.padding.right;
        float paddingY = grid.padding.top + grid.padding.bottom;
        float spacingX = grid.spacing.x * (cols - 1);
        float spacingY = grid.spacing.y * (rows - 1);

        float w = boardContainer.rect.width - paddingX - spacingX;
        float h = boardContainer.rect.height - paddingY - spacingY;

        float cellW = Mathf.Floor(w / cols);
        float cellH = Mathf.Floor(h / rows);

        grid.cellSize = new Vector2(cellW, cellH);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;
    }

    public bool AllMatched()
    {
        foreach (var c in spawnedCards)
        {
            if (!c.IsMatched)
                return false;
        }
        return true;
    }

    public void ClearBoard()
    {
        foreach (Transform t in boardContainer) Destroy(t.gameObject);
        spawnedCards.Clear();
    }

    public BoardState SerializeState()
    {
        BoardState st = new BoardState();
        st.rows = rows; st.cols = cols;
        st.ids = new List<int>();
        st.matched = new List<bool>();
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            st.ids.Add(spawnedCards[i].id);
            st.matched.Add(spawnedCards[i].IsMatched);
        }
        return st;
    }

    public void DeserializeState(BoardState st)
    {
        ClearBoard();
        rows = st.rows;
        cols = st.cols;

        ConfigureGridCellSize(rows, cols);

        for (int i = 0; i < st.ids.Count; i++)
        {
            var go = Instantiate(cardPrefab, boardContainer);
            var card = go.GetComponent<Card>();
            int cardId = st.ids[i];
            Sprite front = frontSprites[cardId % frontSprites.Count];

            card.Initialize(cardId, front, backSprite);
            if (st.matched[i])
            {
                card.SetMatched();
                card.RevealInstant();
            }

            spawnedCards.Add(card);
        }
    }
}

[System.Serializable]
public class BoardState
{
    public int rows, cols;
    public List<int> ids;
    public List<bool> matched;
}
