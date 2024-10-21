using UnityEngine;

public enum VFXType
{
    Attack_Tuci,
    Attack_FeiBiao,
    Range_Flash,
    Range_Lazor,
    Range_Torch
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
                mainModule = vfxObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                mainModule.startColor = color;
                break;
            case VFXType.Range_Lazor:
                mainModule = vfxObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                mainModule.startColor = color;
                break;
            case VFXType.Range_Torch:
                mainModule = vfxObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                mainModule.startColor = color;
                break;
        }
    }

    public void Reduce()
    {
        vfxObject.SetActive(false);
    }
}
