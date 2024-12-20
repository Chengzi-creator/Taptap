using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLazerG : IShow
{
    public override void ShowRange()
    {
        transform.Find("ShowRange").gameObject.SetActive(true);
    }
    public override void SetFaceDirection(int faceDirection)
    {
        switch(faceDirection)
        {
            case 0:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/gR");
                break;
            case 1:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/gU");
                break;
            case 2:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/gL");
                break;
            case 3:
                this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/BuffTower/gD");
                break;
        }
        GameObject.Find("ShowRange").transform.rotation = Quaternion.Euler(0, 0, faceDirection * 90);
    }
}
