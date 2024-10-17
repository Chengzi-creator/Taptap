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
        var vfx = GameObject.Instantiate(prefab_VFX_Attack_Tuci);
        vfx.transform.position = MyGridManager.Instance.GetWorldPos(position);
        vfx.transform.eulerAngles = new Vector3(0, 0, faceDirection * 90);
    }

    public void CreateVFX_Attack_FeiBiao(Vector2Int startPos, Vector2 endPos)
    {
        var vfx = GameObject.Instantiate(prefab_VFX_Attack_FeiBiao);
        vfx.transform.position = MyGridManager.Instance.GetWorldPos(startPos);
        int faceDir = 0;
        if(endPos.y == startPos.y&& startPos.x < endPos.x)
        {
            faceDir = 0;
        }
        else if(startPos.x == endPos.x && endPos.y > startPos.y)
        {
            faceDir = 1;
        }
        if(startPos.y == endPos.y && endPos.x < startPos.x)
        {
            faceDir = 2;
        }
        if (startPos.x == endPos.x && endPos.y < startPos.y)
        {
            faceDir = 3;
        }
        vfx.transform.eulerAngles = new Vector3(0, 0, faceDir * 90);
        vfx.transform.localScale = new Vector3(Vector2.Distance(startPos, endPos) / 4, 1, 1);
    }
}
