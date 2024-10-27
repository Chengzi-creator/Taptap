
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IShow : MonoBehaviour
{
    public virtual void ShowRange()
    {
        transform.Find("ShowRange").gameObject.SetActive(true);
    }
    public virtual void SetFaceDirection(int faceDirection)
    { }
}