using UnityEngine;
using UnityEditor.XCodeEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class DeployIOS {
    static MsdkEnv env = MsdkEnv.Instance;
    static string iosConfigPath = env.PATH_IOS_PLIST;

	static readonly string weixin =
	@"                <string>weixin</string>
                <key>CFBundleURLSchemes</key>
                <array>";
	static readonly string tencentopenapi =
	@"                <string>tencentopenapi</string>
                <key>CFBundleURLSchemes</key>
                <array>";
	static readonly string QQ =
	@"                <string>QQ</string>
                <key>CFBundleURLSchemes</key>
                <array>";
	static readonly string QQLaunch =
	@"                <string>QQLaunch</string>
                <key>CFBundleURLSchemes</key>
                <array>";
    static readonly string tencentVideo =
@"                <string>tencentvideo</string>
                <key>CFBundleURLSchemes</key>
                <array>";
	static readonly string offerId =
	@"        <key>MSDK_OfferId</key>";
	static readonly string qqAppId =
	@"        <key>QQAppID</key>";
	static readonly string qqAppKey =
	@"        <key>QQAppKey</key>";
	static readonly string wxAppId =
	@"        <key>WXAppID</key>";
	static readonly string msdkKey =
	@"        <key>MSDKKey</key>";


	public static void Deploy() {
		UpdateBaseInfo();
	}

    public static void Deploy(string projectPath)
    {
        string path = Path.GetFullPath(projectPath);
        //动态修改 MSDKXcodeConfig.projmods 文件的配置
        CopyFrameworks(path, ConfigSettings.Instance.UseC11);
        CopyOtherFiles(path);

        EditorMod(path);

        // 修改 plist 文件
        UpdateBaseInfo();
        EditorPlist(path);

        // 修改 xcode 代码(UnityAppController.mm)
        EditorCode(path);

        // 更新Config
        ConfigSettings.Instance.Update();
    }

    private static void CopyFrameworks(string pathToBuiltProject, bool isC11)
    {
        string destDir = pathToBuiltProject + "/MSDK";
        if (!Directory.Exists(destDir)) {
            Directory.CreateDirectory(destDir);
        }
        Debug.Log("isC11:" + isC11);
        string tail = "";
        if (isC11) {
            tail = "_C11";
        }
        MsdkUtil.ReplaceDir(env.PATH_LIBRARYS_IOS + "/Library/MSDK" + tail + "/MSDK.framework",
                            destDir + "/MSDK.framework");
        MsdkUtil.ReplaceDir(env.PATH_LIBRARYS_IOS + "/Library/MSDK" + tail + "/MSDKResources" + tail + ".bundle",
                            destDir + "/MSDKResources.bundle");
		MsdkUtil.ReplaceDir(env.PATH_LIBRARYS_IOS + "/Library/MSDK" + tail + "/WGPlatformResources"+ tail +".bundle",
							destDir + "/WGPlatformResources.bundle");

        MsdkUtil.ReplaceDir(env.PATH_LIBRARYS_IOS + "/MSDKAdapter" + tail + "/MSDKAdapter" + tail + ".framework",
                            destDir + "/MSDKAdapter.framework");
        if (!isC11) {
            return;
        }
        DirectoryInfo xcodeMsdk = new DirectoryInfo(destDir);
        DirectoryInfo[] frameworks = xcodeMsdk.GetDirectories("*.framework");
        foreach (DirectoryInfo framework in frameworks) {
            FileInfo[] files = framework.GetFiles("*_C11");
            foreach (FileInfo file in files) {
                file.MoveTo(file.DirectoryName + "/" + file.Name.Replace("_C11", ""));
            }
        }
    }

    private static void CopyOtherFiles(string pathToBuiltProject)
    {
        if (!MsdkUtil.isUnityEarlierThan("5.0")) {
            return;
        }

        string destDir = pathToBuiltProject + "/MSDK";
        if (!Directory.Exists(destDir)) {
            Directory.CreateDirectory(destDir);
        }

        //MsdkUtil.CopyDir(env.PATH_ADAPTER_IOS + "/oc", destDir + "/oc", true);
        MsdkUtil.CopyDir(env.PATH_BUGLY + "/iOS", destDir + "/bugly", true);
    }

    private static void EditorMod(string pathToBuiltProject)
    {
        Dictionary<string, string> modFileRules = new Dictionary<string, string>()
        {
            {".*/MSDK/MSDK.framework"            , "        \"" + pathToBuiltProject + "/MSDK/MSDK.framework\","},
            {".*/MSDK/WGPlatformResources.bundle", "        \"" + pathToBuiltProject + "/MSDK/WGPlatformResources.bundle\","},
            {".*/MSDK/MSDKResources.bundle"      , "        \"" + pathToBuiltProject + "/MSDK/MSDKResources.bundle\","},
			{".*/MSDK/MSDKAdapter.framework"     , "        \"" + pathToBuiltProject + "/MSDK/MSDKAdapter.framework\","},
            {".*/MSDK/bugly/BuglyBridge.h"       , "        \"" + pathToBuiltProject + "/MSDK/bugly/BuglyBridge.h\","},
            {".*/MSDK/bugly/libBuglyBridge.a"    , "        \"" + pathToBuiltProject + "/MSDK/bugly/libBuglyBridge.a\","}
        };

        MsdkUtil.ReplaceTextWithRegex(env.PATH_EDITOR + "/Resources/MSDKXcodeConfig.projmods", modFileRules);
    }

    private static void EditorPlist(string filePath)
    {
        DeployIOS.UpdateBaseInfo();
        XCPlist list = new XCPlist(filePath);
        string plistAdd = File.ReadAllText(iosConfigPath);
        list.AddKey(plistAdd);
        list.Save();
    }

    private static void EditorCode(string projectPath)
    {
        string ocFile = projectPath + "/Classes/UnityAppController.mm";

        StreamReader streamReader = new StreamReader(ocFile);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();
        if (string.IsNullOrEmpty(text_all)) {
            return;
        }
        if (text_all.Contains(MsdkNativeCode.IOS_HEADER)) {
            Debug.LogWarning("You are appending to XCode project, would not modified <Classes/UnityAppController.mm>");
            return;
        }

        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_HEADER, MsdkNativeCode.IOS_HEADER);
        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_FINISH, MsdkNativeCode.IOS_XG_REGISTER);
        MsdkUtil.ReplaceLineBelow(ocFile, MsdkNativeCode.IOS_SRC_OPENURL, "return ", MsdkNativeCode.IOS_HANDLE_URL);
        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_REGISTER, MsdkNativeCode.IOS_XG_SUCC);
        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_REGISTER_FAIL, MsdkNativeCode.IOS_XG_FAIL);
        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_RECEIVE, MsdkNativeCode.IOS_XG_RECEIVE);
        MsdkUtil.WriteBelow(ocFile, MsdkNativeCode.IOS_SRC_ACTIVE, MsdkNativeCode.IOS_XG_CLEAR + MsdkNativeCode.IOS_BECAME_ACTIVE);
    }

	public static void UpdateBaseInfo() {
		MsdkUtil.ReplaceBelow (iosConfigPath, weixin,
		                       "                    <string>" + DeploySettings.Instance.WxAppId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, tencentopenapi,
		                       "                    <string>tencent" + DeploySettings.Instance.QqAppId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, QQ,
			                    "                    <string>" + DeploySettings.Instance.QqScheme + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, QQLaunch,
		                       "                    <string>tencentlaunch" + DeploySettings.Instance.QqAppId + "</string>");
        MsdkUtil.ReplaceBelow(iosConfigPath, tencentVideo,
                               "                    <string>tencentvideo" + DeploySettings.Instance.QqAppId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, offerId,
		                       "        <string>" + DeploySettings.Instance.IOSOfferId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, qqAppId,
		                       "        <string>" + DeploySettings.Instance.QqAppId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, wxAppId,
		                       "        <string>" + DeploySettings.Instance.WxAppId + "</string>");
		MsdkUtil.ReplaceBelow (iosConfigPath, msdkKey,
		                       "        <string>" + DeploySettings.Instance.MsdkKey + "</string>");
	}

}
