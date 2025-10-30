using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private DialogueText dt;
    [SerializeField] private GameObject OptionPrefab;
    [SerializeField] private Image image;
    private DialogueOption[] currentOptions;

    void Start()
    {
        image.enabled = false;
    }

    public void StartDialogue(DialogueInstance dialogue)
    {
        MouseMenu.blocked = true;
        dt.DoCutscene(dialogue.lines, () =>
        {
            ShowOptions(dialogue.options);
        });
    }

    void ShowOptions(DialogueOption[] options)
    {
        MouseMenu.blocked = true;
        image.enabled = true;
        currentOptions = options;
        for (int i = 0; i < options.Length; i++)
        {
            GameObject go = Instantiate(OptionPrefab);
            go.transform.SetParent(transform, false);
            DialogueOptionButton dob = go.GetComponent<DialogueOptionButton>();
            dob.tmp.text = options[i].text;
            dob.OptionIndex = i;
            dob.ds = this;
        }
    }

    public void TakeOption(int index)
    {
        Hide();
        DialogueInstance newDia = currentOptions[index].rest;
        dt.DoCutscene(newDia.lines, () =>
        {
            if (newDia.options.Length != 0)
            {
                ShowOptions(newDia.options);
            }else
            {
                if (currentOptions[index].doesSomething)
                {

                }else
                {
                    Hide();
                    MouseMenu.blocked = false;
                }
            }
        });
    }

    void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        image.enabled = false;
    }
}

[System.Serializable]
public class DialogueInstance
{
    public DialogueLine[] lines;
    public DialogueOption[] options;
}
[System.Serializable]
public class DialogueOption
{
    public string text;
    public DialogueInstance rest;
    public bool doesSomething = false;
}