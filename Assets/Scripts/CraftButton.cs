using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] int id;

    public void ButtonDown()
    {
        if (!CraftManager.Instance.Active)
        {
            return;
        }
        if(CraftManager.Instance.idItemActive != -1)
        {
            
            image.sprite =  CraftManager.Instance.sprites[CraftManager.Instance.idItemActive];
            CraftManager.Instance.CraftItem[id] = CraftManager.Instance.idItemActive;
            CraftManager.Instance.Proverka();
        }
    }

    private void OnEnable()
    {
        image.sprite = null;
    }
}
