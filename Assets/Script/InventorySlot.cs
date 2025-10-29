using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private InventorySlot ToTheLeft;
    [SerializeField] private InventorySlot ToTheRight;
    [SerializeField] private Image SlotImage;
    [SerializeField] private Inventory inventory;
    public Image ItemImage;
    public Items OwnItem = Items.Empty;

    private bool selected = false;
    public bool MarkedAsCombining = false;
    private static int scrollBuffer = 0;
    public bool OnScreen = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnScreen || !selected) return;

        if (scrollBuffer > 0) scrollBuffer--;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && ToTheRight != null && scrollBuffer == 0)
        {
            Deselect();
            ToTheRight.Select();
            scrollBuffer = 5;
        }else if (Input.GetAxis("Mouse ScrollWheel") > 0 && ToTheLeft != null && scrollBuffer == 0)
        {
            Deselect();
            ToTheLeft.Select();
            scrollBuffer = 5;
        }

        if (OwnItem == Items.Empty && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            inventory.CancelInventory();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (inventory.Obj.isDecor)
            {
                inventory.dt.DoCutscene(inventory.ItemDeclineVl);
                inventory.Deactivate();
                return;
            }

            bool found = false;
            if (inventory.Obj.AcceptedItems.Length != 0)
            {
                for (int i = 0; i < inventory.Obj.AcceptedItems.Length; i++)
                {
                    if (inventory.Obj.AcceptedItems[i] == OwnItem)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (found)
            {
                inventory.Obj.HandleClick(InteractionType.UseItem, OwnItem);
                inventory.dt.DoCutscene(inventory.Obj.ItemVl);
            }else
            {
                inventory.dt.DoCutscene(inventory.ItemDeclineVl);
            }
            inventory.Deactivate();
        }else if (Input.GetMouseButtonDown(1)) { 
            HandleCombining();
            Select();
            inventory.AdjustSelectedItemInformation(OwnItem);
        }
    }

    private void HandleCombining()
    {
        if (inventory.CombineItem != Items.Empty && !MarkedAsCombining)
        {
            inventory.CreateCombination(OwnItem);
        }
        else if (MarkedAsCombining)
        {
            MarkedAsCombining = false;
            inventory.CombineItem = Items.Empty;
        }
        else
        {
            inventory.CombineItem = OwnItem;
            MarkedAsCombining = true;
        }
    }

    public void Deselect()
    {
        if (MarkedAsCombining)
        {
            SlotImage.sprite = sprites[1];
        }
        else
        {
            SlotImage.sprite = sprites[0];
        }
        selected = false;
    }

    public void Select()
    {
        if (MarkedAsCombining)
        {
            SlotImage.sprite = sprites[3];
        }
        else
        {
            SlotImage.sprite = sprites[2];
        }
        selected = true;
        inventory.AdjustSelectedItemInformation(OwnItem);
    }

    public void RefreshImages()
    {
        if (selected)
        {
            Select();
        }else
        {
            Deselect();
        }
    }

    public void DeselectRecursiveLeft()
    {
        Deselect();
        if (ToTheLeft) ToTheLeft.DeselectRecursiveLeft();
    }
    public void DeselectRecursiveRight()
    {
        Deselect();
        if (ToTheRight) ToTheRight.DeselectRecursiveRight();
    }

    public void OnPointerEnter(PointerEventData _)
    {
        if (!OnScreen) return;
        if (ToTheLeft) ToTheLeft.DeselectRecursiveLeft();
        if (ToTheRight) ToTheRight.DeselectRecursiveRight();
        Select();
    }
}