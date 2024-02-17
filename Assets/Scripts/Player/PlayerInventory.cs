using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int lumber;
    public TMP_Text inventoryLumberText;

    public int ExtractAllLumber()
    {
        var extractedLumber = lumber;
        lumber = 0;
        inventoryLumberText.text = lumber.ToString();
        return extractedLumber;
    }

    public void AddLumber(int amt)
    {
        lumber += amt;
        inventoryLumberText.text = lumber.ToString();
    }
}
