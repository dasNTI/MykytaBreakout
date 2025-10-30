using UnityEngine;

public class Hanger : ClickableAction
{
    [SerializeField] private Clickable parent;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Sprite bgSprite;
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private AudioSource audio;
    public override void DoAction(InteractionType type, Items item = Items.Empty)
    {
        inventory.AddItem(Items.Kleiderhaken);
        bgSr.sprite = bgSprite;

        ProgressManager.State.Hotelroom_HookTaken = true;
        ProgressManager.State.Inventory_Hook = true;
        audio.Play();
        parent.active = false;
    }
}