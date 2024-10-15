using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSetting
{
    private static GlobalSetting instance;
    public static GlobalSetting Instance
    {
        get{
            if(instance == null)
            {
                instance = new GlobalSetting();
                instance.GlobalSettingSO = Resources.Load<GlobalSettingSO>("SO/GlobalSettingSO");
            }
            return instance;
        }
    }
    public GlobalSettingSO GlobalSettingSO;
}
