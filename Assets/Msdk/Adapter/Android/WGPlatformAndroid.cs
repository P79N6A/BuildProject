#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace Msdk
{

	public class WGPlatformAndroid : WGPlatformUnity, IMsdk
	{
		// MSDK Unity 初始化
		public new void Init()
		{
			base.Init();
            // 注册C++回调
            AndroidConnector.setBridge(MessageCenter.MessageConsumer);
            // 上报Unity版本
            //reportData(WGPlatform.Version);
            reportData();
		}

        private void reportData(string message)
        {
            try {
                LoginRet loginRet = WGGetLoginRecord();
                
                message += "_" + loginRet.open_id;
                using (AndroidJavaObject report = new AndroidJavaObject("com.tencent.msdk.request.MsdkDataReport"))
                {
                    // operate 设置为1001，用以后台过滤
                    report.Call("reportData", message, (int)ePlatform.ePlatform_QQ, 1001);
                }
            } catch (Exception e) {
                Debug.LogError(e.Message);
            }
        }

        public void reportData()
        {
            string cmd = "unity_version_report";
            JsonData jsonData = new JsonData();
            jsonData["msdkUnityVersion"] = WGPlatform.Version;

            AndroidConnector.ReportUnityData(cmd, jsonData.ToJson());
        }
        public static AndroidJavaObject dicToMap(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return null;
            }
            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                map.Call<string>("put", pair.Key, pair.Value);
            }
            return map;
        }
		#region 登录相关接口
		public void WGLogin (ePlatform platform)
		{
            AndroidConnector.Login((int)platform);
		}

		public int WGLoginOpt (ePlatform platform,int overtime)
		{
			return AndroidConnector.LoginOpt((int)platform,overtime);
		}
		public void WGQrCodeLogin (ePlatform platform)
		{
            AndroidConnector.QrCodeLogin((int)platform);
		}

		public bool WGLogout ()
		{
            return AndroidConnector.Logout();
		}

		public LoginRet WGGetLoginRecord ()
		{
            string loginRetStr = AndroidConnector.GetLoginRecord();
			if (loginRetStr != null) {
				return LoginRet.ParseJson (loginRetStr);
			} else {
				return new LoginRet();
			}
		}

		public void WGSetPermission (int permissions)
		{
            AndroidConnector.SetPermission(permissions);
		}

		public void WGLoginWithLocalInfo ()
		{
            AndroidConnector.LoginWithLocalInfo();
		}

		public bool WGSwitchUser (bool switchToLaunchUser)
		{
            return AndroidConnector.SwitchUser(switchToLaunchUser);
		}

		public int WGGetPaytokenValidTime ()
		{
            return AndroidConnector.GetPaytokenValidTime();
		}

        public void WGRealNameAuth(RealNameAuthInfo info)
        {
            string authInfoJsonStr = JsonMapper.ToJson(info);
            AndroidConnector.RealNameAuth(authInfoJsonStr);
        }
		#endregion

		#region QQ相关接口
		/**
		 * 查询关系链
		 */
		public bool WGQueryQQMyInfo ()
		{
            return AndroidConnector.QueryQQMyInfo();
		}

		public bool WGQueryQQGameFriendsInfo ()
		{
            return AndroidConnector.QueryQQGameFriendsInfo();
		}

		/**
		 * 加绑群，加好友
		 */
		public void WGBindQQGroup (string unionid, string union_name, string zoneid, string signature)
		{
            AndroidConnector.BindQQGroup(unionid, union_name, zoneid, signature);
		}

		public void WGAddGameFriendToQQ (string fopenid, string desc, string message)
		{
            AndroidConnector.AddGameFriendToQQ(fopenid, desc, message);
		}

		/**
		 * 分享
		 */
		public void WGSendToQQ (eQQScene scene, string title, string desc, string url, string imgUrl, int imgUrlLen)
		{
            AndroidConnector.SendToQQ((int)scene, title, desc, url, imgUrl, imgUrlLen);
		}

		public void WGSendToQQWithPhoto (eQQScene scene, string imgFilePath)
		{
            AndroidConnector.SendToQQWithPhoto((int)scene, imgFilePath);
		}

		public void WGSendToQQWithMusic (eQQScene scene, string title, string desc, string musicUrl, string musicDataUrl, string imgUrl)
		{
            AndroidConnector.SendToQQWithMusic((int)scene, title, desc, musicUrl, musicDataUrl, imgUrl);
		}

		public bool WGSendToQQGameFriend (int act, string fopenid, string title, string summary,
                string targetUrl, string imgUrl, string previewText, string gameTag, string msdkExtInfo)
		{
             return AndroidConnector.SendToQQGameFriend(act, fopenid, title, summary, targetUrl, imgUrl,
                 previewText, gameTag, msdkExtInfo);
		}

		public void WGSendToQQWithRichPhoto (string summary, ArrayList imgs)
		{
			StringBuilder sb = new StringBuilder();
			JsonWriter writer = new JsonWriter(sb);
			writer.WriteArrayStart();
			foreach (string img in imgs) {
				writer.Write(img);
			}
			writer.WriteArrayEnd();
            AndroidConnector.SendToQQWithRichPhoto(summary, sb.ToString());
		}

		public void WGSendToQQWithVideo (string summary, string videoPath)
		{
            AndroidConnector.SendToQQWithVideo(summary, videoPath);
		}

		/**
		 * QQ群
		 */
		public void WGJoinQQGroup (string qqGroupKey)
		{
            AndroidConnector.JoinQQGroup(qqGroupKey);
		}

        public void WGJoinQQGroup(string groupNum, string groupKey)
        {
            //MsdkUtil.Log("Error : This interface is only use in iOS!");
            //throw new NotImplementedException();
            WGJoinQQGroup(groupKey);
        }

		public void WGQueryQQGroupInfo (string unionId, string zoneId)
		{
            AndroidConnector.QueryQQGroupInfo(unionId, zoneId);
		}

		public void WGUnbindQQGroup (string groupOpenid, string unionId)
		{
            AndroidConnector.UnbindQQGroup(groupOpenid, unionId);
		}

		public void WGQueryQQGroupKey (string groupOpenid)
		{
            AndroidConnector.QueryQQGroupKey(groupOpenid);
		}

        //public void WGCreateQQGroupV2(
        //    string guildId,
        //    string guildName, 
        //    string zoneId,
        //    string roleId, 
        //    string partition
        //    )
        //{
        //    AndroidConnector.CreateQQGroupV2(guildId, guildName, zoneId, roleId, partition);
        //}
        public void WGCreateQQGroupV2(GameGuild gameGuild)
        {
            AndroidConnector.CreateQQGroupV2(gameGuild.guildId, gameGuild.guildName, gameGuild.zoneId, gameGuild.roleId, gameGuild.partition);
        }

        //public void WGJoinQQGroupV2(
        //    string guildId,
        //    string zoneId,
        //    string groupId,
        //    string roleId,
        //    string partition)
        //{
        //    AndroidConnector.JoinQQGroupV2(guildId, zoneId,groupId, roleId, partition);
        //}
        public void WGJoinQQGroupV2(GameGuild gameGuild,string groupId)
        {
            AndroidConnector.JoinQQGroupV2(gameGuild.guildId, gameGuild.zoneId, groupId, gameGuild.roleId, gameGuild.partition);
        }

        public void WGUnbindQQGroupV2(GameGuild gameGuild)
        {
            AndroidConnector.UnbindQQGroupV2(gameGuild.guildId, gameGuild.guildName, gameGuild.zoneId);
        }

        public void WGQueryQQGroupInfoV2(string groupId)
        {
            AndroidConnector.QueryQQGroupInfoV2(groupId);
        }
        public void WGSendToQQWithPhoto(eQQScene scene, ImageParams imageParams, string extraScene, string messageExt) 
        {
            AndroidConnector.SendToQQWithPhotoWithParams((int)scene, imageParams.android_imagePath, extraScene, messageExt);
        }
        public void WGSendToWXWithMiniApp(eWechatScene scene, string title, string desc, byte[] thumbImgData, int thumbImgDataLen, string webpageUrl, string userName, string path, bool withShareTicket, string messageExt, string messageAction)
        {
            AndroidConnector.SendToWXWithMiniApp((int)scene, title, desc, thumbImgData, thumbImgDataLen, webpageUrl, userName, path, withShareTicket, messageExt, messageAction);
        }

        public void WGBindExistQQGroupV2(GameGuild gameGuild, string groupId, string groupName)
        {
            AndroidConnector.BindExistQQGroupV2(groupId, groupName, gameGuild.guildId, gameGuild.zoneId,gameGuild.roleId);
        }

        public void WGGetQQGroupCodeV2(GameGuild gameGuild)
        {
            AndroidConnector.GetQQGroupCodeV2(gameGuild.guildId,gameGuild.zoneId);
        }

        public void WGQueryBindGuildV2(string groupId, int type = 0)
        {
            AndroidConnector.QueryBindGuildV2(groupId, type);
        }

        public void WGGetQQGroupListV2()
        {
            AndroidConnector.GetQQGroupListV2();
        }

        public void WGRemindGuildLeaderV2(GameGuild gameGuild)
        {
             AndroidConnector.RemindGuildLeaderV2(gameGuild.guildId,gameGuild.zoneId, gameGuild.roleId, gameGuild.roleName,
                             gameGuild.partition,gameGuild.userZoneId, gameGuild.type, gameGuild.leaderOpenId,
                             gameGuild.leaderRoleId, gameGuild.leaderZoneId, gameGuild.areaId);
        }

        public void WGSendToWeixinWithVideo(eWechatScene scene, string title, string desc, string thumbUrl, string videoUrl, VideoParams videoParams, string mediaTagName, string messageAction, string messageExt)
        {
            Debug.Log("scene:" + scene + ",title:" + title + ",desc:" + desc + ",thumbUrl:" + thumbUrl + ",videoUrl:" + videoUrl +
                ",videoParams.android_videoPath:" + videoParams.android_videoPath + ",mediaTagName:" + mediaTagName + ",messageAction:" + messageAction + ",messageExt:" + messageExt);
            AndroidConnector.SendToWeixinWithVideo((int)scene, title, desc, thumbUrl, videoUrl, videoParams.android_videoPath, mediaTagName, messageAction, messageExt);
        }

        #endregion

        #region 微信相关接口
		public void WGOpenWeiXinDeeplink (string link)
		{
            AndroidConnector.OpenWeiXinDeeplink(link);
		}

		public void WGRefreshWXToken ()
		{
            AndroidConnector.RefreshWXToken();
		}

		/**
		 * 关系链
		 */
		public bool WGQueryWXMyInfo ()
		{
            return AndroidConnector.QueryWXMyInfo();
		}

		public bool WGQueryWXGameFriendsInfo ()
		{
            return AndroidConnector.QueryWXGameFriendsInfo();
		}

		/**
		 * 分享
		 */
		public void WGSendToWeixin (string title, string desc, string mediaTagName, byte[] imgData,
                int imgDataLen, string messageExt)
		{
            AndroidConnector.SendToWeixin(title, desc, mediaTagName, imgData, imgDataLen, messageExt);
		}

		public void WGSendToWeixinWithUrl (eWechatScene scene, string title, string desc, string url,
                string mediaTagName, byte[] imgData, int imgDataLen, string messageExt)
		{
            AndroidConnector.SendToWeixinWithUrl((int)scene, title, desc, url, mediaTagName, imgData,
                imgDataLen, messageExt);
		}

		public void WGSendToWeixinWithPhoto (eWechatScene scene, string mediaTagName, byte[] imgData,
                int imgDataLen, string messageExt, string messageAction)
		{
            AndroidConnector.SendToWeixinWithPhoto((int)scene, mediaTagName, imgData, imgDataLen,
                messageExt, messageAction);
		}

		public void WGSendToWeixinWithPhotoPath (eWechatScene scene, string mediaTagName, string imgPath,
                string messageExt, string messageAction)
		{
            AndroidConnector.SendToWeixinWithPhotoPath((int)scene, mediaTagName, imgPath, messageExt,
                messageAction);
		}

		public void WGSendToWeixinWithMusic (eWechatScene scene, string title, string desc, string musicUrl,
                string musicDataUrl, string mediaTagName, byte[] imgData, int imgDataLen, string messageExt,
                string messageAction)
		{
            AndroidConnector.SendToWeixinWithMusic((int)scene, title, desc, musicUrl, musicDataUrl,
                    mediaTagName, imgData, imgDataLen, messageExt, messageAction);
		}

		public bool WGSendToWXGameFriend (string fOpenId, string title, string description, string mediaId,
                string messageExt, string mediaTagName, string msdkExtInfo)
		{
            return AndroidConnector.SendToWXGameFriend(fOpenId, title, description, mediaId,
                    messageExt, mediaTagName, msdkExtInfo);
		}

		public bool WGSendMessageToWechatGameCenter (string fOpenid, string title, string content,
                WXMessageTypeInfo pInfo, WXMessageButton pButton, string msdkExtInfo)
		{
			string pInfoJsonString = JsonMapper.ToJson (pInfo);
			string pButtonJsonString = JsonMapper.ToJson (pButton);
            return AndroidConnector.SendMessageToWechatGameCenter(fOpenid, title, content,
                    pInfoJsonString, pButtonJsonString, msdkExtInfo);
		}

        public void WGShareToWXGameline(byte[] data, string gameExtra)
        {
            int lens = 0;
            if (data != null) {
                lens = data.Length;
            }
            AndroidConnector.ShareToWXGameline(data, lens, gameExtra);
        }
		/**
		 * 微信群，卡卷
		 */
		public void WGCreateWXGroup (string unionId, string chatRoomName, string chatRoomNickName)
		{
            AndroidConnector.CreateWXGroup(unionId, chatRoomName, chatRoomNickName);
		}

		public void WGJoinWXGroup (string unionId, string chatRoomNickName)
		{
            AndroidConnector.JoinWXGroup(unionId, chatRoomNickName);
		}

		public void WGQueryWXGroupInfo (string unionId, string openIdList)
		{
            AndroidConnector.QueryWXGroupInfo(unionId, openIdList);
		}

		public void WGSendToWXGroup (int msgType, int subType, string unionId, string title,
                string description, string messageExt, string mediaTagName, string imgUrl,
                string msdkExtInfo)
		{
            AndroidConnector.SendToWXGroup(msgType, subType, unionId, title, description,
                    messageExt, mediaTagName, imgUrl, msdkExtInfo);
		}

		public void WGAddCardToWXCardPackage (string cardId, string timestamp, string sign)
		{
            AndroidConnector.AddCardToWXCardPackage(cardId, timestamp, sign);
		}
		public void WGQueryWXGroupStatus (string unionid, eStatusType opType)
		{
			AndroidConnector.QueryWXGroupStatus (unionid, (int)opType);
		}
		public void WGUnbindWeiXinGroup(string unionid)
		{
            AndroidConnector.UnbindWeiXinGroup(unionid);
		}
        #endregion

        #region 其他接口
		        public string WGGetPf ()
		        {
                    return AndroidConnector.GetPf();
		        }

		        public bool WGCheckApiSupport (eApiName apiName)
		        {
                    return AndroidConnector.CheckApiSupport((int)apiName);
		        }

		        public void WGFeedback (string body)
		        {
                    AndroidConnector.FeedbackWithBody(body);
		        }

		        public void WGEnableCrashReport (bool bRDMEnable, bool bMTAEnable)
		        {
                    AndroidConnector.EnableCrashReport(bRDMEnable, bMTAEnable);
		        }

		        public void WGReportEvent (string name, Dictionary<string, string> eventList, bool isRealTime)
		        {
			        JsonData json = new JsonData ();
			        foreach (string key in eventList.Keys) {
				        json[key] = eventList[key];
			        }
			        string jsonData = json.ToJson ();
                    AndroidConnector.ReportEvent(name, jsonData, isRealTime);
		        }

		        public string WGGetVersion ()
		        {
                    return AndroidConnector.GetVersion();
		        }

		        public string WGGetChannelId ()
		        {
                    return AndroidConnector.GetChannelId();
		        }

		        public string WGGetPlatformAPPVersion (ePlatform platform)
		        {
                    return AndroidConnector.GetPlatformAPPVersion((int)platform);
		        }

		        public string WGGetRegisterChannelId ()
		        {
                    return AndroidConnector.GetRegisterChannelId();
		        }

		        public bool WGIsPlatformInstalled (ePlatform platform)
		        {
                    return AndroidConnector.IsPlatformInstalled((int)platform);
		        }

		        public string WGGetPfKey ()
		        {
                    return AndroidConnector.GetPfKey();
		        }

		        /**
		         * 游戏场景
		         */
		        public void WGStartGameStatus (string status)
		        {
                    AndroidConnector.StartGameStatus(status);
		        }

                public void WGEndGameStatus(string status, int succ, int errorCode)
		        {
                    AndroidConnector.EndGameStatus(status, succ, errorCode);
		        }

		        /**
		         * 公告
		         */
		        public void WGShowNotice (string scene)
		        {
                    AndroidConnector.ShowNotice(scene);
		        }

		        public void WGHideScrollNotice ()
		        {
                    AndroidConnector.HideScrollNotice();
		        }

		        public List<NoticeInfo> WGGetNoticeData (string scene)
		        {
                    string noticeDataStr = AndroidConnector.GetNoticeData(scene);
			        if (noticeDataStr != null) {
				        return NoticeInfoList.ParseJson(noticeDataStr);
			        } else {
				        return new List<NoticeInfo>();
			        }
		        }

		        /**
		         * 内置浏览器
		         */
		        public void WGOpenUrl (string openUrl)
		        {
                    AndroidConnector.OpenUrl(openUrl);
		        }

		        public void WGOpenUrl (string openUrl, eMSDK_SCREENDIR screendir)
		        {
                    AndroidConnector.OpenUrlWithScreenDir(openUrl, (int)screendir);
		        }

		        public string WGGetEncodeUrl (string openUrl)
		        {
                    return AndroidConnector.GetEncodeUrl(openUrl);
		        }

		        public bool WGOpenAmsCenter (string parameters)
		        {
                    return AndroidConnector.OpenAmsCenter(parameters);
		        }

		        /**
		         * 位置信息
		         */
		        public void WGGetNearbyPersonInfo ()
		        {
                    AndroidConnector.GetNearbyPersonInfo();
		        }

		        public bool WGCleanLocation ()
		        {
                    return AndroidConnector.CleanLocation();
		        }

		        public bool WGGetLocationInfo ()
		        {
                    return AndroidConnector.GetLocationInfo();
		        }


		        /**
		         * 广告
		         */
		        public void WGShowAD (eADType adType)
		        {
                    AndroidConnector.ShowAD((int)adType);
		        }

		        public void WGCloseAD (eADType adType)
		        {
                    AndroidConnector.CloseAD((int)adType);
		        }

		        /**
		         * 应用宝更新
		         */
		        public void WGStartSaveUpdate (bool isUseYYB)
		        {
                    AndroidConnector.StartSaveUpdate(isUseYYB);
		        }

		        public void WGCheckNeedUpdate ()
		        {
                    AndroidConnector.CheckNeedUpdate();
		        }

		        public int WGCheckYYBInstalled ()
		        {
                    return AndroidConnector.CheckYYBInstalled();
		        }

		        public long WGAddLocalNotification (LocalMessageAndroid msg)
		        {
			        string jsonString = JsonMapper.ToJson (msg);
                    return AndroidConnector.AddLocalNotification(jsonString);
		        }

		        public void WGClearLocalNotifications ()
		        {
                    AndroidConnector.ClearLocalNotifications();
		        }

		        public void WGSetPushTag (string tag)
		        {
                    AndroidConnector.SetPushTag(tag);
		        }

		        public void WGDeletePushTag (string tag)
		        {
                    AndroidConnector.DeletePushTag(tag);
		        }

		        public void WGBuglyLog (eBuglyLogLevel level, string log)
		        {
                    AndroidConnector.BuglyLog((int)level, log);
		        }
                #endregion

        #region iOS平台接口

                public void WGSendToQQ(eQQScene scene, string title, string desc, string url, byte[] imgData, int imgDataLen)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public void WGSendToQQWithPhoto(eQQScene scene, byte[] imgData, int imgDataLen)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public long WGAddLocalNotification(LocalMessageIOS localMessage)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public long WGAddLocalNotificationAtFront(LocalMessageIOS localMessage)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public void WGClearLocalNotification(LocalMessageIOS localMessage)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public void WGOpenMSDKLog(bool enabled)
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public string WGGetGuestID()
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }

                public void WGResetGuestID()
                {
                    MsdkUtil.Log("Error : This interface is only use in iOS!");
                    throw new NotImplementedException();
                }
                #endregion
	}
}   
#endif
