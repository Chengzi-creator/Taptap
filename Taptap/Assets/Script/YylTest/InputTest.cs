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
    }
}
