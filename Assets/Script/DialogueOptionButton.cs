using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueOptionButton : MonoBehaviour, IPointerDownHandler
{
    public DialogueSystem ds;
    public TMPro.TextMeshProUGUI tmp;
    public int OptionIndex;

    public void OnPointerDown(PointerEventData _)
    {
        ds.TakeOption(OptionIndex);
    }
}
