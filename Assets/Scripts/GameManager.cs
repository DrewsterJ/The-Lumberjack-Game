using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    // https://discussions.unity.com/t/detecting-when-applicationdidbecomeactive-in-unity/47093
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
