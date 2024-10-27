using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLazerR : IShow
{
    public override void SetFaceDirection(int faceDirection)
    {
        switch(faceDirection)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rR");
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rU");
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rL");
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/rD");
                break;
        }
        GameObject.Find("ShowRange").transform.rotation = Quaternion.Euler(0, 0, faceDirection * 90);
    }
}
