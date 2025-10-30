using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Music").Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
