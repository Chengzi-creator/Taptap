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
    GameObject prefab_VFX_Attack_Chuizi;
    GameObject prefab_VFX_Attack_Toushiqi;
    GameObject prefab_VFX_Attack_Lianju;

    GameObject prefab_VFX_Range_Flash;
    GameObject prefab_VFX_Range_Lazor;
    GameObject prefab_VFX_Range_Torch;
    GameObject prefab_VFX_Range_Single;

    GameObject prefab_VFX_Monster_dead;


    GameObject prefab_VFX_Attack_Tower_Self;
    private static void Init()
    {
        // instance.vfxList = new LinkedList<IVFX>();
        instance.prefab_VFX_Attack_Tuci = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Attack_Ziguang_main");
        instance.prefab_VFX_Attack_FeiBiao = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Attack_FeiBiao");
        instance.prefab_VFX_Attack_Chuizi = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Attack_Chuizi");
        instance.prefab_VFX_Attack_Toushiqi = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Attack_Toushiqi");
        instance.prefab_VFX_Attack_Lianju = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Attack_Lianju");

        instance.prefab_VFX_Range_Torch = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Range_Torch");
        instance.prefab_VFX_Range_Flash = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Range_Flash");
        instance.prefab_VFX_Range_Lazor = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Range_Lazor");
        instance.prefab_VFX_Range_Single = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Range_Single");

        instance.prefab_VFX_Monster_dead = Resources.Load<GameObject>("Prefab/UseVFX/VFX_Monster_dead");

        instance.prefab_VFX_Attack_Tower_Self = Resources.Load<GameObject>("Prefab/UseVFX/VFX_attack_Tower_self");
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
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.1f);
    }

    public void CreateVFX_Attack_Chuizi(Vector2Int position, int color = 7)
    {
        var vfx = Get(VFXType.Attack_Chuizi, prefab_VFX_Attack_Chuizi);
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position) + new Vector2(0, 4);
        vfx.SetColor(GetColor(color));
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.5f);
    }

    public void CreateVFX_Attack_Toushiqi(Vector2Int position, int color = 7)
    {
        var vfx = Get(VFXType.Attack_Toushiqi, prefab_VFX_Attack_Toushiqi);
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.SetColor(GetColor(color));
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.5f);
    }

    public void CreateVFX_Attack_Lianju(Vector2Int position, int faceDirection, int color = 7, bool firstInvoke = true)
    {
        var vfx = Get(VFXType.Attack_Lianju, prefab_VFX_Attack_Lianju);
        int attackMapDistance = GetDistance(position, faceDirection);
        float dis = MyGridManager.Instance.GetWorldDistance(attackMapDistance);
        if (firstInvoke)
            vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        else
        {
            vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position)
                - GetFaceDirVector(faceDirection);
        }
        vfx.vfxObject.transform.eulerAngles = new Vector3(0, 0, faceDirection * 90);
        vfx.SetColor(GetColor(color));
        vfx.SetLifeTime((dis + 1) / 2f);
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, (dis + 1) / 2);
        if (firstInvoke)
        {
            Vector2Int endPos = GetFaceDirVector(faceDirection) * attackMapDistance;
            DelayToInvoke.Instance.StartDelayToInvokeDo(CreateVFX_Attack_Lianju,
            position + endPos, (faceDirection + 2) % 4, color, (dis + 1) / 2, false);
        }
    }

    private int GetDistance(Vector2Int position, int faceDir)
    {
        switch (faceDir)
        {
            case 0:
                return MyGridManager.Instance.width - position.x - 1;
            case 1:
                return MyGridManager.Instance.length - position.y - 1;
            case 2:
                return position.x;
            case 3:
                return position.y;
        }
        return 0;
    }

    private Vector2Int GetFaceDirVector(int faceDir)
    {
        switch (faceDir)
        {
            case 0:
                return Vector2Int.right;
            case 1:
                return Vector2Int.up;
            case 2:
                return Vector2Int.left;
            case 3:
                return Vector2Int.down;
        }
        return Vector2Int.zero;
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
        if (color == 0)
        {
            return null;
        }
        var vfx = Get(VFXType.Range_Single, prefab_VFX_Range_Single);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        return vfx;
    }


    public void CreateVFX_Monster_Dead(Vector2 position, int color = 7)
    {
        var vfx = Get(VFXType.Monster_dead, prefab_VFX_Monster_dead);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 0.5f);
    }

    public void CreateVFX_Attack_Tower_Self(Vector2Int position, int color = 7)
    {
        var vfx = Get(VFXType.Attack_Tower_Self, prefab_VFX_Attack_Tower_Self);
        vfx.SetColor(GetColor(color));
        vfx.vfxObject.transform.position = MyGridManager.Instance.GetWorldPos(position);
        DelayToInvoke.Instance.StartDelayToInvokeDo(Reduce, vfx, 3f);
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
