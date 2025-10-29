using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class Closet : ClickableAction
{
    [SerializeField] private Clickable parent;
    [SerializeField] private bool RepresentsClosedDoor = true;
    [SerializeField] private Sprite ChangedBg;
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private Clickable OtherClosetClickable;
    [SerializeField] private AudioSource audio;
    [SerializeField] private DialogueLine OpeningLine1;
    [SerializeField] private DialogueLine OpeningLine2;
    [SerializeField] private DialogueText dt;

    public override void DoAction(InteractionType type, Items _ = Items.Empty)
    {
        if (!ProgressManager.State.Hotelroom_ClosetFirstOpened && !ProgressManager.State.Hotelroom_ClosetOpened && RepresentsClosedDoor)
        {
            parent.StartCoroutine(OpeningCloset());
        }else if (ProgressManager.State.Hotelroom_ClosetOpened)
        {

        }
    }

    IEnumerator OpeningCloset()
    {
        MouseMenu.blocked = true;
        ProgressManager.State.Hotelroom_ClosetFirstOpened = true;
        dt.ConnorSays(OpeningLine1);
        yield return new WaitForSecondsRealtime(OpeningLine1.clip.length);
        audio.Play();
        bgSr.sprite = ChangedBg;
        ProgressManager.State.Hotelroom_ClosetOpened = true;
        yield return new WaitForSecondsRealtime(audio.clip.length);
        dt.ConnorSays(OpeningLine2);
        yield return new WaitForSecondsRealtime(OpeningLine2.clip.length);
        MouseMenu.blocked = false;
        parent.active = false;
    }
}
