using UnityEngine;

public class SceneSetup_Reception : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private Sprite[] bgSprites;
    [SerializeField] private Clickable MopClickable;

    void Start()
    {
        if (ProgressManager.State.Reception_MopTaken)
        {
            bgSr.sprite = bgSprites[1];
            MopClickable.active = false;
        }else
        {
            bgSr.sprite = bgSprites[0];
            MopClickable.active = true;
        }
    }
}
