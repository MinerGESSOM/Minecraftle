using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;


public class StartManager : MonoBehaviour
{
    [SerializeField] CanvasScaler canvas;
    [SerializeField] Transform obj1;
    [SerializeField] Transform obj2;
    [SerializeField] Transform obj3;
    [SerializeField] Transform obj1local;
    [SerializeField] Transform obj2local;
    [SerializeField] Transform obj3local;

    void Start()
    {
        if (YG2.envir.isMobile)
        {
            canvas.referenceResolution = new Vector2(550, 500);
            obj1.parent = obj1local;
            obj2.parent = obj2local;
            obj3.parent = obj3local;
            obj1.localPosition = Vector3.zero;
            obj2.localPosition = Vector3.zero;
            obj3.localPosition = Vector3.zero;
        }
        
    }
}
