using System.Collections;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public InteractionType StandardType = InteractionType.Interact;
    [SerializeField] public string ClickableName = "Test";
    private BoxCollider2D bc;
    private int objectId;
    public bool active = true;

    [SerializeField] private MouseMenu mouseMenu;

    void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        objectId = GetInstanceID(); 
    }
    private void Start()
    {
        if (!mouseMenu) mouseMenu = GameObject.Find("MouseMenu").GetComponent<MouseMenu>();
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

    public IEnumerator HandleClick(InteractionType type)
    {
        Debug.Log("yeet");
        yield return null;
    }

    public IEnumerator HandleClick(InteractionType type, int itemId)
    {
        yield return null;
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