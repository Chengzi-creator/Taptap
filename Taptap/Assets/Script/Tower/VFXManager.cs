using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager
{
    public static VFXManager instance;
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
    private LinkedList<ParticleSystem> vfxList;
    GameObject prefab_VFX_Attack_Tuci;
    private static void Init()
    {
        instance.vfxList = new LinkedList<ParticleSystem>();
        instance.prefab_VFX_Attack_Tuci = Resources.Load<GameObject>("Prefab/VFX/VFX_Attack_Tuci");
    }

    public void CreateVFX_Attack_Tuci(Vector2Int position , int faceDirection)
    {
        var node = vfxList.First;
        while(node != null)
        {
            if(!node.Value.isPlaying)
            {
                GameObject.Destroy(node.Value.gameObject);
                vfxList.Remove(node);
                node = vfxList.First;
            }
            else
            {
                break;
            }
        }
        var vfx = GameObject.Instantiate(prefab_VFX_Attack_Tuci).GetComponent<ParticleSystem>();
        vfx.transform.position = MyGridManager.Instance.GetWorldPos(position);
        // MyGridManager.Instance.GetWorldPos(position);
        vfx.transform.eulerAngles = new Vector3(0 , 0 , faceDirection * 90);
    }
}
