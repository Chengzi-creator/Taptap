using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SourceText : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _text;
   private int Count = 100;
   private float _lastTime = 10f;
   
   private void Awake()
   {
      _text.text = "Icon : " + Count.ToString();
   }

   private void Start()
   {
      
   }

   public void IconChange(int increase)
   {
      Count += increase;
      _text.text = "Icon : " + Count.ToString();
   }
}
