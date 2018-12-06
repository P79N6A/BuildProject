#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using Msdk;

namespace Msdk
{

	public class WGPlatformIOS : WGPlatformUnity, IMsdk
	{

		// MSDK Unity 初始化
		public new void Init ()
		{
			base.Init();
            // 注册C++回调
            iOSConnector.setBridge(MessageCenter.MessageConsumer);
			reportData ();
		}

		#region 登录相关
		public void WGLogin (ePlatform platform)
		{
			iOSConnector.Login ((int) platform);
		}
		public int WGLoginOpt(ePlatform platform, int overtime)
		{
			return iOSConnector.LoginOpt ((int)platform, overtime);
		}
		public void WGQrCodeLogin (ePlatform platform)
		{
			iOSConnector.WGQrCodeLogin ((int) platform);
		}

		public bool WGLogout ()
		{
			return iOSConnector.Logout ();
		}

		public LoginRet WGGetLoginRecord ()
		{
			string loginRetStr = iOSConnector.GetLoginRecord();
			if (loginRetStr != null)
			{
				return LoginRet.ParseJson(loginRetStr);
			}
			else
			{
				return new LoginRet();
			}

		}

		public bool WGSwitchUser (bool flag)
		{
			return iOSConnector.SwitchUser (flag);
		}

		public void WGSetPermission (int permissions)
		{
			iOSConnector.SetPermission (permissions);
		}

		public void WGRefreshWXToken ()
		{
			iOSConnector.RefreshWXToken ();
		}

        public void WGRealNameAuth(RealNameAuthInfo info)
        {
            string authInfoJsonStr = JsonMapper.ToJson(info);
            iOSConnector.RealNameAuth(authInfoJsonStr);
        }
		#endregion

		#region QQ相关
		public void WGSendToQQ (eQQScene scene, string title, string desc, string url, byte[] imgData, int imgDataLen)
		{
			iOSConnector.SendToQQ ((int)scene, title, desc, url, imgData, imgDataLen);
		}

		public void WGSendToQQWithPhoto (eQQScene scene, byte[] imgData, int imgDataLen)
		{
			iOSConnector.SendToQQWithPhoto ((int)scene, imgData, imgDataLen);
		}

		public void WGSendToQQWithMusic (eQQScene scene, string title, string desc, string musicUrl, string musicDataUrl, string imgUrl)
		{
			iOSConnector.SendToQQWithMusic((int) scene, title, desc, musicUrl, musicDataUrl, imgUrl);
		}

		public bool WGSendToQQGameFriend (int act, string fopenid, string title, string summary, string targetUrl, string imgUrl, string previewText, string gameTag, string msdkExtInfo)
		{
			return iOSConnector.SendToQQGameFriend (act, fopenid, title, summary, targetUrl, imgUrl, previewText, gameTag, msdkExtInfo);
		}

		public bool WGQueryQQMyInfo ()
		{
			return iOSConnector.QueryQQMyInfo ();
		}

		public bool WGQueryQQGameFriendsInfo ()
		{
			return iOSConnector.QueryQQGameFriendsInfo ();
		}

		public void WGBindQQGroup (string unionid, string union_name, string zoneid, string signature)
		{
			iOSConnector.BindQQGroup (unionid, union_name, zoneid, signature);
		}

        public void WGJoinQQGroup(string groupNum, string groupKey)
        {
            iOSConnector.JoinQQGroup (groupNum, groupKey);
        }

		public void WGAddGameFriendToQQ (string fopenid, string desc, string message)
		{
			iOSConnector.AddGameFriendToQQ (fopenid, desc, message);
		}
		public void WGCreateQQGroupV2(GameGuild gameGuild)
		{
			iOSConnector.CreateQQGroupV2(gameGuild.guildId, gameGuild.guildName, gameGuild.zoneId, gameGuild.roleId, gameGuild.partition);
		}

		public void WGJoinQQGroupV2(GameGuild gameGuild,string groupId)
		{
			iOSConnector.JoinQQGroupV2(gameGuild.guildId, gameGuild.zoneId, groupId, gameGuild.roleId, gameGuild.partition);
		}


		public void WGUnbindQQGroupV2(GameGuild gameGuild)
		{
			iOSConnector.UnbindQQGroupV2(gameGuild.guildId, gameGuild.guildName, gameGuild.zoneId);
		}

		public void WGQueryQQGroupInfoV2(string groupId)
		{
			iOSConnector.QueryQQGroupInfoV2(groupId);
		}

		public void WGQueryQQGroupKey(string groupOpenId)
		{
			iOSConnector.QueryQQGroupKey(groupOpenId);
		}

		public void WGBindExistQQGroupV2(GameGuild gameGuild, string groupId, string groupName)
		{
			iOSConnector.BindExistQQGroupV2(groupId, groupName, gameGuild.guildId, gameGuild.zoneId,gameGuild.roleId);
		}

		public void WGGetQQGroupCodeV2(GameGuild gameGuild)
		{
			iOSConnector.GetQQGroupCodeV2(gameGuild.guildId,gameGuild.zoneId);
		}

		public void WGQueryBindGuildV2(string groupId, int type = 0)
		{
			iOSConnector.QueryBindGuildV2(groupId, type);
		}

		public void WGGetQQGroupListV2()
		{
			iOSConnector.GetQQGroupListV2();
		}
		public void WGRemindGuildLeaderV2(GameGuild gameGuild)
		{
			iOSConnector.RemindGuildLeaderV2(gameGuild.guildId,gameGuild.zoneId, gameGuild.roleId, gameGuild.roleName,
				gameGuild.partition,gameGuild.userZoneId, gameGuild.type, gameGuild.leaderOpenId,
				gameGuild.leaderRoleId, gameGuild.leaderZoneId, gameGuild.areaId);
		}

        #endregion

#region 微信相关
		public void WGSendToWeixin (string title, string desc, string mediaTagName, byte[] thumbImgData, int thumbImgDataLen, string messageExt)
		{
			iOSConnector.SendToWeixin (title, desc, mediaTagName, thumbImgData, thumbImgDataLen, messageExt);
		}

		public void WGSendToWeixinWithUrl (eWechatScene scene, string title, string desc, string url, string mediaTagName, byte[] thumbImgData, int thumbImgDataLen, string messageExt)
		{
			iOSConnector.SendToWeixinWithUrl ((int)scene, title, desc, url, mediaTagName, thumbImgData, thumbImgDataLen, messageExt);
		}

		public void WGSendToWeixinWithPhoto (eWechatScene scene, string mediaTagName, byte[] imgData, int imgDataLen, string messageExt, string messageAction)
		{
			iOSConnector.SendToWeixinWithPhoto ((int)scene, mediaTagName, imgData, imgDataLen, messageExt, messageAction);
		}

		public void WGSendToWeixinWithMusic (eWechatScene scene, string title, string desc, string musicUrl, string musicDataUrl, string mediaTagName, byte[] imgData, int imgDataLen, string messageExt, string messageAction)
		{
			iOSConnector.SendToWeixinWithMusic ((int)scene, title, desc, musicUrl, musicDataUrl, mediaTagName, imgData, imgDataLen, messageExt, messageAction);
		}

		public void WGSendToWeixinWithVideo(eWechatScene scene, string title, string desc, string thumbUrl, string videoUrl, VideoParams videoParams, string mediaTagName, string messageAction, string messageExt)
		{
			iOSConnector.SendToWeixinWithVideo((int) scene, title, desc, thumbUrl, videoUrl, videoParams.ios_videoData,videoParams.ios_videoDataLen, mediaTagName, messageAction, messageExt);
		}

		public bool WGSendToWXGameFriend (string fOpenId, string title, string description, string mediaId, string messageExt, string mediaTagName, string msdkExtInfo)
		{
			return iOSConnector.SendToWXGameFriend (fOpenId, title, description, mediaId, messageExt, mediaTagName, msdkExtInfo);
		}
		public void WGSendToWXWithMiniApp(eWechatScene scene, string title, string desc, byte[] thumbImgData, int thumbImgDataLen, string webpageUrl, string userName, string path, bool withShareTicket, string messageExt, string messageAction)
		{
			iOSConnector.SendToWXWithMiniApp((int)scene,title,desc, thumbImgData, thumbImgDataLen, webpageUrl, userName, path, withShareTicket, messageExt, messageAction);
		}
		public bool WGSendMessageToWechatGameCenter (string fOpenid, string title, string content, WXMessageTypeInfo pInfo, WXMessageButton pButton, string msdkExtInfo)
		{
			string pInfoJsonString = JsonMapper.ToJson (pInfo);
			string pButtonJsonString = JsonMapper.ToJson (pButton);
			return iOSConnector.SendMessageToWechatGameCenter (fOpenid, title, content, pInfoJsonString, pButtonJsonString, msdkExtInfo);
		}

		public bool WGQueryWXMyInfo ()
		{
			return iOSConnector.QueryWXMyInfo ();
		}

		public bool WGQueryWXGameFriendsInfo ()
		{
			return iOSConnector.QueryWXGameFriendsInfo ();
		}

		public void WGCreateWXGroup (string unionid, string chatRoomName, string chatRoomNickName)
		{
			iOSConnector.CreateWXGroup (unionid, chatRoomName, chatRoomNickName);
		}

		public void WGJoinWXGroup (string unionid, string chatRoomNickName)
		{
			iOSConnector.JoinWXGroup (unionid, chatRoomNickName);
		}

		public void WGQueryWXGroupInfo (string unionid, string openIdList)
		{
			iOSConnector.QueryWXGroupInfo (unionid, openIdList);
		}

		public void WGSendToWXGroup (int msgType, int subType, string unionid, string title, string description, string messageExt, string mediaTagName, string imgUrl, string msdkExtInfo)
		{
			iOSConnector.SendToWXGroup (msgType, subType, unionid, title, description, messageExt, mediaTagName, imgUrl, msdkExtInfo);
		}
		public void WGQueryWXGroupStatus (string unionid, eStatusType opType)
		{
			iOSConnector.QueryWXGroupStatus (unionid, (int)opType);
		}
		public void WGOpenWeiXinDeeplink (string link)
		{
			iOSConnector.OpenWeiXinDeeplink (link);
		}

		public void WGShareToWXGameline(byte[] data, string gameExtra)
		{
			int lens = 0;
			if (data != null) {
				lens = data.Length;
			}
			iOSConnector.ShareToWXGameline(data, lens, gameExtra);
		}

		public void WGUnbindWeiXinGroup(string unionid)
		{
			iOSConnector.UnbindWeiXinGroup(unionid);
		}
#endregion

#region 通用接口
		public void WGFeedback (string body)
		{
			iOSConnector.FeedbackWithBody (body);
		}

		public void WGEnableCrashReport (bool bRDMEnable, bool bMTAEnable)
		{
			iOSConnector.EnableCrashReport (bRDMEnable, bMTAEnable);
		}

		public void WGReportEvent (string name, Dictionary<string, string> eventList, bool isRealTime)
		{
			try
			{
				JsonData json = new JsonData ();
				foreach (string key in eventList.Keys)
				{
					json[key] = eventList[key];
				}
				string jsonString = json.ToJson ();
				iOSConnector.ReportEvent (name, jsonString, isRealTime);
			}
			catch
			{
				Debug.LogError ("解析json失败，具体请看log日志");
			}
		}

		public string WGGetVersion ()
		{
			return iOSConnector.GetVersion ();
		}

		public string WGGetChannelId ()
		{
			return iOSConnector.GetChannelId ();
		}

		public string WGGetPlatformAPPVersion (ePlatform platform)
		{
			return iOSConnector.GetPlatformAPPVersion ((int) platform);
		}

		public string WGGetRegisterChannelId ()
		{
			return iOSConnector.GetRegisterChannelId ();
		}

		public bool WGIsPlatformInstalled (ePlatform platformType)
		{
			return iOSConnector.IsPlatformInstalled ((int)platformType);
		}

		public string WGGetPfKey ()
		{
			return iOSConnector.GetPfKey ();
		}

		public string WGGetPf ()
		{
			return iOSConnector.GetPf ();
		}

		public void WGLoginWithLocalInfo ()
		{
			iOSConnector.LoginWithLocalInfo ();
		}

		public void WGShowNotice (string scene)
		{
			iOSConnector.ShowNotice (scene);
		}

		public void WGHideScrollNotice ()
		{
			iOSConnector.HideScrollNotice ();
		}

		public void WGOpenUrl (string openUrl)
		{
			iOSConnector.OpenUrl (openUrl);
		}

		public void WGOpenUrl (string openUrl, eMSDK_SCREENDIR screendir)
		{
            MsdkUtil.Log("Warnning : This api was deprecated. Please use 'WGOpenUrl(string openUrl)' instead.");
			iOSConnector.OpenUrlWithScreenDir (openUrl, (int)screendir);
		}

		public string WGGetEncodeUrl (string openUrl)
		{
			return iOSConnector.GetEncodeUrl (openUrl);
		}

		public List<NoticeInfo> WGGetNoticeData (string scene)
		{
			string noticeDataStr = iOSConnector.GetNoticeData(scene);
			if (noticeDataStr != null)
			{
				return NoticeInfoList.ParseJson(noticeDataStr);
			}
			else
			{
				return new List<NoticeInfo>();
			}
		}

		public bool WGOpenAmsCenter (string parameters)
		{
			return iOSConnector.OpenAmsCenter (parameters);
		}

		public void WGGetNearbyPersonInfo ()
		{
			iOSConnector.GetNearbyPersonInfo ();
		}

		public bool WGCleanLocation ()
		{
			return iOSConnector.CleanLocation ();
		}

		public bool WGGetLocationInfo ()
		{
			return iOSConnector.GetLocationInfo ();
		}

		public void WGStartGameStatus (string cGameStatus)
		{
			iOSConnector.StartGameStatus (cGameStatus);
		}

		public void WGEndGameStatus (string cGameStatus, int succ, int errorCode)
		{
			iOSConnector.EndGameStatus (cGameStatus, succ, errorCode);
		}

		public int WGGetPaytokenValidTime ()
		{
			return iOSConnector.GetPaytokenValidTime ();
		}
			

		public void WGClearLocalNotifications ()
		{
			iOSConnector.ClearLocalNotifications ();
		}

		public void WGOpenMSDKLog (bool enabled)
		{
			iOSConnector.OpenMSDKLog (enabled);
		}

		public string WGGetGuestID ()
		{
			return iOSConnector.GetGuestID ();
		}

		public void WGResetGuestID ()
		{
			iOSConnector.ResetGuestID ();
		}


		public long WGAddLocalNotification (LocalMessageIOS localMessage)
		{
			string localMsgJsonString = JsonMapper.ToJson (localMessage);
			return iOSConnector.AddLocalNotification (localMsgJsonString);
		}

		public long WGAddLocalNotificationAtFront (LocalMessageIOS localMessage)
		{
			string localMsgJsonString = JsonMapper.ToJson (localMessage);
			return iOSConnector.AddLocalNotificationAtFront (localMsgJsonString);
		}

		public void WGClearLocalNotification (LocalMessageIOS localMessage)
		{
			string localMsgJsonString = JsonMapper.ToJson (localMessage);
			iOSConnector.ClearLocalNotification (localMsgJsonString);
		}

		public void WGSetPushTag (string tag)
		{
			iOSConnector.SetPushTag (tag);
		}

		public void WGDeletePushTag (string tag)
		{
			iOSConnector.DeletePushTag (tag);
		}

        public void WGBuglyLog(eBuglyLogLevel level, string log)
        {
            iOSConnector.BuglyLog((int) level, log);
        }

		public void WGSendToQQWithPhoto(eQQScene scene,ImageParams imageParams,string extraScene,string messageExt)
        {
			iOSConnector.SendToQQWithPhotoWithParams((int)scene,imageParams.ios_imageData, imageParams.ios_imageDataLen, extraScene, messageExt);
        }
#endregion

#region Android平台接口
        public void WGJoinQQGroup(string qqGroupKey)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGQueryQQGroupInfo(string cUnionid, string cZoneid)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGUnbindQQGroup(string cGroupOpenid, string cUnionid)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

//        public void WGQueryQQGroupKey(string cGroupOpenid)
//        {
//            MsdkUtil.Log("Error : This interface is only use in Android!");
//            throw new NotImplementedException();
//        }

        public void WGAddCardToWXCardPackage(string cardId, string timestamp, string sign)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGSendToWeixinWithPhotoPath(eWechatScene scene, string mediaTagName, string imgPath, string messageExt, string messageAction)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGSendToQQ(eQQScene scene, string title, string desc, string url, string imgUrl, int imgUrlLen)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGSendToQQWithPhoto(eQQScene scene, string imgFilePath)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGSendToQQWithRichPhoto(string summary, ArrayList imgs)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGSendToQQWithVideo(string summary, string videoPath)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public bool WGCheckApiSupport(eApiName apiName)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGStartSaveUpdate(bool isUseYYB)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public void WGCheckNeedUpdate()
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public int WGCheckYYBInstalled()
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }

        public long WGAddLocalNotification(LocalMessageAndroid msg)
        {
            MsdkUtil.Log("Error : This interface is only use in Android!");
            throw new NotImplementedException();
        }
		public void reportData()
		{
			string cmd = "unity_version_report";
			JsonData jsonData = new JsonData ();
			jsonData ["msdkUnityVersion"] = WGPlatform.Version;

			iOSConnector.ReportUnityData (cmd,jsonData.ToJson());
		}
#endregion
	}
}
#endif
