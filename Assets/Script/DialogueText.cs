using System.Collections;
using UnityEngine;

public class DialogueText : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRt;
    [SerializeField] private AudioSource audio;
    [SerializeField] private TMPro.TextMeshProUGUI tmp;
    [SerializeField] private Color ConnorColor;
    [SerializeField] private Color JudeColor;
    [SerializeField] private Color SeanColor;

    public static Transform ConnorTransform;
    public static Connor connor;
    public static Vector3 JudePosition;
    public static TalkingCharacter jude;
    public static Vector3 SeanPosition;
    public static TalkingCharacter sean;
    public static bool TalkingActive = false;
    [SerializeField] private float ConnorOffset = 5;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoCutscene(DialogueLine[] dialog)
    {
        MouseMenu.blocked = true;
        TalkingActive = true;
        StartCoroutine(cutscene(dialog));
    }
    IEnumerator cutscene(DialogueLine[] dialog)
    {

        for (int i = 0; i < dialog.Length; i++)
        {
            DialogueLine line = dialog[i];
            switch (line.character)
            {
                case Character.Connor:
                    ConnorSays(line, false);
                    break;
                case Character.Sean:
                    SeanSays(line, false);
                    break;
            }
            yield return new WaitForSecondsRealtime(line.clip.length);
        }
        tmp.text = "";
        MouseMenu.blocked = false;
        TalkingActive = false;
    }

    public void ConnorSays(DialogueLine dl, bool endText = true)
    {
        audio.clip = dl.clip;
        tmp.color = ConnorColor;
        MoveTextTo(GetConnorPos());
        StartCoroutine(ChangeText(new DialogueLine[] { dl }, dl.clip.length, endText));
        connor.StartCoroutine(connor.Speak(dl.clip.length));
        audio.Play();
    }

    public void SeanSays(DialogueLine dl, bool endText = true)
    {
        audio.clip = dl.clip;
        tmp.color = SeanColor;
        MoveTextTo(SeanPosition);
        StartCoroutine(ChangeText(new DialogueLine[] { dl }, dl.clip.length, endText));
        sean.StartCoroutine(sean.Speak(dl.clip.length));
        audio.Play();
    }

    IEnumerator ChangeText(DialogueLine[] parts, float minDuration, bool endText)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            tmp.text = parts[i].text;
            yield return new WaitForSeconds(parts[i].clip.length);
        }
        if (parts.Length == 1)
        {
            yield return new WaitForSeconds(minDuration - parts[0].clip.length);
        }
        if (endText) tmp.text = "";
    }

    void MoveTextTo(Vector3 WorldPos)
    {
        Vector3 screen = RectTransformUtility.WorldToScreenPoint(Camera.main, WorldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, screen, Camera.main, out Vector2 pos);
        transform.localPosition = pos;
    }

    Vector3 GetConnorPos()
    {
        return ConnorTransform.position + Vector3.up * ConnorOffset * ConnorTransform.localScale.x;
    }
}

public enum Character
{
    Connor = 0,
    Jude = 1,
    Sean = 2,
    Zimmernachbar = 3
}

[System.Serializable]
public class DialogueLine
{
    public Character character;
    public AudioClip clip;
    public string text;
}