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

    // GameObject
    // private LinkedList<IVFX> vfxList;
    GameObject prefab_VFX_Attack_Tuci;
    GameObject prefab_VFX_Attack_FeiBiao;
    private static void Init()
    {
        // instance.vfxList = new LinkedList<IVFX>();
        instance.prefab_VFX_Attack_Tuci = Resources.Load<GameObject>("Prefab/VFX/VFX_Attack_Ziguang_main");
        instance.prefab_VFX_Attack_FeiBiao = Resources.Load<GameObject>("Prefab/VFX/VFX_Attack_FeiBiao");
    }



    public void CreateVFX_Attack_Tuci(Vector2Int position, int faceDirection)
    {
        //Debug.Log("pos:" + position + " faceDirection:" + faceDirection);
        var vfx = GameObject.Instantiate(prefab_VFX_Attack_Tuci);
        vfx.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.transform.eulerAngles = new Vector3(0, 0, faceDirection * 90);
    }

    public void CreateVFX_Attack_FeiBiao(Vector2Int startPos, Vector2 endPos)
    {
        var vfx = GameObject.Instantiate(prefab_VFX_Attack_FeiBiao);
        Vector3 startWorldPos = MyGridManager.Instance.GetWorldPos(startPos);
        Vector3 endWorldPos = MyGridManager.Instance.GetWorldPos(endPos);
        LineRenderer lineRenderer = vfx.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startWorldPos);
        lineRenderer.SetPosition(1, endWorldPos);
    }
}
