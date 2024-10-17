using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_1 : MonoBehaviour , IVFX
{
    ParticleSystem particleSystem;
    public bool IsPlaying => particleSystem.isPlaying;
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if(IsPlaying == false)
        {
            Destroy(gameObject);
        }
    }
    // public void Init(Vector2 position)
    // {
    //     transform.position = MyGridManager.Instance.GetWorldPos(position);
    // }
    // public void ReInit()
    // {}
    // public void Remove()
    // {}
}
