using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvBonus : MonoBehaviour
{
    public  float tickX2 = 0;
    public bool ActiveX2 = false;
    public float tickX3 = 0;
    public bool ActiveX3 = false;

    [SerializeField] TextMeshProUGUI textTime1;
    [SerializeField] TextMeshProUGUI textTime2;
    [SerializeField] VKAdManager AddM;

    string simvol;

    void Start()
    {
        simvol = "с";
        /*
        if(true)//Язык
        {
            simvol = "с";
        }
        else
        {
            simvol = "s";
        }
        */
    }

    private void Update()
    {
        if(tickX2 > 0)
        {
            tickX2 -= Time.deltaTime;
            textTime1.text = Mathf.Round(tickX2).ToString() + simvol;
        }
        else if(ActiveX2)
        {
            tickX2 = 0;
            ActiveX2 = false;
            textTime1.text = "";
            CraftManager.Instance.MnX = 1;
        }

        if(tickX3 > 0)
        {
            tickX3 -= Time.deltaTime;

            textTime2.text = Mathf.Round(tickX3).ToString() + simvol;
        }
        else if(ActiveX3)
        {
            tickX3 = 0;
            ActiveX3 = false;
            textTime2.text = "";
            CraftManager.Instance.MnX = 1;
        }
    }


    public void ButtonBonus2()
    {
        if (ActiveX3 || ActiveX2)
        {
            return;
        }
        //Реклама
        AddM.ShowRewarded(success =>
        {
            if (success)
            {
                CraftManager.Instance.MnX = 2;
            }
        });
    }

    public void ButtonBonus3()
    {
        if (ActiveX2 || ActiveX3)
        {
            return;
        }
        //Реклама
        AddM.ShowRewarded(success =>
        {
            if (success)
            {
                CraftManager.Instance.MnX = 3;
            }
        });
    }
}
