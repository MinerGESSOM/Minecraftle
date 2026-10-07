using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    [SerializeField] CraftButton[] craftButtons;

    public void ButtonDown()
    {
        if (!CraftManager.Instance.Active)
        {
            return;
        }
        for(int i = 0; i < 9; i++)
        {
            craftButtons[i].image.sprite = null;
            CraftManager.Instance.CraftItem[i] = -1;
            CraftManager.Instance.Proverka();
        }
    }

    public void Next()
    {
        for(int i = 0; i < 9; i++)
        {
            craftButtons[i].image.sprite = null;
            CraftManager.Instance.CraftItem[i] = -1;
            CraftManager.Instance.Proverka();
        }
    }
}
