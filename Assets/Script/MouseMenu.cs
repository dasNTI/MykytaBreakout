using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseMenu : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image image;
    [SerializeField] private GameObject MouseInfo;
    [SerializeField] private TextMeshProUGUI ObjectName;

    public bool blocked = false;
    public string currentHoveredObject = "";

    private int currentObjectId;
    private Clickable Obj;
    private bool available = false;
    private bool active = false;

    void Start()
    {
        MouseInfo.SetActive(false);
        image.enabled = false;
    }

    public void MakeAvailable(int id, string label, Clickable reference)
    {
        if (blocked) return;
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

    // Update is called once per frame
    void Update()
    {
        if (!available && !active) return;
        if (Input.GetMouseButtonDown(0) && !active)
        {
            active = true;
            image.enabled = true;
            image.sprite = sprites[0];
        }
        else if (Input.GetMouseButtonDown(1) && !active)
        {
            blocked = true;
            Obj.HandleClick(Obj.StandardType);
            MouseInfo.SetActive(false);
        }

        if (active && Input.GetMouseButtonUp(0))
        {
            
        }
    }
}
