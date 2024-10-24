using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager
{
    private static VFXManager instance;
    public static VFXManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new VFXManager();
                Init();
            }
            return instance;
        }
    }


    private Dictionary<VFXType, List<VFX>> m_VFXDic = new Dictionary<VFXType, List<VFX>>();

    // GameObject
    // private LinkedList<IVFX> vfxList;
    GameObject prefab_VFX_Attack_Tuci;
    GameObject prefab_VFX_Attack_FeiBiao;
    GameObject prefab_VFX_Range_Flash;
    GameObject prefab_VFX_Range_Lazor;
    GameObject prefab_VFX_Range_Torch;
    GameObject prefab_VFX_Range_Single;
    private static void Init()
    {
        // instance.vfxList = new LinkedList<IVFX>();
        instance.prefab_VFX_Attack_Tuci = Resources.Load<GameObject>("Prefab/VFX/VFX_Attack_Ziguang_main");
        instance.prefab_VFX_Attack_FeiBiao = Resources.Load<GameObject>("Prefab/VFX/VFX_Attack_FeiBiao");
        instance.prefab_VFX_Range_Torch = Resources.Load<GameObject>("Prefab/VFX/VFX_Range_Torch");
        instance.prefab_VFX_Range_Flash = Resources.Load<GameObject>("Prefab/VFX/VFX_Range_Flash");
        instance.prefab_VFX_Range_Lazor = Resources.Load<GameObject>("Prefab/VFX/VFX_Range_Lazor");
        instance.prefab_VFX_Range_Single = Resources.Load<GameObject>("Prefab/VFX/VFX_Range_Single");
    }

    private VFX Get(VFXType vFXType, GameObject prefab)
    {
        VFX vfx;
        if (m_VFXDic.ContainsKey(vFXType) == false || m_VFXDic[vFXType].Count == 0)
        {
            vfx = new VFX(vFXType, GameObject.Instantiate(prefab));
        }
        else
        {
            vfx = m_VFXDic[vFXType][0];
            m_VFXDic[vFXType].RemoveAt(0);
        }
        vfx.Init();
        return vfx;
    }

    private void Reduce(VFX vfx)
    {
        vfx.Reduce();
        if (m_VFXDic.ContainsKey(vfx.vfxType) == false)
        {
            m_VFXDic.Add(vfx.vfxType, new List<VFX>());
        }
        m_VFXDic[vfx.vfxType].Add(vfx);
    }

    public void CreateVFX_Attack_Tuci(Vector2Int position, int faceDirection, int color = 7)
    {
        //Debug.Log("pos:" + position + " faceDirection:" + faceDirection);
        var vfx = Get(VFXType.Attack_Tuci, prefab_VFX_Attack_Tuci);
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.vfxObject.transform.eulerAngles = new Vector3(0, 0, faceDirection * 90);
        vfx.SetColor(GetColor(color));
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.5f);
    }

    public void CreateVFX_Attack_FeiBiao(Vector2Int startPos, Vector2 endPos, int color = 7)
    {
        var vfx = Get(VFXType.Attack_FeiBiao, prefab_VFX_Attack_FeiBiao);
        Vector3 startWorldPos = MyGridManager.Instance.GetWorldPos(startPos);
        Vector3 endWorldPos = MyGridManager.Instance.GetWorldPos(endPos);
        LineRenderer lineRenderer = vfx.vfxObject.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startWorldPos);
        lineRenderer.SetPosition(1, endWorldPos);
        vfx.SetColor(GetColor(color));
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.5f);
    }

    public VFX CreateVFX_Range_Flash(Vector2Int position, int faceDir, int color = 7)
    {
        VFX vfx = Get(VFXType.Range_Flash, prefab_VFX_Range_Flash);
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.vfxObject.transform.eulerAngles = new Vector3(0, 0, faceDir * 90);
        vfx.SetColor(GetColor(color));
        return vfx;
    }

    public VFX CreateVFX_Range_Lazor(Vector2Int position, int faceDir, int color = 7)
    {
        var vfx = Get(VFXType.Range_Lazor, prefab_VFX_Range_Lazor);
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.eulerAngles = new Vector3(0, 0, faceDir * 90);
        return vfx;
    }

    public VFX CreateVFX_Range_Torch(Vector2Int position, int color = 7)
    {
        var vfx = Get(VFXType.Range_Torch, prefab_VFX_Range_Torch);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        return vfx;
    }

    public VFX CreateVFX_Range_Single(Vector2Int position, int color = 7)
    {
        var vfx = Get(VFXType.Range_Single, prefab_VFX_Range_Single);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        return vfx;
    }

    private Color GetColor(int colorIdx)
    {
        switch (colorIdx)
        {
            case 0:
                return Color.black;
            case 1:
                return Color.red;
            case 2:
                return Color.green;
            case 3:
                return Color.yellow;
            case 4:
                return Color.blue;
            case 5:
                return Color.magenta;
            case 6:
                return Color.cyan;
            case 7:
                return Color.white;
        }
        return Color.white;
    }


    public void ReduceVFX(VFX vfx)
    {
        Reduce(vfx);
    }
}
