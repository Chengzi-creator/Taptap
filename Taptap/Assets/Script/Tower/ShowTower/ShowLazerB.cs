using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLazerB : IShow
{
    public override void ShowRange()
    {
        GameObject.Find("ShowRange").SetActive(true);
    }
    public override void SetFaceDirection(int faceDirection)
    {
        switch(faceDirection)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/bR");
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/bU");
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/bL");
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/bD");
                break;
        }
        GameObject.Find("ShowRange").transform.rotation = Quaternion.Euler(0, 0, faceDirection * 90);
    }
}
