using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFlash : IShow
{
    public override void ShowRange()
    {
        base.ShowRange();
    }
    public override void SetFaceDirection(int faceDirection)
    {
        Debug.Log("setFaceDirection");
        switch (faceDirection)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }
}
