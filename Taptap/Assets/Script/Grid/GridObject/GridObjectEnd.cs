
using UnityEngine;

public class GridObjectEnd : GridObject
{
    private GameObject grid;
    private SpriteRenderer sr;

    public Color[] colorList =
    {
        new Color(0,0,0,255),
        new Color(155,155,155,255),
        new Color(200,200,200,255),
        new Color(255,255,255,255)
    };

    public GridObjectEnd(GridObjectType type, GameObject gameObject) : base(type)
    {
        grid = gameObject;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
    public override void OnClick()
    {
        Debug.Log("End");
    }

    private void SetColor(int colorIdx)
    {
        sr.enabled = true;
        sr.color = colorList[colorIdx] / 255f;
        Debug.Log("Color Change：" + sr.color + " " + colorList[colorIdx]);
    }

    public void HPChange(int hp)
    {
        if(hp <= 100)
        {
            SetColor(0);
        }
        else if (hp <= 200)
        {
            SetColor(1);
        }
        else if (hp <= 300)
        {
            SetColor(2);
        }
        else
        {
            SetColor(3);
        }
    }
}


