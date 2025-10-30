using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private TMPro.TextMeshProUGUI LeftClickAction;
    [SerializeField] private TMPro.TextMeshProUGUI RightClickAction;
    [SerializeField] public MouseMenu mouseMenu;

    private Dictionary<Items, int> itemIndices = new Dictionary<Items, int>();
    private Items _combineItem = Items.Empty;
    public Items CombineItem
    {
        get { return _combineItem; }
        set { 
            if (value != Items.Empty) CombineItemName = itemInformation[itemIndices[value]].name;
            _combineItem = value;
        }
    }
    public string CombineItemName;
    public Clickable Obj;
    public string ClickableName;
   
    [SerializeField] private ItemInformation[] itemInformation;
    [SerializeField] private ItemCombination[] AllowedCombinations;
    public static Items[] StaticSlotItems = null;

    [SerializeField] private AudioSource CombinationFail;
    [SerializeField] private DialogueLine CombinationFailVl;
    [SerializeField] private AudioSource CombinationSuccess;
    [SerializeField] private AudioSource ReceiveItemAudio;
    public DialogueText dt;
    public DialogueLine[] ItemDeclineVl;

    private void Awake()
    {
        FillItemIndices();
    }

    private void Start()
    {
        if (StaticSlotItems == null)
        {
            StaticSlotItems = new Items[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                StaticSlotItems[i] = slots[i].OwnItem;
            }
        }else
        {
            for (int i = 0;i < slots.Length; i++)
            {
                slots[i].OwnItem = StaticSlotItems[i];
            }
        }
        AssignImages();
    }

    public void Resort()
    {
        bool reached = false;
        for (int i = slots.Length - 1; i >= 0; i--)
        {
            if (slots[i].OwnItem != Items.Empty)
            {
                reached = true;
                continue;
            }
            else if (reached)
            {
                for (int j = i; j < slots.Length; j++)
                {
                    if (slots[j + 1].OwnItem == Items.Empty)
                    {
                        slots[j].OwnItem = Items.Empty;
                        break;
                    }
                    slots[j].OwnItem = slots[j + 1].OwnItem;
                }
            }
        }
    }

    public void AddItem(Items item)
    {
        for (int i = 0 ; i < slots.Length; i++)
        {
            if (slots[i].OwnItem == Items.Empty)
            {
                slots[i].OwnItem = item;
                StaticSlotItems[i] = item;
                break;
            }
        }
        AssignImages();
    }

    public void AdjustSelectedItemInformation(Items item)
    {
        if (item == Items.Empty)
        {
            LeftClickAction.text = "Schließen";
            RightClickAction.text = "Schließen";
            return;
        }

        if (CombineItem == Items.Empty)
        {
            RightClickAction.text = "Kombinieren";
        }else if (CombineItem == item)
        {
            RightClickAction.text = "Nicht mehr kombinieren";
        }else
        {
            RightClickAction.text = "Kombinieren mit " + CombineItemName;
        }

        LeftClickAction.text = itemInformation[itemIndices[item]].name + " benutzen";
    }

    public void AssignImages()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Items ownItem = slots[i].OwnItem;
            if (ownItem == Items.Empty)
            {
                slots[i].ItemImage.enabled = false;
                continue;
            }
            Sprite s = itemInformation[itemIndices[ownItem]].sprite;
            slots[i].ItemImage.enabled = true;
            slots[i].ItemImage.sprite = s; 
        }
    }

    private void FillItemIndices()
    {
        for (int i = 0; i < itemInformation.Length; i++)
        {
            Items item = itemInformation[i].item;
            if (itemIndices.ContainsKey(item)) continue;
            itemIndices[item] = i;
        }
    }

    public void Activate(Clickable ActivatingObject)
    {
        Obj = ActivatingObject;
        CombineItem = Items.Empty;
        ClickableName = Obj.ClickableName;
        foreach (InventorySlot s in slots)
        {
            s.MarkedAsCombining = false;
            s.Deselect();
            s.OnScreen = true;
        }
        slots[0].Select();
    }
    public void Deactivate()
    {
        foreach (InventorySlot s in slots) s.OnScreen = false;
        gameObject.SetActive(false);
    }

    public void CancelInventory()
    {
        MouseMenu.blocked = false;
        Deactivate();
    }

    public void CreateCombination(Items additionalItem)
    {
        ItemCombination combination = null;
        for (int i = 0; i < AllowedCombinations.Length; i++)
        {
            ItemCombination c = AllowedCombinations[i];
            if ((c.item1 == additionalItem && c.item2 == CombineItem) || (c.item1 == CombineItem && c.item2 == additionalItem))
            {
                combination = c;
                break;
            }
        }

        if (combination == null)
        {
            DeclineCombination();
            ResetCombiningItems();
            return;
        }

        AcceptCombination(combination);
        ResetCombiningItems();
        CombineItemName = "";
        bool firstOneFound = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (!(slots[i].OwnItem == combination.item1 || slots[i].OwnItem == combination.item2)) continue;
            if (slots[i].OwnItem == Items.Zange) continue;

            if (firstOneFound)
            {
                slots[i].OwnItem = Items.Empty;
            }else
            {
                slots[i].OwnItem = combination.result;
                firstOneFound = true;
            }
        }
        Resort();
        AssignImages();
    }

    void ResetCombiningItems()
    {
        CombineItem = Items.Empty;
        foreach (InventorySlot slot in slots)
        {
            slot.MarkedAsCombining = false;
            slot.RefreshImages();
        }
    }

    private void DeclineCombination()
    {
        CombinationFail.Play();
        GameObject.Find("DialogueText").GetComponent<DialogueText>().ConnorSays(CombinationFailVl);
        Debug.Log("ne bruder lass mal");
    }
    private void AcceptCombination(ItemCombination result)
    {
        CombinationSuccess.Play();
        Debug.Log("Banger KOmbination bruder");
    }
}

[System.Serializable]
public enum Items
{
    Empty = -1,
    Wischmop = 0,
    Kleiderhaken = 1,
    Schnürsenkel = 2,
    KleiderhakenPlusMop = 3,
    Dachbodenstock = 4,
    Zange = 5,
    Leiter = 6,
    Gardine = 7
}

[System.Serializable]
public class ItemInformation
{
    public Items item;
    public Sprite sprite;
    public string name;
}

[System.Serializable]
public class ItemCombination
{
    public Items item1;
    public Items item2;
    public Items result;
}