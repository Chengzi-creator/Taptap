using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSpike : IShow
{
    public override void ShowRange()
    {
        GameObject.Find("ShowRange").SetActive(true);
    }
    public override void SetFaceDirection(int faceDirection)
    {
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
