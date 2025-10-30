using UnityEngine;

public class Mop : ClickableAction
{
    [SerializeField] private Clickable parent;
    [SerializeField] private Sprite bgSprite;
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private Inventory inventory;
    public override void DoAction(InteractionType type, Items item = Items.Empty)
    {
        ProgressManager.State.Reception_MopTaken = true;
        bgSr.sprite = bgSprite;
        parent.active = false;
        inventory.AddItem(Items.Wischmop);
    }
}
