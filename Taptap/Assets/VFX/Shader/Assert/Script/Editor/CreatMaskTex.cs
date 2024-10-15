using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
namespace BRK.GUIExpand
{
    /// <summary>
    /// mask贴图生成拓展，有问题私聊BRK小布：320370252
    /// </summary>
    public class CreatMaskTex : Editor
    {
        public enum RampTexType
        { 
            Line,Line2,Color
        }
        private static int width = 512, height = 512;
        //private static Gradient gradient = new Gradient();
        private static bool rotLine2 = false;
        private static int rotLine1 = 0;
        private static MaterialPropertise CreateTex2D(MaterialPropertise materialPropertise)
        {
            Texture2D t = new Texture2D(width, height);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    t.SetPixel(w, h, Color.white);
                }
            }
            t.Apply();
            byte[] bytes;
            bytes = t.EncodeToPNG();
            string tName = "Ramp";
            string mPath = AssetDatabase.GetAssetPath(materialPropertise.material);
            if (mPath.Equals(""))
            {
                mPath = Application.dataPath;
            }
            string path = EditorUtility.SaveFilePanel("创建新的贴图", mPath, tName,"png");
            string pathName = path.Substring(path.IndexOf("Assets"));
            File.WriteAllBytes(pathName, bytes);
            AssetDatabase.Refresh();
            materialPropertise.t = t;
            materialPropertise.path = pathName;
            materialPropertise.gradient = new Gradient();
            return materialPropertise;

        }
        public struct MaterialPropertise
        {
            public Material material;
            public Texture2D t;
            public string TexName;
            public string path;
            public Color color ;
            public Gradient gradient;

            public MaterialPropertise(string texName, Color color, Texture2D t = null, Material material = null, string path = "", Gradient gradient = null)
            {
                this.material = material;
                this.t = t;
                this.TexName = texName;
                this.path = path;
                this.color = color;
                this.gradient = gradient;
            }
        }
        public static void SaveTex( MaterialPropertise materialP)
        {
            byte[] bytes;
            bytes = materialP.t.EncodeToPNG();
            File.WriteAllBytes(materialP.path, bytes);
            materialP.material.SetTexture(materialP.TexName, AssetDatabase.LoadAssetAtPath(materialP.path, typeof(Texture2D)) as Texture2D);
            AssetDatabase.Refresh();
        }
        private static void BoxColor(CreatMaskTex.MaterialPropertise mainPropertise)
        {
        
            EditorGUI.BeginChangeCheck();
            mainPropertise.color = EditorGUILayout.ColorField(mainPropertise.color);
            if (EditorGUI.EndChangeCheck() && mainPropertise.t != null)
            {
                for (int w = 0; w < mainPropertise.t.width; w++)
                {
                    for (int h = 0; h < mainPropertise.t.height; h++)
                    {
                        mainPropertise.t.SetPixel(w, h, mainPropertise.color);
                    }
                }
                mainPropertise.t.Apply();
            }
        }
        private static void LineRamp(CreatMaskTex.MaterialPropertise mainPropertise)
        {

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Rot"))
            {
                rotLine1 = ++rotLine1%4;
                Debug.Log(rotLine1);
            }
            mainPropertise.gradient = EditorGUILayout.GradientField(mainPropertise.gradient);
           
            float time = 0;
            if (EditorGUI.EndChangeCheck() && mainPropertise.t != null)
            {
                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        time = (float)w / (float)width;
                        switch (rotLine1)
                        {
                            case 0:
                                {
                                    Color color = mainPropertise.gradient.Evaluate(time);
                                    mainPropertise.t.SetPixel(w, h, color);
                                    break;
                                }
                            case 1:
                                {
                                    Color color = mainPropertise.gradient.Evaluate(time);
                                    mainPropertise.t.SetPixel(h, w, color);

                                    break;

                                }
                            case 2:
                                {
                                    Color color = mainPropertise.gradient.Evaluate(1-time);
                                    mainPropertise.t.SetPixel(w, h, color);
                                    break;
                                }
                            case 3:
                                {
                                    Color color = mainPropertise.gradient.Evaluate(1-time);
                                    mainPropertise.t.SetPixel(h, w, color);
                                    break;
                                }
                        }
                    }
                }
                mainPropertise.t.Apply();
            }
        }
        private static void Line2Ramp(CreatMaskTex.MaterialPropertise mainPropertise)
        {
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Rot"))
            {
                rotLine2 = !rotLine2;
            }
            mainPropertise.gradient = EditorGUILayout.GradientField(mainPropertise.gradient);
            float time = 0;
            if (EditorGUI.EndChangeCheck() && mainPropertise.t != null)
            {
                int center = mainPropertise.t.width / 2;
                for (int i = 0; i <= center+1; i++)
                {
                    time = (float)i / (float)center;
                    Color color = mainPropertise.gradient.Evaluate(time);
                    for (int x = 0; x <= center; x++)
                    {
                        if (rotLine2)
                        {
                            mainPropertise.t.SetPixel(center + i, center + x, color);
                            mainPropertise.t.SetPixel(center - i, center + x, color);
                            mainPropertise.t.SetPixel(center + i, center - x, color);
                            mainPropertise.t.SetPixel(center - i, center - x, color);
                        }
                        else
                        {
                            mainPropertise.t.SetPixel(center + x, center + i, color);
                            mainPropertise.t.SetPixel(center - x, center + i, color);
                            mainPropertise.t.SetPixel(center + x, center - i, color);
                            mainPropertise.t.SetPixel(center - x, center - i, color);
                        }
                    }
                }
                mainPropertise.t.Apply();
            }
        }
        private static void BoxRamp(CreatMaskTex.MaterialPropertise mainPropertise)
        {
            EditorGUI.BeginChangeCheck();
            mainPropertise.gradient = EditorGUILayout.GradientField(mainPropertise.gradient);
            float time = 0;
            if (EditorGUI.EndChangeCheck() && mainPropertise.t != null)
            {
                int center = mainPropertise.t.width / 2;
                for (int i = 0; i < center; i++)
                {
                    time = (float)i / (float)center;
                    Color color = mainPropertise.gradient.Evaluate(time);
                    for (int x = 0; x < center; x++)
                    {
                            mainPropertise.t.SetPixel(center + i, center + x, color);
                            mainPropertise.t.SetPixel(center - i, center + x, color);
                            mainPropertise.t.SetPixel(center + i, center - x, color);
                            mainPropertise.t.SetPixel(center - i, center - x, color);
                            mainPropertise.t.SetPixel(center + x, center + i, color);
                            mainPropertise.t.SetPixel(center - x, center + i, color);
                            mainPropertise.t.SetPixel(center + x, center - i, color);
                            mainPropertise.t.SetPixel(center - x, center - i, color);
                    }
                }
                mainPropertise.t.Apply();
            }
        }

        public static void RampShaderGUI(ref CreatMaskTex.RampTexType rampTexType, ref CreatMaskTex.MaterialPropertise mainPropertise, ref Material material)
        {
            using (new GUILayout.HorizontalScope("Box"))
            {
                rampTexType = (CreatMaskTex.RampTexType)EditorGUILayout.EnumPopup(rampTexType, new[] { GUILayout.MaxWidth(60), GUILayout .MinHeight(10)});
                if (GUILayout.Button("创建贴图"))
                {
                    mainPropertise = CreatMaskTex.CreateTex2D(mainPropertise);
                    material.SetTexture(mainPropertise.TexName, mainPropertise.t);
                    GUIUtility.ExitGUI();
                }
                if (mainPropertise.t != null&& GUILayout.Button("保存"))
                {
                    CreatMaskTex.MaterialPropertise materialPropertise = new CreatMaskTex.MaterialPropertise();
                    materialPropertise.material = material;
                    materialPropertise.TexName = mainPropertise.TexName;
                    materialPropertise.t = mainPropertise.t;
                    materialPropertise.path = mainPropertise.path;
                    CreatMaskTex.SaveTex(materialPropertise);
                    mainPropertise = new CreatMaskTex.MaterialPropertise(mainPropertise.TexName, mainPropertise.color);
                }
                if (mainPropertise.gradient != null)
                {
                    switch (rampTexType)
                    {
                        case RampTexType.Line:
                            LineRamp(mainPropertise);
                            break;
                        case RampTexType.Line2:
                            Line2Ramp(mainPropertise);
                            break;
                        case RampTexType.Color:
                            BoxColor(mainPropertise);
                            break;
                    }
                }
               
            }

        }
    }
}
