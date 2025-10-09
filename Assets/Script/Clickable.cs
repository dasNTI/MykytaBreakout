using UnityEngine;

public class Clickable : MonoBehaviour
{
    public InteractionType StandardType = InteractionType.Interact;
    [SerializeField] private string ClickableName = "Test";
    private BoxCollider2D bc;
    private int objectId;

    [SerializeField] private MouseMenu mouseMenu;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        objectId = GetInstanceID(); 
    }

    private void OnMouseEnter()
    {
        mouseMenu.MakeAvailable(objectId, ClickableName, this);
    }
    private void OnMouseExit()
    {
        mouseMenu.StopAvailable(objectId);
    }

    public void HandleClick(InteractionType type)
    {

    }
}

[System.Serializable]
public enum InteractionType
{
    Interact = 0,
    Inspect = 1,
    TalkTo = 2,
    UseItem = 3
}