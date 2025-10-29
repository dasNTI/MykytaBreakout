using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public InteractionType StandardType = InteractionType.Interact;
    [SerializeField] public string ClickableName = "Test";
    private BoxCollider2D bc;
    private int objectId;
    public bool active = true;

    [SerializeField] private MouseMenu mouseMenu;
    [SerializeField] private DialogueLine[] InteractVl;
    [SerializeField] private DialogueLine[] TakeVl;
    [SerializeField] private DialogueLine[] TalkVl;
    [SerializeField] public DialogueLine[] ItemVl;

    [SerializeField] private InteractionType FurtherReactionInteractionType;
    [SerializeField] public bool isDecor = false;
    public Items[] AcceptedItems;
    [SerializeField] private ClickableAction FurtherReaction;

    private DialogueText dt;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        objectId = GetInstanceID(); 
    }
    private void Start()
    {
        if (!mouseMenu) mouseMenu = GameObject.Find("MouseMenu").GetComponent<MouseMenu>();
        dt = GameObject.Find("DialogueText").GetComponent<DialogueText>();
    }
        
    private void OnMouseEnter()
    {
        if (!active) return;
        mouseMenu.MakeAvailable(objectId, ClickableName, this);
    }
    private void OnMouseExit()
    {
        if (!active) return;
        mouseMenu.StopAvailable(objectId);
    }

    public void HandleClick(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.Interact:
                dt.DoCutscene(InteractVl);
                break;
            case InteractionType.Take:
                dt.DoCutscene(TakeVl);
                break;
            case InteractionType.TalkTo:
                dt.DoCutscene(TalkVl);
                break;
        }
        
        if (!isDecor && type == FurtherReactionInteractionType)
        {
            FurtherReaction.DoAction(type);
        }
    }

    public void HandleClick(InteractionType type, Items itemId)
    {
        
    }
}

[System.Serializable]
public enum InteractionType
{
    Interact = 0,
    Take = 1,
    TalkTo = 2,
    UseItem = 3
}

public abstract class ClickableAction : MonoBehaviour
{
    public abstract void DoAction(InteractionType type, Items item = Items.Empty);
}