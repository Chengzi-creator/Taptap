using System;
using UnityEngine;

public enum VFXType
{
    Attack_Tuci,
    Attack_FeiBiao,
    Attack_Chuizi,
    Attack_Toushiqi,
    Attack_Lianju,

    Range_Flash,
    Range_Lazor,
    Range_Torch,
    Range_Single,
    Monster_dead,
}

public class VFX
{
    public GameObject vfxObject;
    public VFXType vfxType;
    public VFX(VFXType vfxType, GameObject gameObject)
    {
        this.vfxObject = gameObject;
        this.vfxType = vfxType;
    }

    public GameObject Init()
    {
        vfxObject.SetActive(true);
        return vfxObject;
    }

    public void SetColor(Color color)
    {
        switch (vfxType)
        {
            case VFXType.Attack_Tuci:
                var mainModule = vfxObject.GetComponent<ParticleSystem>().main;
                mainModule.startColor = color;
                break;
            case VFXType.Attack_FeiBiao:
                vfxObject.GetComponent<LineRenderer>().startColor = color;
                vfxObject.GetComponent<LineRenderer>().endColor = color;
                break;
            case VFXType.Range_Flash:
            case VFXType.Range_Lazor:
            case VFXType.Range_Torch:
            case VFXType.Range_Single:
            case VFXType.Attack_Toushiqi:
            case VFXType.Attack_Chuizi:
            case VFXType.Attack_Lianju:
            case VFXType.Monster_dead:
                mainModule = vfxObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                mainModule.startColor = color;
                break;
        }
    }

    public void Reduce()
    {
        vfxObject.SetActive(false);
    }

    internal void SetLifeTime(float lifeTime)
    {
        switch (vfxType)
        {
            case VFXType.Attack_Lianju:
                var mainModule = vfxObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                mainModule.startLifetime = lifeTime;
                break;
        }
    }
}
