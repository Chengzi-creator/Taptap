using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTorch : MonoBehaviour , IShow
{
    public void ShowRange()
    {
        GameObject.Find("ShowRange").SetActive(true);
    }
    public void SetFaceDirection(int faceDirection)
    {
    }
}
