using UnityEngine;

public class ColorBlock : MonoBehaviour
{
    public float Speed;
    private Vector2 target;
    private bool isActive = false;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public void ReInit(Vector2 target,int color)
    {
        this.target = target;
        isActive = true;
        spriteRenderer.color = ColorBlockManager.Instance.GetColor(color);
    }


    public void OnUpdate(float deltaTime)
    {
        if (!isActive)
        {
            return;
        }
        Vector2 dir = target - (Vector2)transform.position;
        if (dir.magnitude < Speed * deltaTime)
        {
            transform.position = target;
            isActive = false;
            ArriveTarget();
        }
        else
        {
            transform.position += (Vector3)dir.normalized * Speed * deltaTime;
        }
    }
    private void ArriveTarget()
    {
        ColorBlockManager.Instance.Reduce(this);
        InputTest.HP += 10;
    }
}
