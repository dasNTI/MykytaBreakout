using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class MouseMenu : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;

    [SerializeField] private TextMeshProUGUI ObjectName;
    [SerializeField] private GameObject MouseInfo;
    [SerializeField] private RectTransform canvasRt;
    [SerializeField] private GameObject InventoryContainer;
    [SerializeField] private Inventory inventory;

    public static bool blocked = false;
    public string currentHoveredObject = "";
    [SerializeField] private float middleThreshold = 10;

    private int currentObjectId;
    private Clickable Obj;
    private bool available = false;
    private bool active = false;
    private bool inventoryActive;
    private int selectedOption = 0;
    Vector2 initMousePosition;

    void Start()
    {
        MouseInfo.SetActive(false);
        image.enabled = false;
    }

    public void MakeAvailable(int id, string label, Clickable reference)
    {
        if (blocked) return;
        if (active) return;
        currentObjectId = id;
        currentHoveredObject = label;
        MouseInfo.SetActive(true);
        ObjectName.text = label;
        Obj = reference;
        available = true;
    }

    public void StopAvailable(int id)
    {
        if (blocked) return;
        if (active) return;
        if (currentObjectId != id) return;
        currentHoveredObject = "";
        MouseInfo.SetActive(false);
        available = false;
    }

    void Update()
    {
        if (!available && !active) return;
        if (Input.GetMouseButtonDown(0) && !active)
        {
            active = true;
            image.enabled = true;
            image.sprite = sprites[0];

            //Cursor.visible = false;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, Input.mousePosition, Camera.main, out pos);
            transform.localPosition = pos;
            initMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonDown(1) && !active)
        {
            blocked = true;
            Obj.HandleClick(Obj.StandardType);
            MouseInfo.SetActive(false);
        }

        if (active) HandleMouseMenu();

        if (active && Input.GetMouseButtonUp(0))
        {
            active = false;
            if (selectedOption == 0) blocked = false;

            //Cursor.visible = true;
            image.enabled = false;
            MouseInfo.SetActive(false);

            switch (selectedOption)
            {
                case 0:
                    break;
                case 1:
                    Obj.HandleClick(InteractionType.Interact);
                    break;
                case 2:
                    Obj.HandleClick(InteractionType.TalkTo);
                    break;
                case 4:
                    Obj.HandleClick(InteractionType.Take);
                    break;
                case 3:
                    blocked = true;
                    InventoryContainer.SetActive(true);
                    inventory.Activate(Obj);
                    break;
            }
            available = false;
        }
    }

    void HandleMouseMenu()
    {
        Vector2 dif = (Vector2)Input.mousePosition - initMousePosition;
        if (dif.magnitude < middleThreshold)
        {   
            image.sprite = sprites[0];
            selectedOption = 0;
            return;
        }

        float x = Mathf.Sign(dif.x);
        float y = Mathf.Sign(dif.y);
        if (x == 0 && y == 0) return;

        switch ((x, y))
        {
            case (-1, 1):
                image.sprite = sprites[1]; // links oben
                selectedOption = 1;
                break;
            case (1, 1):
                image.sprite = sprites[2];
                selectedOption = 2;
                break;
            case (1, -1):
                image.sprite = sprites[3];
                selectedOption = 3;
                break;
            case (-1, -1):
                image.sprite = sprites[4];
                selectedOption = 4;
                break;
        }
    }
}
