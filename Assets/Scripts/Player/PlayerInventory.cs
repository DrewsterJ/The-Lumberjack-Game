using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int lumber;

    public void AddLumber(int amt)
    {
        lumber += amt;
    }
}
