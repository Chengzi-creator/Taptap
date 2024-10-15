using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Wave :MonoBehaviour
{
    public int waveCount;
    [SerializeField] public int enemyCount;
    [SerializeField] public int sourceCount;
    [SerializeField] public float timeBuild;
    [SerializeField] public float timeEnemy;
    [SerializeField] public GameObject _enemy;
}
