using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTest : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var pos = MyGridManager.Instance.GetMapPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log("pos:" + pos);
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

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            VFXManager.Instance.CreateVFX_Attack_Tuci(new Vector2Int(0, 0), 0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            VFXManager.Instance.CreateVFX_Attack_FeiBiao(new Vector2Int(0, 0), new Vector2Int(1, 0));
        }
    }
}
