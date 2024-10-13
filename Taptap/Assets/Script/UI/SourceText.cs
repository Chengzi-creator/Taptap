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
      StartCoroutine(SourceChange(_lastTime));
   }

   IEnumerator SourceChange(float time)
   {
      yield return new WaitForSeconds(time);
      Count++;
      TextChange();
   }

   void TextChange()
   {
      _text.text = "Icon : " + Count.ToString();
   }
}
