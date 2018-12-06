using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Msdk
{
	public class WGPlatformUnity
	{
		public void Init() 
		{
            string logVersion = "MSDK Unity Version : " + WGPlatform.Version;
            MsdkUtil.Log(logVersion);
            WGPlatform.Instance.WGBuglyLog(eBuglyLogLevel.eBuglyLogLevel_D, logVersion);

			MessageCenter.Instance.Init();
            WGPlatform.Instance.WGSetPermission(ePermission.eOPEN_ALL);
            BuglyAgent.ConfigCrashReporter(2, 4);
			// NOT Required. Enable debug log print, please set false for release version
			BuglyAgent.ConfigDebugMode (false);
            // Setting report level
            BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogException);
			// Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
			// please call this method to enable c# exception handler only.
			BuglyAgent.EnableExceptionHandler ();
			// NOT Required. If you need to report extra data with exception, you can set the extra handler
			// 只在iOS的C#异常时会触发
			//BuglyAgent.SetLogCallbackExtrasHandler (MyLogCallbackExtrasHandler);
		}
			
		/*
		Dictionary<string, string> MyLogCallbackExtrasHandler(){
			Debug.Log("MyLogCallbackExtrasHandler");
			Dictionary<string, string> extras = new Dictionary<string, string> ();
			extras.Add ("Unity crash extra data ", "lalala");
			return extras;
		}
		*/
	}
}

