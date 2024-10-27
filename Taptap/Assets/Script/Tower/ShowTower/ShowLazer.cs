using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTorch : IShow
{
    public override void ShowRange()
    {
        GameObject.Find("ShowRange").SetActive(true);
    }
    public override void SetFaceDirection(int faceDirection)
    {
    }
}
