using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUnRotation : IShow
{
    public override void SetFaceDirection(int faceDirection)
    {
        GameObject.Find("ShowRange").transform.rotation = Quaternion.Euler( 0 , 0 , faceDirection * 90 );
    }
}
