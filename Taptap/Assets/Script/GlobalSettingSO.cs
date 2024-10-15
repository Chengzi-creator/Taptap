using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettingSO", menuName = "ScriptableObjects/GlobalSettingSO")]
public class GlobalSettingSO : ScriptableObject
{
    [SerializeField]
    private float refreshTime;
    public float RefreshTime => refreshTime;

    [Header("黑红绿黄蓝紫青白")]
    [SerializeField]
    private List<float> colorRemainTime = new List<float>();
    public float GetColorRemainTime(int index) => colorRemainTime[index];
    [SerializeField]
    private List<float> buffValue = new List<float>();
    public float GetBuffValue(int index) => buffValue[index];
}
