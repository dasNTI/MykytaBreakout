using UnityEngine;

public class SectionManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefab;
    void Start()
    {
        if (!GameObject.Find("Canvas"))
        {
            GameObject go = Instantiate(UIPrefab);
            DontDestroyOnLoad(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
