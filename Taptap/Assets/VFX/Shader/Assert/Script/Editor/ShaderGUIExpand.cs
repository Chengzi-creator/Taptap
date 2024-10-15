using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace BRK.GUIExpand
{
    public class ShaderGUIExpand : ShaderGUI
    {
        public enum UVClamp
        {
            none,X,Y,XY,
        }
        public enum Custom
        {
            Custom1,
            Custom2
        }
        public enum Channel
        {
            X,Y,Z,W,None
        }
        public static void SelectM(Material mObj)
        {
            if (GUILayout.Button("选中材质"))
            {
                if (mObj == null) return;
                UnityEditor.Selection.activeObject = mObj;
            }
        }
        public static void ShaderPropertyVector2(ref Vector2 v2,string name,ref MaterialProperty uSpeed, ref MaterialProperty vSpeed)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(14);
                GUILayout.Label(name);
                GUILayout.Space(68);
                EditorGUI.BeginChangeCheck();
                v2 = EditorGUILayout.Vector2Field("", v2, new[] {GUILayout.ExpandWidth(true), GUILayout.MinWidth(0) });
                if (EditorGUI.EndChangeCheck())
                {
                    uSpeed.floatValue = v2.x;
                    vSpeed.floatValue = v2.y;
                }
                v2.x = uSpeed.floatValue;
                v2.y = vSpeed.floatValue;

            }
            
            
        }
        public static void ShaderPropertyVector2(ref Vector2 UVSpeed, ref MaterialProperty uSpeed, ref MaterialProperty vSpeed)
        {
            ShaderPropertyVector2(ref UVSpeed, "UV流动", ref uSpeed, ref vSpeed);
        }
        public static void ShaderPropertyCustom(ref Custom custom, ref Channel channel, string name, ref MaterialProperty x, ref MaterialProperty y)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Space(14);
                GUILayout.Label(name);
                GUILayout.Space(26);
                EditorGUI.BeginChangeCheck();
                custom = (Custom)EditorGUILayout.EnumPopup( custom, "MiniPopup", new[] { GUILayout.MaxWidth(180) }) ;
                channel = (Channel)EditorGUILayout.EnumPopup(channel, "MiniPopup", new[] { GUILayout.MaxWidth(80) });
                if (EditorGUI.EndChangeCheck())
                {
                    switch (custom)
                    {
                        case Custom.Custom1:
                            x.floatValue = 0;
                            break;
                        case Custom.Custom2:
                            x.floatValue = 1;
                            break;
                    }
                    switch (channel)
                    {
                        case Channel.X:
                            y.floatValue = 0;

                            break;
                        case Channel.Y:
                            y.floatValue = 1;

                            break;
                        case Channel.Z:
                            y.floatValue = 2;
                            break;
                        case Channel.W:
                            y.floatValue = 3;
                            break;
                        case Channel.None:
                            y.floatValue = 4;
                            break;
                    }
                }
                custom = (Custom)x.floatValue;
                channel = (Channel)y.floatValue;
            }

        }
    }

}
