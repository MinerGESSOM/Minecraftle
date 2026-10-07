using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class AdvBonus : MonoBehaviour
{
    float tickX2 = 0;
    bool ActiveX2 = false;
    float tickX3 = 0;
    bool ActiveX3 = false;

    [SerializeField] TextMeshProUGUI textTime1;
    [SerializeField] TextMeshProUGUI textTime2;

    string simvol;

    void Start()
    {
        if(YG2.lang == "ru")
        {
            simvol = "с";
        }
        else
        {
            simvol = "s";
        }
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
        if (ActiveX3)
        {
            return;
        }
        YG2.RewardedAdvShow("X2", () =>
        {
            CraftManager.Instance.MnX = 2;
            tickX2 = 180f;
            ActiveX2 = true;
        });
    }

    public void ButtonBonus3()
    {
        if (ActiveX2)
        {
            return;
        }
        YG2.RewardedAdvShow("X3", () =>
        {
            CraftManager.Instance.MnX = 3;
            tickX3 = 120f;
            ActiveX3 = true;
        });
    }
}
