using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;
using System.Collections.Generic;

public static class XCodePostProcess
{
    static MsdkEnv env = MsdkEnv.Instance;

#if UNITY_EDITOR
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild( BuildTarget target, string pathToBuiltProject)
    {

#if UNITY_5||UNITY_2017||UNITY_2017_1_OR_NEWER
        if (target == BuildTarget.iOS) {
#else
        if (target == BuildTarget.iPhone) {
#endif
            Debug.Log ("Run XCodePostProcess to Config Xcode project.");
        } else {
            return;
        }

        string path = Path.GetFullPath (pathToBuiltProject);

        // Create a new project object from build target
        XCProject project = new XCProject( path);

        // TODO GAME : Deploy MSDK to xcode project
        DeployIOS.Deploy(pathToBuiltProject);

        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        string[] files = null;
        files = Directory.GetFiles (Application.dataPath, "*.projmods", SearchOption.AllDirectories);
        foreach( string file in files ) {
            project.ApplyMod( file );
        }

        // TODO GAME : optional,设置签名的证书等属性
		project.overwriteBuildSetting("CODE_SIGN_IDENTITY", "iPhone Developer: Shuang Cao (6Y59CM28US)", "Release");
		project.overwriteBuildSetting("CODE_SIGN_IDENTITY", "iPhone Developer: Shuang Cao (6Y59CM28US)", "Debug");
		project.overwriteBuildSetting("PROVISIONING_PROFILE_SPECIFIER", "994d6619-2291-44a9-94ce-d1a25bdb5509", "Release");
		project.overwriteBuildSetting("PROVISIONING_PROFILE_SPECIFIER", "994d6619-2291-44a9-94ce-d1a25bdb5509", "Debug");
        project.overwriteBuildSetting ("ENABLE_BITCODE", "NO", "Release");
        project.overwriteBuildSetting ("ENABLE_BITCODE", "NO", "Debug");

        // Finally save the xcode project
        project.Save();
    }
#endif

}
