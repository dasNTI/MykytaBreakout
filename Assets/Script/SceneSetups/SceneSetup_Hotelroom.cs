using UnityEngine;

public class SceneSetup_Hotelroom : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bgSr;
    [SerializeField] private Sprite[] bgSprites;
    void Start()
    {
        if (ProgressManager.State.Hotelroom_ClosetOpened)
        {
            if (ProgressManager.State.Hotelroom_HookTaken)
            {
                bgSr.sprite = bgSprites[2];
            }else
            {
                bgSr.sprite = bgSprites[1];
            }   
        }else
        {
            bgSr.sprite = bgSprites[0];
        }
    }
}
