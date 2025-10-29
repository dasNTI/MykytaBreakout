using UnityEngine;
using System.Collections;

public class TalkingCharacter : MonoBehaviour
{
    [SerializeField] private Sprite[] MouthSprites;
    [SerializeField] private SpriteRenderer mouthSr;
    [SerializeField] private SpriteRenderer eyesSr;
    public Vector3 textPosition;

    [SerializeField] private bool isSean = false;
    [SerializeField] private bool isJude = false;

    void Start()
    {
        if (isSean)
        {
            DialogueText.SeanPosition = textPosition;
            DialogueText.sean = this;
        }else if (isJude)
        {
            DialogueText.JudePosition = textPosition;
            DialogueText.jude = this;
        }
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(2f, 7f));
            eyesSr.enabled = true;
            yield return new WaitForSecondsRealtime(0.25f);
            eyesSr.enabled = false;
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
