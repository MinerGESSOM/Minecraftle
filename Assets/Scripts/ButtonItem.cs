using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItem : MonoBehaviour
{
    public int idItem;

    public void buttonDown()
    {
        if (!CraftManager.Instance.Active)
        {
            return;
        }
        if(CraftManager.Instance.idItemActive == idItem)
        {
            CraftManager.Instance.idItemActive = -1;
            transform.localScale = new Vector3(1, 1, 1);
            CraftManager.Instance.activeButton = null;
        }
        else
        {
            if(CraftManager.Instance.activeButton != null)
            {
                CraftManager.Instance.activeButton.localScale = new Vector3(1, 1, 1);
            }
            CraftManager.Instance.idItemActive = idItem;
            CraftManager.Instance.activeButton = transform;
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        }
    }
}
