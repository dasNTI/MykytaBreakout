using UnityEngine;

public class DialogueAction : ClickableAction
{
    [SerializeField] private DialogueSystem ds;
    [SerializeField] private DialogueInstance dialogue;
    public override void DoAction(InteractionType type, Items item = Items.Empty)
    {
        ds.StartDialogue(dialogue);
    }
}
