using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SourceText : MonoBehaviour,ISource
{
   [SerializeField] private TextMeshProUGUI _text;
   public float Count = 100;
   
   private void Awake()
   {
      _text.text = "Icon : " + Count.ToString();
   }

   private void Start()
   {
      
   }

   private void Update()
   {
      
   }

   public void IconIncrease(float increase)
   {
      Count += increase;
      _text.text = "Icon : " + Count.ToString();
   }

   public void IconDecrease(float decrease)
   {
      Count -= decrease;
      _text.text = "Icon : " + Count.ToString();
   }
}
