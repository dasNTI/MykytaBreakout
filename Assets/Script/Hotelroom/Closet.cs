using System.Collections;
using UnityEngine;

public class Closet : ClickableAction
{
    [SerializeField] private Clickable parent;
    [SerializeField] private bool RepresentsClosedDoor = true;
    [SerializeField] private Sprite ChangedBg;
    [SerializeField] private Sprite AltBg;
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private Clickable OtherClosetClickable;
    [SerializeField] private Clickable HookClickable;
    [SerializeField] private AudioSource audio;
    [SerializeField] private DialogueLine OpeningLine1;
    [SerializeField] private DialogueLine OpeningLine2;
    [SerializeField] private DialogueText dt;

    public override void DoAction(InteractionType type, Items _ = Items.Empty)
    {
        if (RepresentsClosedDoor)
        {
            if (!ProgressManager.State.Hotelroom_ClosetFirstOpened && !ProgressManager.State.Hotelroom_ClosetOpened)
            {
                parent.StartCoroutine(OpeningCloset());
            }
            else if (!ProgressManager.State.Hotelroom_ClosetOpened)
            {
                ProgressManager.State.Hotelroom_ClosetOpened = true;
                audio.Play();
                if (!ProgressManager.State.Hotelroom_HookTaken)
                {
                    bgSr.sprite = ChangedBg;
                    HookClickable.active = true;
                }else
                {
                    bgSr.sprite = AltBg;
                    HookClickable.active = false;
                }
                parent.active = false;
                OtherClosetClickable.active = true;
            }
        }else
        {
            ProgressManager.State.Hotelroom_ClosetOpened = false;
            bgSr.sprite = ChangedBg;
            HookClickable.active = false;
            OtherClosetClickable.active = true;
            parent.active = false;
            audio.Play();
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
        OtherClosetClickable.active = true;
        HookClickable.active = true;
    }
}
