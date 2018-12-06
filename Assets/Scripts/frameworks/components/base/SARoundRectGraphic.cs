using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 本文来自 叫我上上 的CSDN 博客 ，全文地址请点击：https://blog.csdn.net/vikingsc2007_1/article/details/77232681?utm_source=copy 
/// </summary>
namespace Sakura
{
    [ExecuteInEditMode, RequireComponent(typeof(CanvasRenderer), typeof(RectTransform)), DisallowMultipleComponent]
    [AddComponentMenu("PTUI/RoundCorner (Unity UI Canvas)")]
    public class SARoundRectGraphic : MaskableGraphic
    {

        //Inspector面板上直接拖入  
        public Shader shader = null;

        [Range(0, 0.5f)]
        public float _cornerArea = 0;



        protected override void Start()
        {
            base.Start();
            material = GenerateMaterial(shader);
            material.SetFloat("_Width", rectTransform.rect.width);
            material.SetFloat("_Height", rectTransform.rect.height);
        }


        private void Update()
        {
            material.SetFloat("_RoundRadius", _cornerArea);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            material.SetFloat("_Width", rectTransform.rect.width);
            material.SetFloat("_Height", rectTransform.rect.height);
        }

        //根据shader创建用于屏幕特效的材质
        protected Material GenerateMaterial(Shader shader)
        {
            if (shader == null)
                return null;

            if (shader.isSupported == false)
                return null;
            Material material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;

            if (material)
                return material;

            return null;
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (material != null)
                Object.DestroyImmediate(material);
        }
    }

}