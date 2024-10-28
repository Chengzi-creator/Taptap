using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTest : MonoBehaviour
{
    private static int hp;
    public static int HP
    {
        get => hp;
        set
        {
            hp = value;
            MyGridManager.Instance.ChangeHomeHP(hp);
        }
    }
    private void Start()
    {
        MyGridManager.Instance.LoadLevel(0);
        HP = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ColorBlockManager.Instance.OnUpdate(Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var pos = MyGridManager.Instance.GetMapPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log("pos:" + Camera.main.ScreenToWorldPoint(Input.mousePosition) + " " + MyGridManager.Instance.GetGridMidWorldPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), out bool valid) + " " + valid);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Grid")
                {
                    MyGrid myGrid = hit.collider.gameObject.GetComponent<MyGrid>();
                    myGrid.OnClick();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            MyGridManager.Instance.CalculatePath();
            MyGridManager.Instance.DrawPath();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            MyGridManager.Instance.ErasePath();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            VFXManager.Instance.CreateVFX_Attack_Tuci(new Vector2Int(1, 0), 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            VFXManager.Instance.CreateVFX_Attack_FeiBiao(new Vector2Int(0, 0), new Vector2Int(1, 1), 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            VFXManager.Instance.CreateVFX_Range_Lazor(new Vector2Int(0, 0), 1, 5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            VFXManager.Instance.CreateVFX_Range_Flash(new Vector2Int(0, 0), 1, 6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            VFXManager.Instance.CreateVFX_Range_Torch(new Vector2Int(0, 0), 7);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            VFXManager.Instance.CreateVFX_Range_Single(new Vector2Int(0, 0), 6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ColorBlockManager.Instance.CreateColorBlock(new Vector2Int(0, 0), 2, 6);
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            VFXManager.Instance.CreateVFX_Attack_Chuizi(new Vector2Int(0, 0), 6);
        }
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            VFXManager.Instance.CreateVFX_Attack_Toushiqi(new Vector2Int(0, 0), 5);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    VFXManager.Instance.CreateVFX_Attack_Lianju(new Vector2Int(0, 6), 1, 6);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    VFXManager.Instance.CreateVFX_Monster_Dead(new Vector2(0.5f, 0), 5);
        //    MyGridManager.Instance.CalculateAllGridCanPutTower();
        //}
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            VFXManager.Instance.CreateVFX_Attack_Tower_Self(new Vector2Int(0, 0), 5);
        }
    }
}
