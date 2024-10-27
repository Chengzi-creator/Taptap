using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnRotation : MonoBehaviour , IShow
{
    public void ShowRange()
    {
        GameObject.Find("ShowRange").SetActive(true);
    }
    public void SetFaceDirection(int faceDirection)
    {
        GameObject.Find("ShowRange").transform.rotation = Quaternion.Euler( 0 , 0 , faceDirection * 90 );
    }
}
