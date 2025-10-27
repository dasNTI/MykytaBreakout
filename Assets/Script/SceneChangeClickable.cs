using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeClickable : MonoBehaviour
{
    [SerializeField] private Vector3 InteractPosition;
    [SerializeField] private float InteractScale;
    [SerializeField] private bool move = true;
    [SerializeField] private Connor connor;
    [SerializeField] private Texture2D CursorImage;
    [SerializeField] private AudioSource audio;
    public bool active = true;
    public string SceneName;

    private void OnMouseOver()
    {
        if (!active) return;
        if (MouseMenu.blocked) return;
        Cursor.SetCursor(CursorImage, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        if (!active) return;
        if (MouseMenu.blocked) return;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        Cursor.SetCursor(null, new Vector2(0.5f, 0.5f), CursorMode.Auto);
        MouseMenu.blocked = true;
        active = false;
        if (move)
        {
            connor.MoveTo(InteractPosition, InteractScale, () =>
            {
                if (audio)
                {
                    StartCoroutine(ChangeWithSound());
                }
                else
                {
                    MouseMenu.blocked = false;
                    SceneManager.LoadScene(SceneName);
                }
            });
        }else
        {
            if (audio)
            {
                StartCoroutine(ChangeWithSound());
            }
            else
            {
                MouseMenu.blocked = false;
                SceneManager.LoadScene(SceneName);
            }
        }
    }

    IEnumerator ChangeWithSound()
    {
        audio.Play();
        yield return new WaitForSecondsRealtime(audio.clip.length);
        MouseMenu.blocked = false;
        SceneManager.LoadScene(SceneName);
    }
}
