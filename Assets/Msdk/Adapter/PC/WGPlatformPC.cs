#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Msdk;

public class WGPlatformPC : WGPlatformUnity, IMsdk
{
    private DataGenerator dataGenerator = DataGenerator.Instance;

	// MSDK Unity 初始化
	public new void Init() 
	{
		base.Init();
	}

	public void WGLogin (ePlatform platform)
	{
        dataGenerator.LoginData.flag = (int)eFlag.eFlag_Succ;
        if (platform == ePlatform.ePlatform_QQ) {
            dataGenerator.LoginData.platform = (int)ePlatform.ePlatform_QQ;
            dataGenerator.LoginData.desc = ":-) , QQ授权成功 cb: content: success statusCode: 200";
        } else {
            dataGenerator.LoginData.platform = (int)ePlatform.ePlatform_Weixin;
            dataGenerator.LoginData.desc = ":-) , 微信授权成功 cb: content: success statusCode: 200";
        }
        MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
	}

	public int WGLoginOpt (ePlatform platform,int overtime)
	{
		dataGenerator.LoginData.flag = (int)eFlag.eFlag_Succ;
		if (platform == ePlatform.ePlatform_QQ) {
			dataGenerator.LoginData.platform = (int)ePlatform.ePlatform_QQ;
			dataGenerator.LoginData.desc = ":-) , QQ授权成功 cb: content: success statusCode: 200";
		} else {
			dataGenerator.LoginData.platform = (int)ePlatform.ePlatform_Weixin;
			dataGenerator.LoginData.desc = ":-) , 微信授权成功 cb: content: success statusCode: 200";
		}
		MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
		return eFlag.eFlag_Succ;
	}

	public void WGQrCodeLogin (ePlatform platform)
	{
        dataGenerator.LoginData.flag = (int)eFlag.eFlag_Succ;
        dataGenerator.LoginData.platform = (int)ePlatform.ePlatform_Weixin;
        dataGenerator.LoginData.desc = ":-) , 微信授权成功 cb: content: success statusCode: 200";
        MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
	}

	public bool WGLogout ()
	{
        dataGenerator.LoginData.flag = (int)eFlag.eFlag_Error;
		return true;
	}

	public LoginRet WGGetLoginRecord ()
	{
        if (dataGenerator.LoginData.flag == (int)eFlag.eFlag_Succ) {
            return dataGenerator.LoginData;
        } else {
            return new LoginRet();
        }
	}

	public bool WGSwitchUser (bool flag)
	{
        MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
        return true;
	}

	public void WGSetPermission (int permissions)
	{
	}

	public void WGRefreshWXToken ()
	{
        MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
	}

    public void WGRealNameAuth(RealNameAuthInfo info)
    {
        MsdkEvent.Instance.HandleRealNameAuthNotify(dataGenerator.AuthData);
    }

	public void WGSendToQQWithMusic (eQQScene scene, string title, string desc, string musicUrl, string musicDataUrl, string imgUrl)
	{
		MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public bool WGSendToQQGameFriend (int act, string fopenid, string title, string summary, string targetUrl, string imgUrl, string previewText, string gameTag, string msdkExtInfo)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
        return true;
	}

	public bool WGQueryQQMyInfo ()
	{
        if (dataGenerator.LoginData.flag == eFlag.eFlag_Succ) {
            dataGenerator.RelationData.flag = eFlag.eFlag_Succ;
            dataGenerator.RelationData.desc = "查询成功";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        } else {
            dataGenerator.RelationData.flag = eFlag.eFlag_Error;
            dataGenerator.RelationData.desc = "查询失败";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        }
        return true;
	}

	public bool WGQueryQQGameFriendsInfo ()
	{
        if (dataGenerator.LoginData.flag == eFlag.eFlag_Succ) {
            dataGenerator.RelationData.flag = eFlag.eFlag_Succ;
            dataGenerator.RelationData.desc = "查询成功";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        } else {
            dataGenerator.RelationData.flag = eFlag.eFlag_Error;
            dataGenerator.RelationData.desc = "查询失败";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        }
        return true;
	}

	public void WGBindQQGroup (string unionid, string union_name, string zoneid, string signature)
	{
		MsdkEvent.Instance.HandleBindGroupNotify(dataGenerator.GroupData);
	}

    public void WGCreateQQGroupV2(GameGuild gameGuild)
	{
		MsdkEvent.Instance.HandleCreateGroupV2Notify(dataGenerator.GroupData);
	}

    public void WGJoinQQGroupV2(GameGuild gameGuild, string groupId)
    {
        MsdkEvent.Instance.HandleJoinGroupV2Notify(dataGenerator.GroupData);
    }

    public void WGBindExistQQGroupV2(GameGuild gameGuild, string groupId, string groupName)
    {
        MsdkEvent.Instance.HandleBindExistGroupV2Notify(dataGenerator.GroupData);
    }
    public void WGGetQQGroupCodeV2(GameGuild gameGuild)
    {
        MsdkEvent.Instance.HandleGetGroupCodeV2Notify(dataGenerator.GroupData);
    }

    public void WGQueryBindGuildV2(string groupId, int type = 0)
    {
        MsdkEvent.Instance.HandleQueryBindGuildV2Notify(dataGenerator.GroupData);
    }

    public void WGGetQQGroupListV2()
    {
        MsdkEvent.Instance.HandleGetGroupListV2Notify(dataGenerator.GroupData);
    }

    public void WGRemindGuildLeaderV2(GameGuild gameGuild)
    {
        MsdkEvent.Instance.HandleRemindGuildLeaderV2Notify(dataGenerator.GroupData);
    }

	public void WGAddGameFriendToQQ (string fopenid, string desc, string message)
	{
	}

	public void WGSendToWeixin (string title, string desc, string mediaTagName, byte[] thumbImgData, int thumbImgDataLen, string messageExt)
	{
		MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToWeixinWithUrl (eWechatScene scene, string title, string desc, string url, string mediaTagName, byte[] thumbImgData, int thumbImgDataLen, string messageExt)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToWeixinWithPhoto (eWechatScene scene, string mediaTagName, byte[] imgData, int imgDataLen, string messageExt, string messageAction)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToWeixinWithMusic (eWechatScene scene, string title, string desc, string musicUrl, string musicDataUrl, string mediaTagName, byte[] imgData, int imgDataLen, string messageExt, string messageAction)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public bool WGSendMessageToWechatGameCenter (string fOpenid, string title, string content, WXMessageTypeInfo pInfo, WXMessageButton pButton, string msdkExtInfo)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
        return true;
	}

	public bool WGSendToWXGameFriend (string fOpenId, string title, string description, string mediaId, string messageExt, string mediaTagName, string msdkExtInfo)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
        return true;
	}

	public bool WGQueryWXMyInfo ()
	{
        if (dataGenerator.LoginData.flag == eFlag.eFlag_Succ) {
            dataGenerator.RelationData.flag = eFlag.eFlag_Succ;
            dataGenerator.RelationData.desc = "查询成功";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        } else {
            dataGenerator.RelationData.flag = eFlag.eFlag_Error;
            dataGenerator.RelationData.desc = "查询失败";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        }
        return true;
	}

	public bool WGQueryWXGameFriendsInfo ()
	{
        if (dataGenerator.LoginData.flag == eFlag.eFlag_Succ) {
            dataGenerator.RelationData.flag = eFlag.eFlag_Succ;
            dataGenerator.RelationData.desc = "查询成功";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        } else {
            dataGenerator.RelationData.flag = eFlag.eFlag_Error;
            dataGenerator.RelationData.desc = "查询失败";
            MsdkEvent.Instance.HandleRelationNofify(dataGenerator.RelationData);
        }
        return true;
	}

	public void WGQueryWXGroupStatus (string unionid, eStatusType opType)
	{
		if (dataGenerator.LoginData.flag == eFlag.eFlag_Succ) {
			dataGenerator.GroupData.flag = eFlag.eFlag_Succ;
			if (opType == eStatusType.ISCREATED) {
				dataGenerator.GroupData.desc = "已经邦定微信群";
			} else if (opType == eStatusType.ISJOINED) {
				dataGenerator.GroupData.desc = "已经加入微信群";
			}
			MsdkEvent.Instance.HandleQueryWXGroupStatusNotify(dataGenerator.GroupData);
		} else {
			dataGenerator.GroupData.flag = eFlag.eFlag_Error;
			dataGenerator.GroupData.desc = "没有登陆，查询失败";
			MsdkEvent.Instance.HandleQueryWXGroupStatusNotify(dataGenerator.GroupData);
		}

	}

    public void WGCreateQQGroupV2(
           string guildId,
           string guildName,
           string zoneId,
           string roleId,
           string partition
           )
    {
        MsdkEvent.Instance.HandleBindGroupNotify(dataGenerator.GroupData);
    }

    public void WGJoinQQGroupV2(
        string guildId,
        string zoneId,
        string groupId,
        string roleId,
        string partition)
    {
        MsdkEvent.Instance.HandleJoinQQGroupNotify(dataGenerator.GroupData);
    }


    public void WGUnbindQQGroupV2(GameGuild gameGuild)
    {
        MsdkEvent.Instance.HandleUnbindGroupNotify(dataGenerator.GroupData);
    }

    public void WGQueryQQGroupInfoV2( string groupId)
    {
        MsdkEvent.Instance.HandleQueryGroupInfoNotify(dataGenerator.GroupData);
    }

    public void WGShareToWXGameline(byte[] data, string gameExtra)
    {
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
    }

	public void WGCreateWXGroup (string unionid, string chatRoomName, string chatRoomNickName)
	{
		MsdkEvent.Instance.HandleCreateWXGroupNotify(dataGenerator.GroupData);
	}

	public void WGJoinWXGroup (string unionid, string chatRoomNickName)
	{
		MsdkEvent.Instance.HandleJoinWXGroupNotify(dataGenerator.GroupData);
	}

	public void WGQueryWXGroupInfo (string unionid, string openIdList)
	{
		MsdkEvent.Instance.HandleQueryGroupInfoNotify(dataGenerator.GroupData);
	}

	public void WGSendToWXGroup (int msgType, int subType, string unionid, string title, string description, string messageExt, string mediaTagName, string imgUrl, string msdkExtInfo)
	{
		MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGUnbindWeiXinGroup(string groupId)
	{
		MsdkEvent.Instance.HandleQueryGroupInfoNotify(dataGenerator.GroupData);
	}

	public void WGOpenWeiXinDeeplink (string link)
	{
	}

	public void WGFeedback (string body)
	{
        MsdkEvent.Instance.HandleFeedbackNotify(0, "statusCode: 200; errorContent: {\"ret\":0,\"msg\":\"sucess\"}");
	}

	public void WGEnableCrashReport (bool bRDMEnable, bool bMTAEnable)
	{
	}

	public void WGReportEvent (string name, System.Collections.Generic.Dictionary<string, string> eventList, bool isRealTime)
	{
	}

	public string WGGetVersion ()
	{
		return "2.14.xa";
	}

	public string WGGetChannelId ()
	{
		return "0000000";
	}

	public string WGGetPlatformAPPVersion (ePlatform platform)
	{
		return "5.9.5";
	}

	public string WGGetRegisterChannelId ()
	{
        return "0000000";
	}

	public bool WGIsPlatformInstalled (ePlatform platformType)
	{
		return true;
	}

	public string WGGetPfKey ()
	{
        return "e9638c4ea7295ae02ebf1f529239bd65";
	}

	public string WGGetPf ()
	{
        return "desktop_m_qq-73213123-android-00000000-qq-100703379-D51F963BA3E2571ABD8244D95F9B9AD0";
	}

	public void WGLoginWithLocalInfo ()
	{
		MsdkEvent.Instance.HandleLoginNotify(dataGenerator.LoginData);
	}

	public void WGShowNotice (string scene)
	{
	}

	public void WGHideScrollNotice ()
	{
	}

	public void WGOpenUrl (string openUrl)
	{
        MsdkEvent.Instance.HandleWebviewNotify(dataGenerator.openwebviewRet);
	}

	public void WGOpenUrl (string openUrl, eMSDK_SCREENDIR screendir)
	{
        MsdkEvent.Instance.HandleWebviewNotify(dataGenerator.openwebviewRet);
	}

	public string WGGetEncodeUrl (string openUrl)
	{
        return openUrl + "?key=vaule&test=test";
	}

	public List<NoticeInfo> WGGetNoticeData (string scene)
	{
        string jsonStr = "{\"_notice_list\":[{\"_end_time\":\"1459252800\",\"_picArray\":[],\"_content_url\":\"\", " + 
            "\"_msg_type\":0,\"_start_time\":\"1456150200\",\"_content_type\":0,\"_open_id\":\"\",\"_msg_content\":\"广告测试\", " +
            "\"_update_time\":\"\",\"_msg_url\":\"\",\"_msg_id\":\"17838\",\"_msg_title\":\"test3\",\"_msg_scene\":\"1\"}]}";
        List<NoticeInfo> notices = NoticeInfoList.ParseJson(jsonStr);
        return notices;
	}

	public bool WGOpenAmsCenter (string parameters)
	{
        return true;
	}

	public void WGGetNearbyPersonInfo ()
	{
		MsdkEvent.Instance.HandleLocationNotify(dataGenerator.RelationData);
	}

	public bool WGCleanLocation ()
	{
        MsdkEvent.Instance.HandleLocationNotify(dataGenerator.cleanLocationData);
        return true;
	}

	public bool WGGetLocationInfo ()
	{
		MsdkEvent.Instance.HandleLocationGotNotify(dataGenerator.LocationData);
        return true;
	}

	public void WGStartGameStatus (string cGameStatus)
	{
	}

	public void WGEndGameStatus (string cGameStatus, int succ, int errorCode)
	{
	}

	public void WGShowAD (eADType adType)
	{
	}

	public void WGCloseAD (eADType adType)
	{
	}

	public void WGSetPushTag (string tag)
	{
	}

	public void WGDeletePushTag (string tag)
	{
	}

	public void WGClearLocalNotifications ()
	{
	}

	public void WGBuglyLog (eBuglyLogLevel level, string log)
	{
	}

	public void WGJoinQQGroup (string qqGroupKey)
	{
	}

    public void WGJoinQQGroup(string groupNum, string groupKey)
    {
    }

	public void WGQueryQQGroupInfo (string cUnionid, string cZoneid)
	{
		MsdkEvent.Instance.HandleQueryGroupInfoNotify(dataGenerator.GroupData);
	}

	public void WGUnbindQQGroup (string cGroupOpenid, string cUnionid)
	{
		MsdkEvent.Instance.HandleUnbindGroupNotify(dataGenerator.GroupData);
	}

	public void WGQueryQQGroupKey (string cGroupOpenid)
	{
		MsdkEvent.Instance.HandleQueryGroupKeyNotify(dataGenerator.GroupData);
	}

	public void WGAddCardToWXCardPackage (string cardId, string timestamp, string sign)
	{
		MsdkEvent.Instance.HandleAddWXCardNotify(dataGenerator.CardData);
	}

	public void WGSendToWeixinWithPhotoPath (eWechatScene scene, string mediaTagName, string imgPath, string messageExt, string messageAction)
	{
		MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToQQ (eQQScene scene, string title, string desc, string url, string imgUrl, int imgUrlLen)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToQQWithPhoto (eQQScene scene, string imgFilePath)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToQQWithRichPhoto (string summary, ArrayList imgs)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

    public void WGSendToQQWithPhoto(eQQScene scene, ImageParams imageParams, string extraScene, string messageExt)
    {
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
    }
	public void WGSendToQQWithVideo (string summary, string videoPath)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}
    public void WGSendToWXWithMiniApp(eWechatScene scene, string title, string desc, byte[] thumbImgData,int thumbImgDataLen, string webpageUrl, string userName, string path, bool withShareTicket, string messageExt, string messageAction)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}
    public void WGSendToWeixinWithVideo(eWechatScene scene, string title, string desc, string thumbUrl, string videoUrl, VideoParams videoParams, string mediaTagName, string messageAction, string messageExt)
    {
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
    }
	public bool WGCheckApiSupport (eApiName apiName)
	{
        return true;
	}

	public void WGStartSaveUpdate (bool isUseYYB)
	{
        MsdkEvent.Instance.HandleDownloadAppProgressChanged(1115662, 5396874);
        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
        timer.Interval = 1200;
        timer.Enabled = true;
    }

    private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) 
    {
        MsdkEvent.Instance.HandleDownloadAppStateChanged(100, 0, "SelfUpdate success !");
    }


	public void WGCheckNeedUpdate ()
	{
        MsdkEvent.Instance.HandleCheckNeedUpdateInfo(5396874, "测试应用宝省流量更新", 0, 0, 
                "http://dd.myapp.com/16891/FE9EFF9EE3B66D21285145AB2409D8EC.apk?fsname=com.example.wegame_2.7.1.myapp_270000.apk&__k1__=y",1);
	}

	public int WGCheckYYBInstalled ()
	{
		return TMYYBInstallState.ALREADY_INSTALLED;
	}

	public long WGAddLocalNotification (LocalMessageAndroid msg)
	{
        return 0;
	}

	public void WGSendToQQ (eQQScene scene, string title, string desc, string url, byte[] imgData, int imgDataLen)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}

	public void WGSendToQQWithPhoto (eQQScene scene, byte[] imgData, int imgDataLen)
	{
        MsdkEvent.Instance.HandleShareNofify(dataGenerator.ShareData);
	}
		

	public int WGGetPaytokenValidTime ()
	{
        return 1465110708;
	}
		

	public long WGAddLocalNotification (LocalMessageIOS localMessage)
	{
        return 0;
	}

	public long WGAddLocalNotificationAtFront (LocalMessageIOS localMessage)
	{
        return 0;
	}

	public void WGClearLocalNotification (LocalMessageIOS localMessage)
	{
	}

	public void WGOpenMSDKLog (bool enabled)
	{
	}

	public string WGGetGuestID ()
	{
        return "G_D51F963BA3E2571ABD8244D95F9B9AD0";
	}

	public void WGResetGuestID ()
	{
	}
}

#endif