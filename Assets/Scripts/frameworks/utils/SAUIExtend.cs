using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public class SAUIExtend
    {
        private static Material shareGrayMaterial;
        private static string shareGrayShaderPath = "ZED/UISetToHuise";

        private static Material shareRoundRectMaterial;
        private static string shareRoundRectPath = "Sakura/RoundMask";

        private static Dictionary<Color, Texture> texture2Dictionary = new Dictionary<Color, Texture>();

        public static T GetComponent<T>(GameObject go, string path = "") where T : Component
        {
            if (go == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(path))
            {
                return go.GetComponent<T>();
            }

            Transform tran = go.transform.Find(path);
            if (tran == null)
            {
                return null;
            }
            return tran.GetComponent<T>();
        }

        public static Vector2 GetSize(GameObject go)
        {
            return go.GetUISize();
        }

        public static Material CreatShareGrayMaterial()
        {
            if (shareGrayMaterial == null)
            {
                shareGrayMaterial = new Material(Shader.Find(shareGrayShaderPath));
            }
            return shareGrayMaterial;
        }

        public static Material CreateRoundRectMaterial()
        {
            if (shareRoundRectMaterial == null)
            {
                shareRoundRectMaterial = new Material(Shader.Find(shareRoundRectPath));
            }
            return shareRoundRectMaterial;
        }


        public static Texture GetSharedColorTexture(Color color)
        {
            Texture result;
            if (texture2Dictionary.TryGetValue(color, out result) == false)
            {
                Texture2D texture2D = new Texture2D(1, 1);
                texture2D.SetPixel(1, 1, color);
                texture2D.Apply();

                result = texture2D;
                texture2Dictionary[color] = result;
            }
            return result;
        }

        public static RawImage CreateRawImage(string name = "RawImage", GameObject parent = null)
        {
            GameObject go = CreateEmpty(name, parent);
            RawImage image = go.AddComponent<RawImage>();
            image.raycastTarget = false;
            return image;
        }

        public static Image CreateImage(string name = "Image", GameObject parent = null)
        {
            GameObject go = CreateEmpty(name, parent);
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            return image;
        }

        public static GameObject CreateEmpty(string name, GameObject parent = null)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            go.AddComponent<CanvasRenderer>();
            go.layer = LayerMask.NameToLayer("UI");
            if (parent != null)
            {
                go.transform.SetParent(parent.transform, false);
            }
            return go;
        }
    }

}
