#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Msdk
{
    public class AndroidConnector
    {
        // 设置桥接，C++可通过 SendToUnity 回调到C#
        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBridge(MessageCenter.SendToUnity bridge);

        // MSDK接口
        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Login(int platform);

		[DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LoginOpt(int platform,int overtime);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QrCodeLogin(int platform);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetLoginRecord();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoginWithLocalInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Logout();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SwitchUser(bool isSwitch);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPermission(int permissions);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPaytokenValidTime();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RefreshWXToken();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RealNameAuth(string authInfo);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool QueryQQMyInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool QueryQQGameFriendsInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SendToQQ(
            int scene,
            string title,
            string desc,
            string url,
            string imgurl,
            int imgUrlLen);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToQQWithMusic(
            int scene,
            string title,
            string desc,
            string musicUrl,
            string musicDataUrl,
            string imgUrl);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SendToQQGameFriend(
            int act,
            string fopenid,
            string title,
            string summary,
            string targetUrl,
            string imgUrl,
            string previewText,
            string gameTag,
            string msdkExtInfo);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToQQWithPhoto(int scene, string imgPath);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToQQWithPhotoWithParams(int scene, string imgPath, string extraScene, string messageExt);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToQQWithRichPhoto(string summary, string imgPaths);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToQQWithVideo(string summary, string videoPath);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BindQQGroup(
           string unionid,
           string union_name,
           string zoneid,
           string signature);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void JoinQQGroup(string qqGroupKey);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnbindQQGroup(string groupOpenId, string unionId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryQQGroupInfo(string unionId, string zoneId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryQQGroupKey(string groupOpenId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddGameFriendToQQ(string fopenid, string desc, string message);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool QueryWXMyInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool QueryWXGameFriendsInfo();
		
		[DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool QueryWXGroupStatus(string unionid,int type);

		[DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool UnbindWeiXinGroup(string unionid);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateQQGroupV2(
            string guildId,
            string guildName, 
            string zoneId,
            string roleId, 
            string partition
            );

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void JoinQQGroupV2(
            string guildId,
            string zoneId,
            string groupId,
            string roleId,
            string partition);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnbindQQGroupV2(
            string guildId,
            string guildName,
            string zoneId);
		[DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
		public static extern void WGUnbindWeiXinGroup(
			string guildId,
			string guildName,
			string zoneId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryQQGroupInfoV2(string groupId);

        //[DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void SendToQQWithPhoto(int scene, ImageParams imageParams, string extraScene, string messageExt);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BindExistQQGroupV2(string groupId, string groupName, string guildId, string zoneId, string roleId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetQQGroupCodeV2(string guildId,string zoneId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryBindGuildV2(string groupId, int type = 0);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetQQGroupListV2();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RemindGuildLeaderV2(string guildId,string zoneId, string roleId, string roleName,
                             string partition,string userZoneId, string type, string leaderOpenId,
                             string leaderRoleId, string leaderZoneId, string areaId);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShareToWXGameline(byte[] data, int lens, string gameExtra);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixin(
            string title,
            string desc,
            string mediaTagName,
            byte[] imgData,
            int imgDataLen,
            string messageExt);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixinWithUrl(
            int scene,
            string title,
            string desc,
            string url,
            string mediaTagName,
            byte[] imgData,
            int imgDataLen,
            string messageExt);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixinWithPhoto(
            int scene,
            string mediaTagName,
            byte[] imgData,
            int imgDataLen,
            string messageExt,
            string messageAction);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixinWithPhotoPath(
            int scene,
            string mediaTagName,
            string imgPath,
            string messageExt,
            string messageAction);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixinWithMusic(
            int scene,
            string title,
            string desc,
            string musicUrl,
            string musicDataUrl,
            string mediaTagName,
            byte[] imgData,
            int imgDataLen,
            string messageExt,
            string messageAction);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SendToWXGameFriend(
            string fOpenId,
            string title,
            string desc,
            string mediaId,
            string messageExt,
            string mediaTagName,
            string msdkExtInfo);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SendMessageToWechatGameCenter(
            string fOpenid,
            string title,
            string content,
            string pTypeInfo,
            string pButtonInfo,
            string msdkExtInfo);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateWXGroup(string unionid, string chatRoomName, string chatRoomNickName);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void JoinWXGroup(string unionid, string chatRoomNickName);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void QueryWXGroupInfo(string unionid, string openIdList);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWXGroup(
            int msgType,
            int subType,
            string unionid,
            string title,
            string description,
            string messageExt,
            string mediaTagName,
            string imgUrl,
            string msdkExtInfo);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenWeiXinDeeplink(string link);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddCardToWXCardPackage(string cardId, string timestamp, string sign);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShowNotice(string scene);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HideScrollNotice();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetNoticeData(string scene);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenUrl(string openUrl);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenUrlWithScreenDir(string openUrl, int screenDir);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetEncodeUrl(string openUrl);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OpenAmsCenter(string parameters);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReportEvent(string name, string eventList, bool isRealTime);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetNearbyPersonInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetLocationInfo();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CleanLocation();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EnableCrashReport(bool bRDMEnable, bool bMTAEnable);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsPlatformInstalled(int platform);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetPlatformAPPVersion(int platform);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CheckApiSupport(int api);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetVersion();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetChannelId();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetRegisterChannelId();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetPf();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetPfKey();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FeedbackWithBody(string body);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CheckYYBInstalled();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CheckNeedUpdate();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartSaveUpdate(bool isUseYYB);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShowAD(int scene);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseAD(int scene);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartGameStatus(string status);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void EndGameStatus(string status, int succ, int errorCode);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern long AddLocalNotification(string localMsgJsonStr);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearLocalNotifications();

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPushTag(string tag);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DeletePushTag(string tag);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BuglyLog(int level, string log);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWXWithMiniApp(int scene, string title, string desc, byte[] thumbImgData, int thumbImgDataLen, string webpageUrl, string userName, string path, bool withShareTicket, string messageExt, string messageAction);

        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendToWeixinWithVideo(int scene, string title, string desc, string thumbUrl, string videoUrl, string videoPath, string mediaTagName, string messageAction, string messageExt);
        
        [DllImport("MsdkAdapter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReportUnityData(string name, string jsonData);
    }
}
#endif
