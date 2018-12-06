using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using Msdk;
using UnityEngine.UI;

public class MsdkDemo : MonoBehaviour {

	// 手势滑动
	private Vector2 scrollPosition = Vector2.zero;
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;
	private float inertiaDuration = 0.5f;
	private Vector2 lastDeltaPos;

	// 界面相关参数
	public Font myFont;
	private int fontSize = Screen.width / 20;
	private GUIStyle messageStyle;
	private int messageFontSize = Screen.width / 28;
	private GUIStyle buttonStyle;
	private int buttonFontSize = Screen.width / 24;

	private ePlatform loginPlatform;
	private bool loginStateMaybeChange = true;
	private string loginState = "未登录";
	private string message = ""; // 结果展示
	private bool isShowDiffAcount = false;  // 是否展示异账号窗口
	private bool isClickable = true;

	private int toolbarID;  // 记录Toolbar按钮的ID

	#if UNITY_ANDROID
	private string[] toolbarInfo = new string[] {"QQ","微信","公告","其他"};   // Toolbar按钮上的信息
	#else
	private string[] toolbarInfo = new string[] {"QQ","微信","公告&游客","其他"};   // Toolbar按钮上的信息
	#endif
	private int diffAcountWindowWidth;
	private int diffAcountWindowHeight;

	private string inputText = "";
	private byte[] _imgData;
	string imgLocalUrl = "";
    string androidSdcard = "";
	WWW loadImg;
	///转半角的函数(DBC case)    
	///全角空格为12288，半角空格为32    
	///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248//     
	public static string ToDBC( string input)    
	{    
		char[] array = input.ToCharArray();    
		for (int i = 0; i < array.Length; i++)    
		{    
			if (array[i] == 12288)    
			{    
				array[i] = (char)32;    
				continue;    
			}    
			if (array[i] > 65280 && array[i] < 65375)    
			{    
				array[i] = (char)(array[i] - 65248);    
			}    
		}    
		return new string(array);    
	}    
	public byte[] imgData
	{
		get
		{
			if (_imgData == null || _imgData.Length == 0) {
//				Debug.Log ("Screen Shot");
//				var rect = new Rect (0, 0, Screen.width, Screen.height);
//				Texture2D screenShot = new Texture2D ((int)rect.width, (int)rect.height);
//				screenShot.ReadPixels (rect, 0, 0);
//				screenShot.Apply ();
//				_imgData = screenShot.EncodeToPNG ();
				Debug.Log ("load img");
				TextAsset imgBytes = Resources.Load<TextAsset>("tencentgame");
				_imgData = imgBytes.bytes;
			}
			Debug.Log ("get imgData:"+_imgData.Length);
			return _imgData;
		}
		set
		{
			_imgData = value;
		}
	}
    public GameObject emojiPanel;
    public static bool isShow = true;


	public ePlatform LoginPlatform {
		get {
			if (loginStateMaybeChange) {
				LoginRet ret = WGPlatform.Instance.WGGetLoginRecord();
				if (ret.flag == 0) {
					loginPlatform = (ePlatform)ret.platform;
				} else {
					loginPlatform = ePlatform.ePlatform_None;
				}
				loginStateMaybeChange = false;
			}
			return loginPlatform;
		}
	}

	void Awake()
	{
        /* TODO GAME
         * 第0步
         * 此MSDK Unity版本，提供了一个PC平台上虚拟的MSDK客户端环境。开启此环境后，部分接口(如登录，分享)调用后会返回
         * 包含伪造数据的回调事件，以帮忙游戏在PC环境下快速验证游戏逻辑，减少烦琐的打包工作。
         */
        WGPlatform.SetPCDebug(true);

        // 打开iOS系统的MSDK调试日志
#if UNITY_IPHONE
        WGPlatform.Instance.WGOpenMSDKLog(true);
#endif
        /* TODO GAME
         * 第1步
         * 因部分事件可在未调用相关接口时触发,所以需要在游戏初始化MSDK前，并尽早注册以下事件:
         * LoginEvent,WakeupEvent,CrashReportMessageEvent,CrashReportDataEvent.
         *
         * Crash时额外数据上报事件发生在非UnityMain线程，请避免在此回调中操作Unity对象,
         * 其他事件发生在UnityMain线程，可自由操作Unity对象。Debug.LogException()方法
         * 打印的异常也会上报到bugly
         */
        MsdkEvent.Instance.LoginEvent += (LoginRet ret) =>
		{
            Debug.Log(ret.ToString());

			switch (ret.flag)
			{
			case eFlag.eFlag_Succ:
				// 登陆成功, 可以读取各种票据
				int platform= ret.platform;
				if((int) ePlatform.ePlatform_Weixin == platform) {
					loginState = "微信登陆成功";
				} else if((int) ePlatform.ePlatform_QQ == platform) {
					loginState = "QQ登陆成功";
				} else if((int) ePlatform.ePlatform_QQHall == platform) {
					loginState = "大厅登陆成功";
				} else if((int) ePlatform.ePlatform_Guest == platform) {
					loginState = "游客登陆成功";
				}
				break;
				// 游戏逻辑，对登陆失败情况分别进行处理
			case eFlag.eFlag_Local_Invalid:
				// 自动登录失败, 需要重新授权, 包含本地票据过期, 刷新失败登所有错误
				loginState = "自动登陆失败";
				break;
            case eFlag.eFlag_Need_Realname_Auth:
                loginState = "需要实名认证";
                // TODO GAME 游戏如果配置realNameAuth为0或者1在这里取消对msdk的超时处理

                // TODO GAME 游戏如果配置realNameAuth为2在这里调用自定义的实名认证界面，
                // 并调用 WGRealNameAuth(RealNameAuthInfo info)处理用户填写内容，认证结果
                // 通过RealNameAuthEvent事件回调给游戏
                /*
                RealNameAuthInfo info = new RealNameAuthInfo();
                info.name = "李雷";
                info.identityType = eIDType.eIDType_HKMacaoTaiwanID;
                info.identityNum = "430455198411262142";
                //info.provinceID = 11;
                //info.city = "深圳";
                WGPlatform.Instance.WGRealNameAuth(info);
                */
                break;
			case eFlag.eFlag_WX_UserCancel:
			case eFlag.eFlag_WX_NotInstall:
			case eFlag.eFlag_WX_NotSupportApi:
			case eFlag.eFlag_WX_LoginFail:
			default:
				loginState = "登陆失败";
				break;
			}
		};

		MsdkEvent.Instance.WakeupEvent += (WakeupRet ret) =>
		{
            Debug.Log(ret.ToString());
			// TODO GAME 这里增加处理异账号的逻辑
			if (eFlag.eFlag_Succ == ret.flag || eFlag.eFlag_AccountRefresh == ret.flag) {
				// 本地账号登录成功可直接进入游戏
                if ((int)eWakeupPlatform.eWakeupPlatform_Weixin == ret.platform)
                {
					loginState = "微信登陆成功";
                }
                else if ((int)eWakeupPlatform.eWakeupPlatform_QQ == ret.platform)
                {
					loginState = "QQ登陆成功";
                }
                else if ((int)eWakeupPlatform.eWakeupPlatform_QQHall == ret.platform)
                {
					loginState = "大厅登陆成功";
                }
                else if ((int)eWakeupPlatform.eWakeupPlatform_TencentMsdk == ret.platform)
                {
                    loginState = "本地有票据，腾讯视频拉起成功";
                    message = "";
                    foreach (KVPair kv in ret.extInfo)
                    {
                        message = message + kv.key + ":" + kv.value + "\n"; 
                    }
                }
			} else if (eFlag.eFlag_UrlLogin == ret.flag) {
				// 本地无账号信息，自动用拉起的账号登录，登录结果在OnLoginNotify()中回调
			} else if (ret.flag == eFlag.eFlag_NeedSelectAccount) {
				// 异账号时，游戏需要弹出提示框让用户选择需要登陆的账号
				Debug.Log("diff account");
				isShowDiffAcount = true;
				message = "异账号！";
			} else if (ret.flag == eFlag.eFlag_NeedLogin) {
				// 没有有效的票据，登出游戏让用户重新登录
				loginState = "登录失败";
				message = "请重新登录";
                if ((int)eWakeupPlatform.eWakeupPlatform_TencentMsdk == ret.platform)
                {
                    loginState = "本地无票据腾讯视频拉起成功";
                    foreach (KVPair kv in ret.extInfo)
                    {
                        message = message + kv.key + ":" + kv.value + "\n";
                    }
                }
			} else {
				loginState = "未登录";
				message = "OnWakeupNotify    flag : " + ret.flag + "\ndesc : " + ret.desc;
				WGPlatform.Instance.WGLogout();
			}
		};

        /*
         * Crash时额外数据上报事件发生在非UnityMain线程，请避免在此回调中操作Unity对象。
         * 极少数系统异常场景下会导致Crash事件无法正常完成，从而使真正的Crash堆栈被覆盖，因此不建议实现此回调
         * 上报异常数据；推荐使用 WGBuglyLog() 接口在运行期间缓存需要上报的数据，Crash发生时会自动上报。详见：
         * http://wiki.dev.4g.qq.com/v4/Unity/bugly.html#Unity_PrintLog
         */
        MsdkEvent.Instance.CrashReportMessageEvent += () => {
            return "Report extra message when crash happened. MSDK version : " + WGPlatform.Version;
        };

        MsdkEvent.Instance.CrashReportDataEvent += () => {
            string extraDataStr = "Report extra data when crash happened. MSDK version : " + WGPlatform.Version;
            byte[] extraData = System.Text.Encoding.Default.GetBytes(extraDataStr);
            return extraData;
        };

        /*
         * 第2步
         * TODO GAME 初始化MSDK的C#层
         */
        WGPlatform.Instance.Init();
	}

	void Start ()
	{
       
        /*
		 * 以下事件可在相关接口调用前再注册
		 */
        MsdkEvent.Instance.RealNameAuthEvent += (RealNameAuthRet authRet) => {
            Debug.Log(authRet.ToString());
            if (authRet.flag == eFlag.eFlag_Succ) {
                // 实名认证成功，调用自动登录进入游戏
                WGPlatform.Instance.WGLogin(ePlatform.ePlatform_None);
            } else {
                // 实名认证失败，
                switch (authRet.errorCode) {
                    // 根据authRet.errorCode判断失败原因
                    case RealNameAuthErrCode.eErrCode_InvalidIDCard:
                    case RealNameAuthErrCode.eErrCode_InvalidChineseName:
                    // ......
                    default:
                        break;
                }
            }
        };

        MsdkEvent.Instance.ShareEvent += (ShareRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ) {
                // 分享成功
            } else {
                // 分享失败处理
            }
        };

        MsdkEvent.Instance.RelationEvent += (RelationRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();

            showMessageWithEmoji(message);
            message = "";
            if (ret.flag == eFlag.eFlag_Succ) {
                // 查询关系链成功
            } else {
                // 查询关系链失败
            }
        };

        MsdkEvent.Instance.NearbyEvent += (RelationRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            showMessageWithEmoji(message);
            message = "";
            if (ret.flag == eFlag.eFlag_Succ) {
                // 查询附近的人成功
            } else {
                // 查询附近的人失败
            }
        };

        MsdkEvent.Instance.LocateEvent += (LocationRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            
            if (ret.flag == eFlag.eFlag_Succ) {
                // 获取位置信息成功
            } else {
                // 获取位置信息失败
            }
        };

        MsdkEvent.Instance.FeedbackEvent += (flag, desc) => {
            Debug.Log("flag : " + flag + "; desc : " + desc);
            message = "flag : " + flag + "; desc : " + desc;
        };

        MsdkEvent.Instance.CreateWXGroupEvent += (GroupRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ) {
                // 创建微信群正确返回
            } else {
                // 创建微信群失败
            }
        };

        MsdkEvent.Instance.QueryGroupEvent += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            ///ret.mQQGroupInfo.relation表明qq群用户与qq群的关系，1:群主，2:管理员，3:普通成员，4:非成员
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 查询群信息成功
            }
            else
            {
                // 查询群信息失败
            }
        };

        MsdkEvent.Instance.JoinWXGroupEvent += (GroupRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ) {
                // 加入微信群正确返回
            } else {
                // 加入微信群失败
            }
        };
        MsdkEvent.Instance.JoinQQGroupEvent += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 查询群信息成功
            }
            else
            {
                // 查询群信息失败
            }
        };

        MsdkEvent.Instance.OnCreateGroupV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 创建绑定群信息成功
            }
            else
            {
                // 创建绑定群信息失败
            }
        };

        MsdkEvent.Instance.OnJoinGroupV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 加入群成功
            }
            else
            {
                // 创加入群失败
            }
        };

        MsdkEvent.Instance.QueryGroupInfoV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 查询群信息成功
            }
            else
            {
                // 查询群信息失败
            }
        };

        MsdkEvent.Instance.UnbindGroupV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 解绑群成功
            }
            else
            {
                // 解绑群失败
            }
        };

        MsdkEvent.Instance.GetGroupCodeV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 查询工会绑定群信息成功
            }
            else
            {
                // 查询工会绑定群信息失败
            }
        };

        MsdkEvent.Instance.QueryBindGuildV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 查询工会绑定群信息成功
            }
            else
            {
                // 查询工会绑定群信息失败
            }
        };

        MsdkEvent.Instance.BindExistGroupV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 绑定已创建的QQ群接口成功
            }
            else
            {
                // 绑定已创建的QQ群接口失败
            }
        };

        MsdkEvent.Instance.GetGroupListV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 拉取我创建的QQ群列表成功
            }
            else
            {
                // 拉取我创建的QQ群列表失败
            }
        };

        MsdkEvent.Instance.RemindGuildLeaderV2Event += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 提醒会长绑群成功
            }
            else
            {
                // 提醒会长绑群失败
            }
        };
        
        MsdkEvent.Instance.QueryWXGroupStatusEvent += (GroupRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if(eFlag.eFlag_Succ == ret.flag){
                Debug.Log("查询成功");
                if(ret.errorCode == 0){//0表示没有创建群或者加群
                    Debug.Log("没有创建群或者加群");
                }else if(ret.errorCode == 1){//表示已经创建群或者加群
                    Debug.Log("已经创建群或者加群");
                }
            }else{
                Debug.Log("查询失败");
            }
            
        };

        MsdkEvent.Instance.WebviewEvent += (WebviewRet ret) =>
        {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 打开浏览器
                Debug.Log("打开浏览器");
            }
            else if (ret.flag == eFlag.eFlag_Webview_closed)
            {
                // 关闭浏览器
                Debug.Log("关闭浏览器");
            }
            else if (ret.flag == eFlag.eFlag_Webview_page_event) 
            {
                // 传递的js消息
                Debug.Log("传递的js消息:"+ret.msgData);
            }
        };
		MsdkEvent.Instance.BindGroupEvent += (GroupRet ret) => {
			Debug.Log(ret.ToString());
			message = ret.ToString();
			if (ret.flag == eFlag.eFlag_Succ) {
				// 绑定QQ群成功
			} else {
				// 绑定QQ群失败
			}
		};
        MsdkEvent.Instance.UnbindGroupEvent += (GroupRet ret) =>
        {
            Debug.Log("UnbindGroupEvent");
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ)
            {
                // 解绑QQ群成功
            }
            else
            {
                // 解绑QQ群失败
            }
        };
		MsdkEvent.Instance.QueryQQGroupKeyEvent += (GroupRet ret) => {
			Debug.Log(ret.ToString());
			message = ret.ToString();
			if (ret.flag == eFlag.eFlag_Succ) {
				// 查询QQ群Key成功
				string groupKey = ret.mQQGroupInfo.groupKey;
			} else {
				// 查询QQ群Key失败
			}
		};
#if UNITY_ANDROID || UNITY_EDITOR
        MsdkEvent.Instance.AddWXCardEvent += (CardRet ret) => {
            Debug.Log(ret.ToString());
            message = ret.ToString();
            if (ret.flag == eFlag.eFlag_Succ) {
                // 微信插入卡卷成功
            } else {
                // 微信插入卡卷失败
            }
        };
			
        MsdkEvent.Instance.CheckUpdateEvent += (long newApkSize, string newFeature, long patchSize,
            int status, string updateDownloadUrl, int updateMethod) => {
                Debug.Log("newApkSize : " + newApkSize + "; newFeature : " + newFeature + "; patchSize : "
                    + patchSize + "; updateDownloadUrl : " + updateDownloadUrl);
            if (status == TMSelfUpdateUpdateInfo.STATUS_OK) {
                //查询成功
                if (updateMethod == TMSelfUpdateUpdateInfo.UpdateMethod_NoUpdate) {
                    message = "无更新包";
                } else if (updateMethod == TMSelfUpdateUpdateInfo.UpdateMethod_Normal) {
                    message = "可进行全量更新, url=" + updateDownloadUrl + ", newApkSize=" + newApkSize + ", patchSize=" + patchSize;
                } else if (updateMethod == TMSelfUpdateUpdateInfo.UpdateMethod_ByPatch) {
                    message = "可进行增量更新, url=" + updateDownloadUrl + ", newApkSize=" + newApkSize + ", patchSize=" + patchSize;
                }
            } else {
                message = status + " : 查询失败";
            }
        };

        MsdkEvent.Instance.DownloadAppProgressEvent += (long receiveDataLen, long totalDataLen) => {
            Debug.Log("receiveDataLen : " + receiveDataLen + "; totalDataLen : " + totalDataLen);
            long progress = receiveDataLen * 100 / totalDataLen;
            message = "下载中，已完成 " + progress + "%";
        };

        MsdkEvent.Instance.DownloadAppStateEvent += (int state, int errorCode, string errorMsg) => {
            Debug.Log("state : " + state + "; errorCode : " + errorCode + "; errorMsg : " + errorMsg);
            if (errorCode == 0) {
                message = "下载完成";
            } else {
                message = "下载失败";
            }
        };

        MsdkEvent.Instance.DownloadYYBProgressEvent += (string url, long receiveDataLen, long totalDataLen) => {
            Debug.Log("Download YYB url is " + url);
            Debug.Log("receiveDataLen : " + receiveDataLen + "; totalDataLen : " + totalDataLen);
            long progress = receiveDataLen * 100 / totalDataLen;
            message = "下载中，已完成 " + progress + "%";
        };

        MsdkEvent.Instance.DownloadYYBStateEvent += (string url, int state, int errorCode, string errorMsg) => {
            Debug.Log("Download YYB url is " + url);
            Debug.Log("state : " + state + "; errorCode : " + errorCode + "; errorMsg : " + errorMsg);
            if (errorCode == 0) {
                message = "下载完成";
            } else {
                message = "下载失败";
            }
        };
#endif

// Demo 初始化参数
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

#elif UNITY_ANDROID
// Android自动登录 : 在Android生命周期 onResume 中调用 WGPlatform.onResume()，MsdkActivity中已实现
// 调用安卓系统方法获取sdcard存储区路径
AndroidJavaClass androidEnv = new AndroidJavaClass("android.os.Environment");
androidSdcard = androidEnv.CallStatic<AndroidJavaObject> ("getExternalStorageDirectory").Call <string>("getPath");
imgLocalUrl = androidSdcard + "/test.jpg";
loadImg = new WWW("file://" + imgLocalUrl);
imgData = loadImg.bytes;

#endif
	}

	// Update is called once per frame
	void Update ()
	{
		// Demo 手势滑动实现
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
				lastDeltaPos = Input.GetTouch(0).deltaPosition;
			}
			else if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				print ("End:"+lastDeltaPos.y+"|"+Input.GetTouch(0).deltaTime);
				if (Mathf.Abs(lastDeltaPos.y)> 20.0f)
				{
					scrollVelocity = (int)(lastDeltaPos.y * 0.5/ Input.GetTouch(0).deltaTime);
					//print(scrollVelocity);
				}
				timeTouchPhaseEnded = Time.time;
			}
		}
		else
		{
			if (scrollVelocity != 0.0f)
			{
				// slow down
				float t = (Time.time - timeTouchPhaseEnded)/inertiaDuration;
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
				scrollPosition.y += frameVelocity * Time.deltaTime;

				if (t >= inertiaDuration)
					scrollVelocity = 0;
			}
		}
	}

	void OnGUI()
	{
        if (!isShow)
        {
            return;
        }
		// 界面相关参数
        if (Screen.height > Screen.width) {
            fontSize = Screen.width / 22;
            messageFontSize = Screen.width / 27;
            buttonFontSize = Screen.width / 24;
            defaultButtonHeight = Screen.height / 12;
            toolbarID = GUILayout.Toolbar(toolbarID, toolbarInfo, GUILayout.Height(Screen.height / 13));
        } else {
            fontSize = Screen.width / 35;
            messageFontSize = Screen.width / 38;
            buttonFontSize = Screen.width / 35;
            defaultButtonHeight = Screen.height / 10;
            toolbarID = GUILayout.Toolbar(toolbarID, toolbarInfo, GUILayout.Height(Screen.height / 12));
        }


		GUI.skin.label.font = myFont;
		GUI.skin.button.font = myFont;
		GUI.skin.textField.font = myFont;
		GUI.skin.label.fontSize = fontSize;
		GUI.skin.button.fontSize = buttonFontSize;
		GUI.skin.textField.fontSize = fontSize;
		messageStyle = new GUIStyle (GUI.skin.label);
		messageStyle.fontSize = messageFontSize;
		buttonStyle = new GUIStyle (GUI.skin.button);
		buttonStyle.fontSize = buttonFontSize;

		diffAcountWindowWidth = Screen.width;
		diffAcountWindowHeight = Screen.height / 3;

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));

		GUILayout.Label("登录态 : " + loginState);
		GUILayout.Label("消息 : ");
		GUILayout.Label (message, messageStyle);
       // StartCoroutine(this.SetUITextThatHasEmoji(this.receipeText, "\U0001F3B5 \U0001F3B6 \U0001F3B7 \U0001F3B8 \U0001F3B9 \U0001F3BA"));
		LabelAndTextField ("部分接口测试需要输入参数:", ref inputText, fontSize * 2);

		if (isShowDiffAcount)
		{
			PopWindow(0, diffAcountWindowWidth, diffAcountWindowHeight, ShowDiffAcount);
		}
		else
		{
			switch(toolbarID)
			{
			case 0: ShowQQ(); break;
			case 1: ShowWX(); break;
			case 2: ShowNotice(); break;
			case 3: ShowOthers(); break;
			default: ShowQQ(); break;
			}
		}
		GUILayout.EndScrollView();

	}

	// 用于按钮延时点击，避免短时重复点击
	private void letClickable() {
		isClickable = true;
	}

	// 异账号弹窗处理示例
	void ShowDiffAcount(int windowID)
	{
		int row = 2;
		GUILayout.Label("请选择登录账户", GUILayout.MaxWidth(500));
		GUILayout.BeginHorizontal ();
        if (Button("本地账号", row))
		{
			Debug.Log ("本地账户");
			if(!WGPlatform.Instance.WGSwitchUser(false)){
				WGPlatform.Instance.WGLogout();
				loginState = "未登录";
				loginStateMaybeChange = true;
			}
			message = " ";
			isShowDiffAcount = false;
		}
        if (Button("拉起账号", row))
		{
			Debug.Log ("拉起账户");
			if(!WGPlatform.Instance.WGSwitchUser(true)){
				WGPlatform.Instance.WGLogout();
				loginState = "未登录";
				loginStateMaybeChange = true;
			}
			message = " ";
			isShowDiffAcount = false;
		}
		GUILayout.EndHorizontal ();
	}

	#region 接口调用示例
	void ShowQQ()
	{
		// 示例参数
		string title = "QQ分享-title";
		string desc = "QQ分享-desc";
		string summary = "QQ分享-summary";
		string previewText = "";
		string msdkExtInfo = "msdkExtInfo";
		string imgNetUrl = "http://qzonestyle.gtimg.cn/open_proj/proj_open_v2/ac/home/qrcode.jpg";
		string musicUrl = "http://y.qq.com/i/song.html?songid=1135734&source=qq";
		string musicDataUrl = "http://wiki.dev.4g.qq.com/v2/cry.mp3";
		string targetUrl = "http://gamecenter.qq.com/gcjump?game_tag=MSG_INVITE&plat=qq&pf=invite&appid=100703379&from=androidqq&" +
			"uin=182849215&originuin=61793295&platformId=qq_m&sid=Ac0o-208NGD3k3FNCv3J4Q4f&gamedata=gamedata&platformdata=platformdata";
		ImageParams imgParam = new ImageParams();
#if UNITY_ANDROID
		//图片支持png,jpg 必须放在 sdcard 中
        imgParam.android_imagePath = imgLocalUrl;
#elif UNITY_IPHONE ||UNITY_IOS
		imgParam.ios_imageData = imgData;
		imgParam.ios_imageDataLen = imgData.Length;
#endif
		GUILayout.BeginHorizontal ();
		if (Button ("QQ登录", 3)) {
			Debug.Log ("点击QQ登录");
			WGPlatform.Instance.WGLogin (ePlatform.ePlatform_QQ);
		}
		if (Button ("QQ登录Opt", 3)) {
			Debug.Log ("点击QQ登录");
			WGPlatform.Instance.WGLoginOpt (ePlatform.ePlatform_QQ);
		}
		if (Button ("登出", 3)) {
			Debug.Log ("登出");
			WGPlatform.Instance.WGLogout ();
			loginState = "未登录";
			loginStateMaybeChange = true;
			message = " ";
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		if (Button ("个人信息", 2))
		{
			Debug.Log ("个人信息");
			WGPlatform.Instance.WGQueryQQMyInfo ();
		}
		if (Button ("同玩好友信息", 2))
		{
			Debug.Log ("同玩好友信息");
			WGPlatform.Instance.WGQueryQQGameFriendsInfo ();
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("结构化分享到空间", 3))
		{
			string testUrl = ToDBC(targetUrl + "；");

			Debug.Log ("分享到QQ空间"+testUrl);
			#if UNITY_ANDROID
				WGPlatform.Instance.WGSendToQQ (eQQScene.QQScene_QZone, title, desc, targetUrl, imgNetUrl, imgNetUrl.Length);
			#else
				WGPlatform.Instance.WGSendToQQ (eQQScene.QQScene_QZone, title, desc, testUrl, imgData, imgData.Length);
			#endif
		}

		if (Button ("结构化分享到好友", 3))
		{
			Debug.Log ("分享到QQ好友");
			#if UNITY_ANDROID
			WGPlatform.Instance.WGSendToQQ (eQQScene.QQScene_Session, title, desc, targetUrl, imgLocalUrl, imgLocalUrl.Length);
			#else
			WGPlatform.Instance.WGSendToQQ (eQQScene.QQScene_Session, title, desc, targetUrl, imgData, imgData.Length);
			#endif
		}

		if (Button ("分享音乐到空间", 3))
		{
			Debug.Log ("分享音乐到空间");
			WGPlatform.Instance.WGSendToQQWithMusic (eQQScene.QQScene_QZone,title, desc, musicUrl, musicDataUrl, imgNetUrl);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("分享音乐到好友", 3)) {
			Debug.Log ("分享音乐到好友");
			WGPlatform.Instance.WGSendToQQWithMusic (eQQScene.QQScene_Session, title, desc, musicUrl, musicDataUrl, imgNetUrl);
		}
		if (Button ("后端分享到好友", 3)) {
			Debug.Log ("后端分享到好友");
			if (inputText == "") {
				message = "请在下面输入框中输入同玩好友的openid";
			} else {
				WGPlatform.Instance.WGSendToQQGameFriend (1, inputText, title, summary, targetUrl, imgNetUrl, previewText, "MSG_FRIEND_EXCEED", msdkExtInfo);
			}

		}
		if (Button ("纯图分享到好友", 3)) {
			Debug.Log ("纯图分享到好友");
			#if UNITY_ANDROID
			//图片支持png,jpg 必须放在 sdcard 中
			WGPlatform.Instance.WGSendToQQWithPhoto (eQQScene.QQScene_Session, imgLocalUrl);
			#else
			WGPlatform.Instance.WGSendToQQWithPhoto (eQQScene.QQScene_Session, imgData, imgData.Length);
			#endif
		}
        GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal ();
		if (Button ("纯图分享到空间", 3)) {
			Debug.Log ("纯图分享到空间");
			#if UNITY_ANDROID
			WGPlatform.Instance.WGSendToQQWithPhoto (eQQScene.QQScene_QZone, imgLocalUrl);
			#else
			WGPlatform.Instance.WGSendToQQWithPhoto (eQQScene.QQScene_QZone, imgData, imgData.Length);
			#endif
		}
		if (Button ("纯图带参分享空间", 3)) {
			//图片支持png,jpg 必须放在 sdcard 中
			Debug.Log("纯图带参分享空间");
			#if UNITY_ANDROID

			WGPlatform.Instance.WGSendToQQWithPhoto(eQQScene.QQScene_QZone, imgParam,"MESSAGE_ACTION_SNS_CANVAS#Honor=1","messagExt");
			#else

			WGPlatform.Instance.WGSendToQQWithPhoto (eQQScene.QQScene_QZone, imgParam,"MESSAGE_ACTION_SNS_CANVAS#Honor=1","messagExt");
			#endif
		}
        #if UNITY_ANDROID
        if (Button ("富图分享", 3)) {
            Debug.Log ("富图分享");
            ArrayList imgs = new ArrayList();
            string mySummary = "我的游戏精彩瞬间";
            imgs.Add(androidSdcard + "/test.jpg");
            imgs.Add(androidSdcard + "/test2.jpg");
            imgs.Add(androidSdcard + "/test3.jpg");
            WGPlatform.Instance.WGSendToQQWithRichPhoto(mySummary, imgs);
        }
        if (Button ("视频分享", 3)) {
            Debug.Log ("视频分享");
            string mySummary = "我的游戏精彩瞬间";
            string myVideo = androidSdcard + "/cry.mp4";
            WGPlatform.Instance.WGSendToQQWithVideo(mySummary, myVideo);
        }
        
        #endif
		GUILayout.EndHorizontal ();



		GUILayout.BeginHorizontal ();
		if (Button ("添加QQ好友", 4)) {
			Debug.Log ("添加QQ好友");
			if (inputText == "") {
				message = "请在下面输入框中输入需要添加为好友的玩家openid";
			} else {
				WGPlatform.Instance.WGAddGameFriendToQQ (inputText, "demo玩家A", "你好，我是demo玩家B");
			}
		}
		if (Button ("绑定QQ群", 4)) {
			Debug.Log ("绑定QQ群");
            string[] paramArr = inputText.Split(' ');
            if (paramArr.Length!=3)
            {
                message = "请再下面输入框中输入unionid union_name zonid 并用空格隔开";
                return;
            }
            string unionid = paramArr[0];
            string union_name = paramArr[1];
            string zoneid = paramArr[2];
			// signature = md5("玩家openid_游戏appid_游戏appkey_公会id_区id")
			// 一般游戏后台计算后传给客户端
			// 此sigature使用的openid为2996337391账号，非此账号登录点击此按钮会提示“身份验证失败”
            LoginRet lr = WGPlatform.Instance.WGGetLoginRecord();
            string beforecry = lr.open_id + "_" + "100703379" + "_" + "4578e54fb3a1bd18e0681bc1c734514e" + "_" + unionid + "_" + zoneid;
            string sigature = md5String(beforecry);
            Debug.Log("unionid:" + unionid + ",union_name:" + union_name + ",zoneid:" + zoneid+",beforecry:"+beforecry);
            Debug.Log("sigature:" + sigature);
			WGPlatform.Instance.WGBindQQGroup (unionid, union_name, zoneid, sigature);
		}
        if (Button("查询加群Key", 4))
        {
            Debug.Log("查询QQ群的加群Key");
            string group_openid = inputText;
            if (group_openid == "")
            {
                message = "参数错误，请输入QQ群的Openid";
            }
            else
            {
                WGPlatform.Instance.WGQueryQQGroupKey(group_openid);
            }
        }
        if (Button ("游戏内加QQ群", 4)) {
            Debug.Log ("游戏内加QQ群");
            string[] paramArr = inputText.Split(' ');
            if (2 == paramArr.Length && null != paramArr[0] && null != paramArr[1]) {
                //例如：318107239 ce201c6a5038cdcd2abf326586b9e29703a618be6af8acaeadf23c0ae85dd54c
                WGPlatform.Instance.WGJoinQQGroup(paramArr[0], paramArr[1]);
            } else {
                message = "参数错误，请输入 QQ群的群号 QQ群的加群Key";
            }

        }
		GUILayout.EndHorizontal ();

		#if UNITY_ANDROID || UNITY_EDITOR
		GUILayout.BeginHorizontal ();
		if (Button ("查询QQ群信息", 3)) {
			Debug.Log ("查询绑定的QQ群信息");
			string[] paramArr = inputText.Split(' ');
			if(2 == paramArr.Length && null != paramArr[0] && null != paramArr[1]) {
				WGPlatform.Instance.WGQueryQQGroupInfo (paramArr[0],paramArr[1]);
			} else {
				message = "参数错误，请输入: 公会ID 大区ID \n 如:\n11 22";
			}
		}

		GUILayout.EndHorizontal ();
		#endif

        GUILayout.BeginHorizontal();
        if (Button("绑定Q群V2", 4))
        {
            Debug.Log("绑定QQ群V2接口");
			string[] paramArr = inputText.Split(' ');
            string guildId, guildName, zoneid, roleId, partition;

            if (5 == paramArr.Length && null != paramArr[0] && null != paramArr[1] && null != paramArr[2] && null != paramArr[3] && null!=paramArr[4])
            {
                guildId = paramArr[0]; guildName = paramArr[1]; zoneid = paramArr[2]; roleId = paramArr[3]; partition = paramArr[4];
                GameGuild gameGuild = new GameGuild();
                gameGuild.guildId = guildId; gameGuild.guildName = guildName; gameGuild.zoneId = zoneid; gameGuild.roleId = roleId; gameGuild.partition = partition;
                WGPlatform.Instance.WGCreateQQGroupV2(gameGuild);
			} else {
                message = "参数错误，请输入guildId,guildName,zoneid,roleId,partition并用空格分开";
			}
		}
        if (Button("查询Q群V2", 4))
        {
            Debug.Log("查询Q群V2接口");

            string groupId = inputText;

            if (null != groupId && groupId.Length>0)
            {
                WGPlatform.Instance.WGQueryQQGroupInfoV2(groupId);
            }
            else
            {
                message = "参数错误，请输入groupId";
            }
        }
        if (Button("加入Q群V2", 4))
        {
            Debug.Log("加入QQ群V2接口");
            string[] paramArr = inputText.Split(' ');
            string guildId, zoneId, groupId, roleId,partition;

            if (5 == paramArr.Length && null != paramArr[0] && null != paramArr[1] && null != paramArr[2] && null != paramArr[3] && null != paramArr[4])
            {
                guildId = paramArr[0]; zoneId = paramArr[1]; groupId = paramArr[2]; roleId = paramArr[3]; partition = paramArr[4];
                GameGuild gameGuild = new GameGuild();
                gameGuild.guildId = guildId; gameGuild.zoneId = zoneId; gameGuild.roleId = roleId; gameGuild.partition = partition;
                WGPlatform.Instance.WGJoinQQGroupV2(gameGuild,groupId);
            }
            else
            {
                message = "参数错误，请输入guildId, zoneId, groupId, roleId, partition并用空格分开";
            }
        }
         if (Button("解绑Q群V2", 4))
        {
            Debug.Log("解绑QQ群V2接口");
            string[] paramArr = inputText.Split(' ');
            string guildId, guildName, zoneid;

            if (3 == paramArr.Length && null != paramArr[0] && null != paramArr[1] && null != paramArr[2])
            {
                guildId = paramArr[0]; guildName = paramArr[1]; zoneid = paramArr[2];
                GameGuild gameGuild = new GameGuild();
                gameGuild.guildId = guildId; gameGuild.guildName = guildName; gameGuild.zoneId = zoneid;
                WGPlatform.Instance.WGUnbindQQGroupV2(gameGuild);
            }
            else
            {
                message = "参数错误，请输入guildId,guildName,zoneid 并用空格分开";
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        //拉取我创建的QQ群列表
        if (Button("群列表V2", 3))
        {
            Debug.Log("群列表");
            WGPlatform.Instance.WGGetQQGroupListV2();
        }
        //绑定我已创建的QQ群
        if (Button("绑已有群V2", 3))
        {
            Debug.Log("绑已有群");
            string[] paramArr = inputText.Split(' ');
            if (5 == paramArr.Length && !String.IsNullOrEmpty(paramArr[0]) && !String.IsNullOrEmpty(paramArr[1])&&
                !String.IsNullOrEmpty(paramArr[2]) && !String.IsNullOrEmpty(paramArr[3]) && !String.IsNullOrEmpty(paramArr[4]))
            {
                GameGuild gameguild = new GameGuild();
                string groupId = paramArr[0]; string groupName = paramArr[1];
				gameguild.guildId = paramArr[2]; gameguild.zoneId = paramArr[3];
				gameguild.roleId = paramArr[4];
                WGPlatform.Instance.WGBindExistQQGroupV2(gameguild, groupId, groupName);
            }
            else
            {
				message = "参数错误，请输入: groupId groupName guildId zoneId roleId并用空格隔开";
            }
        }
        //提醒会长绑群
        if (Button("提醒会长绑群V2", 3))
        {
			Debug.Log ("提醒会长绑群");
			string[] paramArr = inputText.Split (' ');

			if (11 == paramArr.Length) {
                
				for (int i = 0; i < 10; i++) {
					if (String.IsNullOrEmpty (paramArr [i])) {
						message = "参数错误，请输入: guildId zoneId roleId roleName partition userZoneId type leaderOpenId leaderRoleId leaderZoneId areaId\n并用空格隔开";
						return; 
					}
				}
				GameGuild gameguild = new GameGuild ();
				gameguild.guildId = paramArr [0]; 
				gameguild.zoneId = paramArr [1];
				gameguild.roleId = paramArr [2]; 
				gameguild.roleName = paramArr [3]; 
				gameguild.partition = paramArr [4]; 
				gameguild.userZoneId = paramArr [5];
				gameguild.type = paramArr [6];

				if (paramArr [7] == "0") {
					LoginRet lr = WGPlatform.Instance.WGGetLoginRecord ();
					gameguild.leaderOpenId = lr.open_id;
				} else {
					gameguild.leaderOpenId = paramArr [7];
				}
				gameguild.leaderRoleId = paramArr [8];
				gameguild.leaderZoneId = paramArr [9];
				gameguild.areaId = paramArr [10];
				WGPlatform.Instance.WGRemindGuildLeaderV2 (gameguild);
			}else {
				message = "参数错误，请输入: guildId zoneId roleId roleName partition userZoneId type leaderOpenId leaderRoleId leaderZoneId areaId\n并用空格隔开";
			}
		} 

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        //查询群绑定的公会
        if (Button("群绑的公会V2", 2))
        {
            Debug.Log("群绑的公会");
            string[] paramArr = inputText.Split(' ');
            if (2 == paramArr.Length && !String.IsNullOrEmpty(paramArr[0]) && !String.IsNullOrEmpty(paramArr[1]))
            {
                GameGuild gameGuild = new GameGuild();
                string groupId = paramArr[0]; int type = int.Parse(paramArr[1]);
                WGPlatform.Instance.WGQueryBindGuildV2(groupId, type);
            }
            else
            {
                message = "参数错误，请输入: 工会groupId 公会type 用空格隔开";
            }
        } 
		//查询工会绑的群
        if (Button("公会绑的群V2", 2))
        {
			Debug.Log("公会绑的群");
            string[] paramArr = inputText.Split(' ');
            if (2 == paramArr.Length && !String.IsNullOrEmpty(paramArr[0]) && !String.IsNullOrEmpty(paramArr[1]))
            {
                GameGuild gameGuild = new GameGuild();
                string guildId = paramArr[0]; 
                string zoneId = paramArr[1];
                gameGuild.guildId = guildId;
                gameGuild.zoneId = zoneId;
                WGPlatform.Instance.WGGetQQGroupCodeV2(gameGuild);
            }
            else
            {
				message = "参数错误，请输入: 公会groupId 公会zoneId 用空格隔开";
            }
        } 
        
        GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal ();
		#if UNITY_ANDROID
		if (Button ("解绑QQ群", 3)) {
			Debug.Log ("解绑已绑定的QQ群");
			string[] paramArr = inputText.Split(' ');
			if(2 == paramArr.Length && null != paramArr[0] && null != paramArr[1]) {
				WGPlatform.Instance.WGUnbindQQGroup (paramArr[0],paramArr[1]);
			} else {
				message = "参数错误，请输入: QQ群openid 公会ID";
			}
		}
		#endif
		if (Button ("手Q是否安装", 3))
		{
			Debug.Log ("手Q是否安装");
			message = (WGPlatform.Instance.WGIsPlatformInstalled(ePlatform.ePlatform_QQ)).ToString();
		}
		if (Button ("获取手Q版本", 3))
		{
			Debug.Log ("获取手Q版本");
			#if UNITY_IPHONE
			message = "ios not available";
			#else
			message = WGPlatform.Instance.WGGetPlatformAPPVersion(ePlatform.ePlatform_QQ);
			#endif
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("获取登录票据", 3)) {
			Debug.Log ("获取登录票据");
			LoginRet ret = WGPlatform.Instance.WGGetLoginRecord();
			if (ret != null) {
                message = ret.ToString();
			} else {
				message = "unity WGGetLoginRecord failed";
			}

		}
		if (Button ("获取pf+pfkey", 3)) {
			Debug.Log ("获取pf+pfkey");
			message = WGPlatform.Instance.WGGetPf() + "\n" + WGPlatform.Instance.WGGetPfKey();

		}
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        Button(""); // 解决Demo在PC运行时界面显示不全
        GUILayout.EndHorizontal ();
	}

	void ShowWX()
	{
		// 示例参数
		string title = "WX分享-title";
		string desc = "WX分享-desc";
		string mediaId = "";
		string mediaTagName = "MSG_INVITE";
		string messageExt = "messageExt";
		string messageAction = "WECHAT_SNS_JUMP_URL";
		string msdkExtInfo = "msdkExtInfo";
		string imgNetUrl = "http://qzonestyle.gtimg.cn/open_proj/proj_open_v2/ac/home/qrcode.jpg";
		string musicUrl = "http://y.qq.com/i/song.html?songid=1135734&source=qq";
		string musicDataUrl = "http://wiki.dev.4g.qq.com/v2/cry.mp3";
		string targetUrl = "http://www.qq.com";


        string video_tile = "视频分享";
        string video_desc = "微信短视频分享";
        string video_mediaTagName = "MSG_SHARE_MOMENT_VIDEO";
        string video_messageAction = "MESSAGE_ACTION_SNS_VIDEO#gameseq=1491995805&GameSvrEntity=87929&RelaySvrEntity=2668626528&playersnum=10&IsShortVideo";
        string thumbUrl = "http://vweixinthumb.tc.qq.com/150/20250/snsvideodownload?filekey=30270201010420301e02020096040253480410f8ba73c92741da1cd13c9824313bc15902024ea20400&hy=SH&storeid=32303137303530383038353035323030303735633830626533333166346336613965333230613030303030303936&bizid=1023";
        string videoUrl = "http://shzjwxsns.video.qq.com/102/20202/snsvideodownload?filekey=30270201010420301e02016604025348041086277b0fa88402549570698994b49289020310eefe0400&hy=SH&storeid=32303137303530383038353035323030303832333037626533333166346336613965333230613030303030303636&bizid=1023";
        VideoParams videoParams = new VideoParams();
        ImageParams imgParams = new ImageParams();
#if UNITY_ANDROID
        imgParams.android_imagePath = androidSdcard + "/test.jpg"; ;
#endif
        string miniapp_username = "gh_e9f675597c15";
        string miniapp_path = "pages/indexSelAddr/indexSelAddr";
        
#if UNITY_ANDROID
        videoParams.android_videoPath = androidSdcard + "/cry.mp4";
#endif

#if UNITY_IPHONE||UNITY_IOS
		TextAsset videoBytes = Resources.Load<TextAsset>("video.mp4");
		videoParams.ios_videoData = videoBytes.bytes;
		videoParams.ios_videoDataLen = videoBytes.bytes.Length;
#endif

		GUILayout.BeginHorizontal ();
		if (Button ("微信登录", 3)) {
			Debug.Log ("点击登录微信");
			WGPlatform.Instance.WGLogin (ePlatform.ePlatform_Weixin);
		}
		if (Button ("微信登录Opt", 3)) {
			Debug.Log ("点击登录微信");
			WGPlatform.Instance.WGLoginOpt(ePlatform.ePlatform_Weixin);
		}
		if (Button ("扫码登录", 3)) {
			Debug.Log ("点击扫码登录");
			WGPlatform.Instance.WGQrCodeLogin (ePlatform.ePlatform_Weixin);
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		if (Button ("登出", 2)) {
			Debug.Log ("登出");
			WGPlatform.Instance.WGLogout ();
			loginStateMaybeChange = true;
			loginState = "未登录";
			message = " ";
		}

		if (Button ("刷新Token", 2)) {
			Debug.Log ("手动刷新微信AcessToken");
			WGPlatform.Instance.WGRefreshWXToken();
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("个人信息", 3)) {
			Debug.Log ("查询个人信息");
			WGPlatform.Instance.WGQueryWXMyInfo();
		}
		if (Button ("同玩好友信息", 3)) {
			Debug.Log ("查询同玩好友信息");
			WGPlatform.Instance.WGQueryWXGameFriendsInfo();
		}
#if UNITY_ANDROID
		if (Button ("插卡到微信卡包", 3)) {
			Debug.Log ("插卡到微信卡包");
			string cardid = "pe7Gmjg3EpKtnwzNAGHMGJhNKSo4";
			string timestamp = "1111111111";
			string sign = "sdffsfffff";
            WGPlatform.Instance.WGAddCardToWXCardPackage(cardid, timestamp, sign);
            /*
			string[] paramArr = inputText.Split(' ');
			if(3 == paramArr.Length && null != paramArr[0] && null != paramArr[1] && null != paramArr[2]) {
				WGPlatform.Instance.WGAddCardToWXCardPackage(paramArr[0], paramArr[1], paramArr[2]);
			} else {
                message = "插卡到微信卡包 参数错误";
				Debug.Log ("插卡到微信卡包 参数错误");
			}
             * */
		}
#endif
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("结构化消息分享", 3)) {
			Debug.Log ("结构化消息分享");;
			WGPlatform.Instance.WGSendToWeixin (title, desc, mediaTagName, imgData, imgData.Length, messageExt);
		}
		if (Button ("大图分享到好友", 3)) {
			Debug.Log ("大图分享到好友");
			WGPlatform.Instance.WGSendToWeixinWithPhoto(eWechatScene.WechatScene_Session, mediaTagName, imgData, imgData.Length, messageExt, messageAction);
		}
		if (Button ("大图分享到朋友圈", 3)) {
			Debug.Log ("大图分享到朋友圈");
			string SendToWeixinWithPhotoMsgAction = inputText;
            //方便测试
            if (String.IsNullOrEmpty(SendToWeixinWithPhotoMsgAction))
            {
                SendToWeixinWithPhotoMsgAction = "MESSAGE_ACTION_JUMP_H5_1#scene_id=1&p=1&q=1";

            }
            WGPlatform.Instance.WGSendToWeixinWithPhoto(eWechatScene.WechatScene_Timeline, mediaTagName, imgData, imgData.Length, messageExt, SendToWeixinWithPhotoMsgAction);
		}
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal();

        if (Button("视频分享到朋友圈", 2))
        {
            Debug.Log("视频分享到朋友圈");
            Debug.Log("eWechatScene.WechatScene_Timeline" + eWechatScene.WechatScene_Timeline + ",video_tile:" + video_tile + ",video_desc:" + video_desc + ",thumbUrl" + "," + videoUrl);
            WGPlatform.Instance.WGSendToWeixinWithVideo(eWechatScene.WechatScene_Timeline, video_tile, video_desc, thumbUrl, videoUrl, videoParams,
                video_mediaTagName, video_messageAction, messageExt);
        }
        if (Button("小程序分享好友", 2))
        {
            Debug.Log("小程序分享好友");
            WGPlatform.Instance.WGSendToWXWithMiniApp(eWechatScene.WechatScene_Session, title, desc, imgData, imgData.Length, targetUrl, miniapp_username, miniapp_path, true, messageExt, messageAction);
        }


        GUILayout.EndHorizontal();


        


		#if UNITY_ANDROID || UNITY_EDITOR
		GUILayout.BeginHorizontal ();
		if (Button ("分享本地图片到好友", 2))
		{
			Debug.Log ("分享本地图片到好友");
			WGPlatform.Instance.WGSendToWeixinWithPhotoPath(eWechatScene.WechatScene_Session, mediaTagName, imgLocalUrl, messageExt, messageAction);
		}
		if (Button ("分享本地图片到朋友圈", 2))
		{
			Debug.Log ("分享本地图片到朋友圈");
			WGPlatform.Instance.WGSendToWeixinWithPhotoPath(eWechatScene.WechatScene_Timeline, mediaTagName, imgLocalUrl, messageExt, messageAction);
		}
		GUILayout.EndHorizontal ();
		#endif

		GUILayout.BeginHorizontal ();
		if (Button ("分享音乐到好友", 3)) {
			Debug.Log ("分享音乐到好友");
			WGPlatform.Instance.WGSendToWeixinWithMusic(eWechatScene.WechatScene_Session, title, desc,musicUrl, musicDataUrl, mediaTagName, imgData, imgData.Length, messageExt, messageAction);
		}
		if (Button ("分享音乐到朋友圈", 3)) {
			Debug.Log ("分享音乐到朋友圈");
			WGPlatform.Instance.WGSendToWeixinWithMusic(eWechatScene.WechatScene_Timeline, title, desc,musicUrl, musicDataUrl, mediaTagName, imgData, imgData.Length, messageExt, messageAction);
		}
		if (Button ("后端分享", 3)) {
			Debug.Log ("后端分享");
			if (inputText == "") {
				message = "请在下面输入框中输入同玩好友的openid";
			} else {
				WGPlatform.Instance.WGSendToWXGameFriend( inputText, title, desc, mediaId, messageExt, mediaTagName, msdkExtInfo);
			}
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("Url分享到好友", 4)) {
			Debug.Log("Url分享到好友");
			WGPlatform.Instance.WGSendToWeixinWithUrl(eWechatScene.WechatScene_Session, title, desc, targetUrl, mediaTagName, imgData, imgData.Length, messageExt);
		}
		if (Button ("Url分享到朋友圈", 4)) {
			Debug.Log("Url分享到朋友圈");
			WGPlatform.Instance.WGSendToWeixinWithUrl(eWechatScene.WechatScene_Timeline, title, desc, targetUrl, mediaTagName, imgData, imgData.Length, messageExt);
		}
		if (Button ("分享到游戏中心", 4)) {
			Debug.Log("分享到游戏中心");
			LoginRet loginRet = WGPlatform.Instance.WGGetLoginRecord();
			if (loginRet == null || loginRet.open_id == "") {
				message = "分享出错，用户未登录";
            } else {
			    // 需要分享到的好友openid，demo设置为发送给自己
			    string send2Openid = loginRet.open_id;
			    TypeInfoLink msgLink = new TypeInfoLink();
			    msgLink.pictureUrl = "http://download.wegame.qq.com/wepang/RedGame_Winner_140.png";
			    msgLink.targetUrl = "http://www.qq.com";
			    ButtonApp buttonApp = new ButtonApp();
			    buttonApp.name = "拉起游戏";
			    buttonApp.messageExt = "messageExt";

			    WGPlatform.Instance.WGSendMessageToWechatGameCenter (send2Openid, title, desc, msgLink, buttonApp, msdkExtInfo);
            }
		}
        if (Button("分享微信游戏圈", 4))
        {
            Debug.Log("分享微信游戏圈");
			LoginRet loginRet = WGPlatform.Instance.WGGetLoginRecord();
			if (loginRet == null || loginRet.open_id == "") {
				message = "分享出错，用户未登录";
            } else {
                TextAsset imgBytes = Resources.Load<TextAsset>("tencentgame");
                Debug.Log("imgBytes.bytes:" + imgBytes.bytes.Length);
                WGPlatform.Instance.WGShareToWXGameline(imgBytes.bytes, "msdktest");
            }
		}
        
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("首页deeplink", 3)) {
			Debug.Log ("打开游戏中心首页deeplink");
			string link = "INDEX"; //游戏中心首页
			WGPlatform.Instance.WGOpenWeiXinDeeplink(link);
		}
		if (Button ("详情页deeplink", 3)) {
			Debug.Log ("打开游戏中心详情页deeplink");
			string link = "DETAIL"; //游戏中心详情页
			WGPlatform.Instance.WGOpenWeiXinDeeplink(link);
		}
		if (Button ("游戏库deeplink", 3)) {
			Debug.Log ("打开游戏中心游戏库deeplink");
			string link = "LIBRARY"; //游戏中心游戏库
			WGPlatform.Instance.WGOpenWeiXinDeeplink(link);
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("url页面deeplink", 3)) {
			Debug.Log ("打开跳转url页面deeplink");
			string link = "http://www.qq.com/"; //跳转url页面
			WGPlatform.Instance.WGOpenWeiXinDeeplink(link);
		}
		if (Button ("微信是否安装", 3)) {
			Debug.Log ("WX是否安装");
			message = (WGPlatform.Instance.WGIsPlatformInstalled(ePlatform.ePlatform_Weixin)).ToString();
		}
		#if UNITY_ANDROID
		if (Button ("获取微信版本", 3)) {
			Debug.Log ("获取微信版本");
			message = WGPlatform.Instance.WGGetPlatformAPPVersion(ePlatform.ePlatform_Weixin);
		}
		#endif
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("获取pf+pfkey", 3)) {
			Debug.Log ("获取pf+pfkey");
			message = WGPlatform.Instance.WGGetPf() + "\n" + WGPlatform.Instance.WGGetPfKey();

		}
		if (Button ("获取登录票据", 3)) {
			Debug.Log ("获取登录票据");
			LoginRet ret = WGPlatform.Instance.WGGetLoginRecord();
			if (ret != null) 	{
                message = ret.ToString();
			} else 	{
				message = "unity WGGetLoginRecord failed";
			}
		}
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        if (Button("创建微信群", 2)) {
            Debug.Log("创建微信群");
            if (inputText == "") {
                message = "请在输入框中输入公会ID";
            } else {
                string unionId = inputText;
                string chatRoomName = "MSDK-Unity公会";
                string chatRoomNickName = "用户的群昵称";
                WGPlatform.Instance.WGCreateWXGroup(unionId, chatRoomName, chatRoomNickName);
            }
        }
        if (Button("加入微信群", 2)) {
            Debug.Log("加入微信群");
            if (inputText == "") {
                message = "请在输入框中输入公会ID";
            } else {
                string unionId = inputText;
                string chatRoomNickName = "用户的群昵称";
                WGPlatform.Instance.WGJoinWXGroup(unionId, chatRoomNickName);
            }
        }
        GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
		if (Button("解绑群", 4)) {
			Debug.Log("微信解绑群");
			if (inputText == "") {
				message = "请在输入框中输入公会ID";
			} else {
				string unionId = inputText;
				WGPlatform.Instance.WGUnbindWeiXinGroup(unionId);
			}
		}
		if (Button("查询群状态", 4)) {
			Debug.Log("微信查询群状态");
			if (inputText == "") {
				message = "请在输入框中输入公会ID，查询类型";
			} else {
				string unionId = "";
				eStatusType opType = eStatusType.ISCREATED;
				string[] inputArr = inputText.Split(' ');
				if (inputArr != null && inputArr.Length == 2) {
					unionId = inputArr [0];
					opType = (eStatusType)Convert.ToInt32 (inputArr [1]);
                    Debug.Log("查询群，状态："+unionId+","+opType);
				} 
				WGPlatform.Instance.WGQueryWXGroupStatus(unionId,opType);
			}
		}
        if (Button("查询群成员信息", 4)) {
            Debug.Log("查询群成员信息");
            if (inputText == "") {
                message = "请在输入框中输入公会ID";
            } else {
                string unionId = inputText;
                string openIdList = "oGRTijiYT4CaRfydUbDFR25kAmwQ";
                WGPlatform.Instance.WGQueryWXGroupInfo(unionId, openIdList);
            }
        }
        if (Button("分享结构化消息到微信群", 4)) {
            Debug.Log("分享结构化消息到微信群");
            if (inputText == "") {
                message = "请在输入框中输入公会ID";
            } else {
                string unionId = inputText;
                WGPlatform.Instance.WGSendToWXGroup(1, 1, unionId, title, desc, messageExt, mediaTagName, imgNetUrl, msdkExtInfo);
            }
        }
        GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        Button(""); // 解决Demo运行时界面显示不全
        GUILayout.EndHorizontal ();
	}

	void ShowNotice()
	{

		GUILayout.BeginHorizontal ();
		if (Button ("展示公告", 3)) {
			Debug.Log ("展示公告");
			WGPlatform.Instance.WGShowNotice(inputText);
		}
		if (Button ("获取公告数据", 3)) {
			Debug.Log ("获取公告数据");
			List<NoticeInfo> notices = WGPlatform.Instance.WGGetNoticeData(inputText);
			if(notices == null){
				message = "获取公告数据失败！";
				return;
			}
			StringBuilder builder = new StringBuilder();
			for(int i = 0; i < notices.Count; i++){
				builder.AppendLine(notices[i].ToString()+";");
			}
			message = "获取到:"+notices.Count+"条数据;"+builder.ToString();
		}
		if (Button ("隐藏滚动公告", 3)) {
			Debug.Log ("隐藏滚动公告");
			WGPlatform.Instance.WGHideScrollNotice();
		}
		GUILayout.EndHorizontal ();

		#if UNITY_IPHONE
		GUILayout.BeginHorizontal ();
		if (Button("游客登录", 3)) {
			Debug.Log ("点击登录游客");
			WGPlatform.Instance.WGLogin (ePlatform.ePlatform_Guest);
		}
		if (Button("重置游客账号", 3)) {
			Debug.Log ("重置游客账号");
			WGPlatform.Instance.WGResetGuestID ();
			loginState = "未登录";
			loginStateMaybeChange = true;
		}
		if (Button ("登出", 3)) {
			Debug.Log ("登出");
			WGPlatform.Instance.WGLogout ();
			loginStateMaybeChange = true;
			loginState = "未登录";
			message = " ";
		}
		GUILayout.EndHorizontal ();
		#endif
	}

	void ShowOthers()
	{
		GUILayout.BeginHorizontal ();
		if (Button ("打开浏览器", 4)) {
			Debug.Log ("打开内置浏览器");
			if (inputText == "") {
				message = "请在下面输入框中输入要打开的网址";
			} else {
				WGPlatform.Instance.WGOpenUrl(inputText);
			}
		}
		if (Button ("横屏打开", 4)) {
			Debug.Log ("横屏打开内置浏览器");
			if (inputText == "") {
				message = "请在下面输入框中输入要打开的网址";
			} else {
				WGPlatform.Instance.WGOpenUrl(inputText, eMSDK_SCREENDIR.eMSDK_SCREENDIR_LANDSCAPE);
			}
		}
		if (Button ("竖屏打开", 4)) {
			Debug.Log ("竖屏打开内置浏览器");
			if (inputText == "") {
				message = "请在下面输入框中输入要打开的网址";
			} else {
				WGPlatform.Instance.WGOpenUrl(inputText, eMSDK_SCREENDIR.eMSDK_SCREENDIR_PORTRAIT);
			}
		}
        if (Button("浏览器js接口", 4))
        {
            Debug.Log("浏览器js接口"); 
            String demoUrl = "https://img.ssl.msdk.qq.com/wiki/test/msdkjs.html";
            WGPlatform.Instance.WGOpenUrl(demoUrl, eMSDK_SCREENDIR.eMSDK_SCREENDIR_PORTRAIT);
        }
        
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("自定义数据上报", 3)) {
			Debug.Log ("自定义数据上报");
			string name = "ReportEventTest";
			Dictionary<string, string> data = new Dictionary<string, string>();
			data.Add("key1", "values1");
			data.Add("key2", "values2");
			WGPlatform.Instance.WGReportEvent(name, data, true);
		}
		if (Button ("自动登录", 3)) {
			Debug.Log ("自动登录");
			WGPlatform.Instance.WGLoginWithLocalInfo();
		}
		if (Button ("反馈上报", 3)) {
			Debug.Log ("反馈上报");
			WGPlatform.Instance.WGFeedback(inputText);
		}
		GUILayout.EndHorizontal ();

		#if UNITY_ANDROID
		GUILayout.BeginHorizontal ();

		if (Button ("检查接口是否支持(需要输入接口ID）", 3f)) {
			Debug.Log ("检查接口是否支持");
            try {
                int api = int.Parse(inputText);
                bool result = WGPlatform.Instance.WGCheckApiSupport((eApiName)api);
                message = result.ToString();
            } catch (Exception e) {
                Debug.LogError(e.Message);
            }
		}
		GUILayout.EndHorizontal ();
		#endif

		GUILayout.BeginHorizontal ();
		if (Button ("获取MSDK版本", 3)) {
			Debug.Log ("获取MSDK版本");
			message = WGPlatform.Instance.WGGetVersion();
		}
		if (Button ("获取安装渠道", 3)) {
			Debug.Log ("获取安装渠道");
			message = WGPlatform.Instance.WGGetChannelId();
		}
		if (Button ("获取注册渠道", 3)) {
			Debug.Log ("获取注册渠道");
			message = WGPlatform.Instance.WGGetRegisterChannelId();
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("附近的人", 3)) {
			if (isClickable == true) {
				isClickable = false;
				Debug.Log ("附近的人");
                message = "正在等待网络请求返回回调";
                WGPlatform.Instance.WGGetNearbyPersonInfo();
				Invoke ("letClickable", 0.8F);
			}
		}
		if (Button ("获取玩家位置", 3)) {
			if (isClickable == true) {
				isClickable = false;
				Debug.Log ("获取玩家位置");
                message = "正在等待网络请求返回回调";
                WGPlatform.Instance.WGGetLocationInfo ();
				Invoke ("letClickable", 0.8F);
			}
		}

		if (Button ("清空位置信息", 3)) 	{
			if (isClickable == true) {
				isClickable = false;
				Debug.Log ("清空位置信息");
				WGPlatform.Instance.WGCleanLocation();
				Invoke ("letClickable", 0.8F);
			}
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
#if UNITY_ANDROID
		if (Button ("native异常", 3)) {
			Debug.Log ("native异常上报测试");
			nativeCrashTest();
		}
		if (Button ("java异常", 3)) {
			Debug.Log ("空指针异常上报测试");
			javaCrashTest();
		}
#elif UNITY_IPHONE
		if (Button ("oc异常", 3)) {
			Debug.Log ("objective-c异常上报测试");
			iOSCrashTest();
		}
#endif
		if (Button ("C#异常", 3))
		{
			Debug.Log ("C#层异常上报测试");
			cSharpMSDKCrashTest();
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();

		#if UNITY_ANDROID || UNITY_EDITOR
		if (Button ("应用宝是否安装", 3)) {
			Debug.Log ("应用宝是否安装");
			int ret = WGPlatform.Instance.WGCheckYYBInstalled();
			switch (ret)
			{
            case -1:
                message = ret + " ： 应用宝省流量开关未打开";
                break;
			case 0:
				message = ret + " ： 应用宝已安装";
				break;
			case 1:
				message = ret + " ： 应用宝未安装";
				break;
			default:
				message = ret + " ： 安装了低版本的应用宝";
				break;
			}
		}
		if (Button ("检查游戏更新", 3))
		{
			Debug.Log ("检查游戏更新");
			WGPlatform.Instance.WGCheckNeedUpdate();
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		if (Button ("拉起应用宝省流量更新", 2))
		{
			Debug.Log ("拉起应用宝省流量更新");
			WGPlatform.Instance.WGStartSaveUpdate(true);
		}
		if (Button ("直接在应用内省流量更新", 2))
		{
			Debug.Log ("直接在应用内省流量更新");
			WGPlatform.Instance.WGStartSaveUpdate(false);
		}
		#endif
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        if (Button ("设置自定义标签", 2)) {
            Debug.Log ("设置自定义标签");
            if (inputText == "") {
                message = "请在下面输入框中输入标签";
            } else {
                WGPlatform.Instance.WGSetPushTag(inputText);
            }
        }
        if (Button ("删除自定义标签", 2)) {
            Debug.Log ("删除自定义标签");
            if (inputText == "") {
                message = "请在下面输入框中输入标签";
            } else {
                WGPlatform.Instance.WGDeletePushTag(inputText);
            }
        }
        GUILayout.EndHorizontal ();

		#if UNITY_ANDROID
		GUILayout.BeginHorizontal ();
		if (Button ("添加本地推送", 2)) {
			Debug.Log ("添加本地推送");
			string[] times = Regex.Split (inputText, " ");
			if (inputText == "" || times.Length != 3) {
				message = "请在下面输入框中输入推送时间，格式为yyyyMMdd HH mm";
			} else {
				LocalMessageAndroid msg = new LocalMessageAndroid();
				msg.type = 1;
				msg.action_type = 1;
				msg.content = "信鸽本地推送测试";
				msg.title = "您有新的消息";
				msg.date = times[0];
				msg.hour = times[1];
				msg.min = times[2];
				WGPlatform.Instance.WGAddLocalNotification(msg);
			}
		}
		if (Button ("取消本地推送", 2)) {
			Debug.Log ("取消本地推送");
			WGPlatform.Instance.WGClearLocalNotifications();
		}
		GUILayout.EndHorizontal ();
		#endif

		#if UNITY_IPHONE
		GUILayout.BeginHorizontal ();
		if (Button ("添加本地推送", 3)) {
			Debug.Log ("添加本地推送");
			if (inputText == "") {
				message = "请在下面输入框中输入推送时间，格式为yyyy-MM-dd HH:mm:ss";
			} else {
				LocalMessageIOS msg = new LocalMessageIOS();
				msg.fireDate = inputText;
				msg.alertBody = "Local Notification";
				msg.badge = 1;
				WGPlatform.Instance.WGAddLocalNotification(msg);
			}
		}
		if (Button ("添加前台推送", 3)) {
			Debug.Log ("添加前台推送");
			LocalMessageIOS msg = new LocalMessageIOS();
			msg.alertBody = "Local Notification At Front";
			msg.userInfoKey = "FrontKey";
			msg.userInfoValue = "FrontValue";
			WGPlatform.Instance.WGAddLocalNotificationAtFront(msg);
		}
		if (Button ("取消推送任务", 3)) {
			Debug.Log ("取消推送任务");
			WGPlatform.Instance.WGClearLocalNotifications();
		}
		GUILayout.EndHorizontal ();
		#endif

		GUILayout.BeginHorizontal ();
		if (Button("bugly日志上报", 3)) {
			// 使用此接口打印的日志会在Crash时上报到 bugly.qq.com 的“自定义日志”处
			Debug.Log ("bugly日志上报");
			WGPlatform.Instance.WGBuglyLog(eBuglyLogLevel.eBuglyLogLevel_E, "bugly message test");
		}
        if (Button("Url添加加密票据", 3)) {
            // 使用此接口打印的日志会在Crash时上报到 bugly.qq.com 的“自定义日志”处
            Debug.Log("Url添加加密票据");
            message = WGPlatform.Instance.WGGetEncodeUrl("http://www.qq.com");
        }
		GUILayout.EndHorizontal ();

        GUILayout.BeginHorizontal ();
        Button(""); // 解决Demo在运行时界面显示不全
        GUILayout.EndHorizontal ();
	}
	#endregion


#if UNITY_ANDROID
	// 调用Android方法
    // TODO Game 如果游戏需要调用示例方法，需要将 className 改为自己包名路径下的MGameActivity,如 com.xxx.xxx.MGameActivity
	private static readonly string className = "com.example.wegame.MGameActivity";
	AndroidJavaClass GetUnityPlayer()
	{
		return new AndroidJavaClass (className);;
	}

	/**
     * 拉起支付Demo,需要先安装支付示例工程AndroidPaySample(位于MSDKzip包中的Tencent AndroidPayRelease.zip中)
     * 才能拉起支付Demo
     */
	void launchPayDemo()
	{
		GetUnityPlayer ().CallStatic ("launchPayDemo");
	}

	/**
     * 造native层崩溃测试异常上报,游戏崩溃后会将堆栈信息上报到bugly
     */
	void nativeCrashTest()
	{
		GetUnityPlayer ().CallStatic ("nativeCrashTest");
	}

	/**
     * 制造空指针异常测试异常上报,游戏崩溃后会将堆栈信息上报到腾bugly中
     */
	void javaCrashTest()
	{
		GetUnityPlayer ().CallStatic ("nullPointerExceptionTest");
	}
#endif

#if UNITY_IPHONE
	// 调用iOS方法
	[DllImport("__Internal")]
	public static extern void iOSCrashTest ();
#endif

	/**
	 * 测试C#层崩溃上报
	 */
	void cSharpMSDKCrashTest()
	{
        Debug.Log("cSharpMSDKCrashTest");
        int[] array = new int[2];
		array [3] = 0;
	}
    /**
     * 模仿服务器算md5
     * */
    public static string md5String(string s)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(s);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
	#region MSDKDemo 界面实现相关
	private int defaultButtonHeight = Screen.height / 12;
	// label:按钮名称  widthScale:按钮宽度为最大maxWidth宽度除以widthScale  buttonHeight:按钮的高度 maxWidth:最大宽度
	private bool Button(string label, int maxWidth, float widthScale, int buttonHeight)
	{

		if (GUILayout.Button (label, buttonStyle,GUILayout.MinHeight (buttonHeight), GUILayout.MaxWidth (maxWidth / widthScale)))
		{
			message = ""; // 每次点击按钮后消息清空
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool Button(string label, float widthScale, int buttonHeight)
	{
		return Button (label, Screen.width, widthScale, buttonHeight);
	}

	private bool Button(string label, float widthScale)
	{
		return Button (label, widthScale, defaultButtonHeight);
	}

	private bool Button(string label)
	{
		return Button (label, 1);
	}

	private void LabelAndTextField(string label, ref string text, float height)
	{
		GUILayout.BeginVertical ();
		GUILayout.Label(label, GUILayout.MaxWidth(Screen.width));
		text = GUILayout.TextField(text,  GUILayout.Height(height));
		GUILayout.EndVertical ();
	}

	private void PopWindow(int id, int width, int height, GUI.WindowFunction func)
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = 18;

		Rect windowRect = new Rect ((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
		windowRect = GUI.Window(id, windowRect, func, "", style);
	}

    private void showMessageWithEmoji(string msg) {
        ShowOffEmoji s = (ShowOffEmoji)emojiPanel.GetComponent("ShowOffEmoji");
        s.gameObject.SetActive(true);
        MsdkDemo.isShow = false;
        s.SetEmoji(msg);
    }
	#endregion

}
