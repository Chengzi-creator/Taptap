using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public Vector2 MapPos { get; private set; }
    /// <summary>
    /// ��������
    /// </summary>
    public Vector2 WorldPos { get; private set; }

    /// <summary>
    /// �ڸø����ϵ�����(�������ϰ����㣬�յ�),���û����Ϊ��
    /// </summary>
    public GridObject HoldObject;
    /// <summary>
    /// ��
    /// </summary>
    //public MyLight light;
    /// <summary>
    /// ���Ӵ�С
    /// </summary>
    public static Vector2 GridSize = new Vector2(2, 2);

    /// <summary>
    /// ��ͨ������
    /// </summary>
    public bool CanPass
    {
        get => HoldObject == null || HoldObject.Type != GridObjectType.Obstacle;
    }

    /// <summary>
    /// ���յ����
    /// </summary>
    public bool IsEndGrid
    {
        get => HoldObject != null && HoldObject.Type == GridObjectType.End;
    }
    /// <summary>
    /// ��ʼ�����
    /// </summary>
    /// <param name="mapPos"></param>
    /// <param name="worldPos"></param>
    /// <param name="holdObject"></param>
    public void Init(Vector2 mapPos, Vector2 worldPos, GridObject holdObject = null)
    {
        MapPos = mapPos;
        WorldPos = worldPos;
        if (holdObject != null)
            SetHoldObject(holdObject);
    }
    /// <summary>
    /// ���ø����ϵ�����
    /// </summary>
    /// <param name="gridObject"></param>
    public void SetHoldObject(GridObject gridObject)
    {
        HoldObject = gridObject;
        HoldObject.transform.position = new Vector3(WorldPos.x, WorldPos.y, 0);
    }
    /// <summary>
    /// �������
    /// </summary>
    public void OnClick()
    {
        if (HoldObject != null)
        {
            HoldObject.OnClick();
        }
        else
        {
            Debug.Log("no object on this grid");
        }
    }


    /// <summary>
    /// ��ʾ�Ƿ����
    /// </summary>
    public void ShowEmpty()
    {
        if (HoldObject)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    /// <summary>
    /// ȡ����ʾ�Ƿ����
    /// </summary>
    public void CancelShowEmpty()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }



}
