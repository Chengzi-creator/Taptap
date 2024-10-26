using System.Collections.Generic;
using UnityEngine;

public class ColorBlockManager : MonoBehaviour, IColorBlockManager
{
    public GameObject colorBlockPrefab;
    /// <summary>
    /// 目标点，世界坐标
    /// </summary>
    public Vector2 target;

    private List<ColorBlock> colorBlockPool = new List<ColorBlock>();
    private List<ColorBlock> colorBlockActive = new List<ColorBlock>();

    private static ColorBlockManager instance;
    public static ColorBlockManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load<GameObject>("Prefab/ColorBlockManager")).GetComponent<ColorBlockManager>();
            }
            return instance;
        }
    }

    public void SetTarget(Vector2 target)
    {
        //Debug.Log($"target :{target}");
        this.target = target;
    }

    public void CreateColorBlock(Vector2Int mapPos, int count, int color = 7)
    {
        Vector2 worldPos = MyGridManager.Instance.GetWorldPos(mapPos);
        for (int i = 0; i < count; i++)
        {
            CreateColorBlock(worldPos, color);
        }

    }

    private void CreateColorBlock(Vector2 worldPos, int color)
    {
        ColorBlock colorBlock = Get();
        colorBlock.gameObject.SetActive(true);
        Vector2 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        colorBlock.transform.position = worldPos + randomOffset;
        colorBlock.ReInit(target, color);
        colorBlockActive.Add(colorBlock);
    }

    private ColorBlock Get()
    {
        ColorBlock colorBlock;
        if (colorBlockPool.Count == 0)
        {
            colorBlock = Instantiate(colorBlockPrefab, gameObject.transform).GetComponent<ColorBlock>();
        }
        else
        {
            colorBlock = colorBlockPool[0];
            colorBlockPool.RemoveAt(0);
        }
        return colorBlock;
    }

    public void Reduce(ColorBlock colorBlock)
    {
        colorBlockActive.Remove(colorBlock);
        colorBlockPool.Add(colorBlock);
        colorBlock.gameObject.SetActive(false);
    }

    public void OnUpdate(float deltaTime)
    {
        for (int i = 0; i < colorBlockActive.Count; i++)
        {
            colorBlockActive[i].OnUpdate(deltaTime);
        }
    }
}
