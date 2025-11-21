using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    public int id; 
    public Image frontImage;
    public Image backImage; 
    public float flipDuration = 0.22f;
    public bool IsMatched { get; private set; }
    public bool IsFaceUp { get; private set; }
    public bool IsAnimating { get; private set; }
    Button button;
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
    }
    public void Initialize(int id, Sprite frontSprite, Sprite backSprite)
    {
        this.id = id;
        frontImage.sprite = frontSprite;
        backImage.sprite = backSprite;
        IsMatched = false;
        IsFaceUp = false;
        SetVisualInstant(false);
    }
    void OnClicked()
    {
        if (IsMatched || IsAnimating || IsFaceUp) return;
        StartCoroutine(Flip(true));
    }

    public IEnumerator Flip(bool faceUp)
    {
        IsAnimating = true;
        float t = 0f;
        float half = flipDuration / 2f;

        while (t < half)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / half);
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, p), 1f, 1f);
            yield return null;
        }

        IsFaceUp = faceUp;
        frontImage.gameObject.SetActive(IsFaceUp);
        backImage.gameObject.SetActive(!IsFaceUp);

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / half);
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, p), 1f, 1f);
            yield return null;
        }
        transform.localScale = Vector3.one;
        IsAnimating = false;
        yield break;
    }
    public void SetMatched()
    {
        IsMatched = true;
        button.interactable = false;
    }
    public void RevealInstant()
    {
        StopAllCoroutines();
        IsFaceUp = true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        IsAnimating = false;
    }
    public void HideInstant()
    {
        StopAllCoroutines();
        IsFaceUp = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        IsAnimating = false;
    }
    void SetVisualInstant(bool faceUp)
    {
        IsFaceUp = faceUp;
        frontImage.gameObject.SetActive(faceUp);
        backImage.gameObject.SetActive(!faceUp);

    }
}