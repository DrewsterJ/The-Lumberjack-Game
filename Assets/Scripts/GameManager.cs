using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int collectedLumber;
    
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

    public void AddLumber(int amt)
    {
        collectedLumber += amt;
    }
}
