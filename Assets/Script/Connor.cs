using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Connor : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer eyesSr;
    [SerializeField] private SpriteRenderer mouthSr;
    [SerializeField] private Sprite[] MouthSprites;

    public bool moving = false;
    public bool speaking = false;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float animationInterval = 0.25f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        eyesSr.enabled = false;
        StartCoroutine(Blink());
    }
    private void Awake()
    {
        DialogueText.connor = this;
        DialogueText.ConnorTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(2f, 7f));
            if (!moving)
            {
                eyesSr.enabled = true;
                yield return new WaitForSecondsRealtime(0.25f);
            }
            eyesSr.enabled = false;
        }
    }

    public void MoveTo(Vector3 newPos, System.Action OnComplete = null)
    {
        float distance = Vector2.Distance(newPos, transform.position);
        if (transform.position.x > newPos.x) sr.flipX = true;
        moving = true;
        eyesSr.enabled = false;
        mouthSr.enabled = false;

        StartCoroutine(Walk());
        transform.DOMove(newPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            StopCoroutine(Walk());
            sr.flipX = false;
            sr.sprite = sprites[0];
            mouthSr.enabled = true;
            mouthSr.sprite = MouthSprites[0];
            moving = false;
            if (OnComplete != null) OnComplete.Invoke();
        });
    }
    public void MoveTo(Vector3 newPos, float newScale, System.Action OnComplete = null)
    {
        float distance = Vector2.Distance(newPos, transform.position);
        transform.DOScale(Vector3.one * newScale, distance / speed).SetEase(Ease.Linear);
        MoveTo(newPos, OnComplete);
    }

    IEnumerator Walk()
    {
        while (moving)
        {
            for (int i = 1; i < sprites.Length; i++)
            {
                if (!moving) break; 
                sr.sprite = sprites[i];
                yield return new WaitForSecondsRealtime(animationInterval);
            }
        }
    }

    public IEnumerator Speak(float dur)
    {
        float timeLeft = dur;
        float interval = 0.25f;
        int currentMouthIndex = 1;
        while (timeLeft >= interval)
        {
            mouthSr.sprite = MouthSprites[currentMouthIndex];
            currentMouthIndex++;
            if (currentMouthIndex == MouthSprites.Length) currentMouthIndex = 0;
            timeLeft -= interval;
            yield return new WaitForSecondsRealtime(interval);
        }
        yield return new WaitForSecondsRealtime(timeLeft);
        mouthSr.sprite = MouthSprites[0];
    }
}
