using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[InitializeOnLoad]
class MsdkMenu : ScriptableObject
{
    [MenuItem("MSDK/Deploy Settings")]
    public static void EditDeploy()
    {
        Selection.activeObject = DeploySettings.Instance;
    }

    [MenuItem("MSDK/Config Settings")]
    public static void EditConfig()
    {
        Selection.activeObject = ConfigSettings.Instance;
    }

    [MenuItem("MSDK/PC Environment Settings")]
    public static void EditCallback()
    {
        Selection.activeObject = CallbackSettings.Instance;
    }

    [MenuItem("MSDK/MSDK Unity Wiki")]
    public static void OpenWiki()
    {
        string url = "http://msdk.ied.com/config.html";
        Application.OpenURL(url);
    }

    [MenuItem("MSDK/Feedback")]
    public static void EditFeedback()
    {
        EditorUtility.DisplayDialog("我要吐槽", "如果您在使用 MSDK Unity3d插件版中有问题或建议，请发邮件到<qingcuilu@tencent.com>反馈，感谢您帮助我们改善产品。", "确认");
    }
}
