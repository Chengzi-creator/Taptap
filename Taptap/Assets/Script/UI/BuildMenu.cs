using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    [SerializeField] private GameObject SetupMasks;
    [SerializeField] private GameObject MenuMasks;
    [SerializeField] private Toggle _toggle;
    
    private void Awake()
    {
        MenuMasks.SetActive(false);
        SetupMasks.SetActive(false);
        _toggle.onValueChanged.AddListener(isOn => MenuControl());
    }

    void MenuControl()
    {
        if (_toggle.isOn)
        {
            MenuMasks.SetActive(true);
        }
        else
        {
            MenuMasks.SetActive(false);
        }
    }
    
}
