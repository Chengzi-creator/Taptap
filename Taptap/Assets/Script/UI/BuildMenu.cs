using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [Serializable]
    public struct  ToggeleViewPair//直接将按钮和view绑定
    {
        public Toggle toggle;
        public GameObject view;
    }
    
    [SerializeField] public List<ToggeleViewPair> ToggeleViewPairs;//可以直接添加
    
    private void Awake()
    {
       foreach (var pair in ToggeleViewPairs)
       {
           pair.view.SetActive(false);
       }
    }
    
    private void Update()
    {
        //Click();
        //UIManager.Instance?.CheckToggleImages();
    }
    
 
    private void Click()
    {
        foreach (var pair in ToggeleViewPairs)
        {
            if (pair.toggle.isOn)
            {
                pair.view.SetActive(true);
                break;
            }
            else
            {
                pair.view.SetActive(false);
            }
        }
    }
    
}
