#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Msdk
{

	public class iOSConnector {

        [DllImport("__Internal")]
        public static extern void setBridge(MessageCenter.SendToUnity bridge);

		[DllImport("__Internal")]
		public static extern string GetLoginRecord();

		[DllImport("__Internal")]
		public static extern void Login(int platform);

		[DllImport("__Internal")]
		public static extern int LoginOpt(int platform,int overtime);

		[DllImport("__Internal")]
		public static extern void WGQrCodeLogin(int platform);

		[DllImport("__Internal")]
		public static extern bool Logout();

        [DllImport("__Internal")]
        public static extern void RealNameAuth(string infoStr);

		[DllImport("__Internal")]
		public static extern void SetPermission(int permissions);

		[DllImport("__Internal")]
		public static extern void SendToWeixin(
			string title,
			string desc,
			string mediaTagName,
			byte[] thumbImgData,
			int thumbImgDataLen,
			string messageExt
			);
			

		[DllImport("__Internal")]
		public static extern void SendToWeixinWithUrl(
			int scene,
			string title,
			string desc,
			string url,
			string mediaTagName,
			byte[] thumbImgData,
			int thumbImgDataLen,
			string messageExt
			);
			

		[DllImport("__Internal")]
		public static extern void SendToWeixinWithPhoto(
			int scene,
			string mediaTagName,
			byte[] imgData,
			int imgDataLen,
			string messageExt,
			string messageAction
			);
			
		[DllImport("__Internal")]
		public static extern void SendToWeixinWithVideo(
			int scene, 
			string title, 
			string desc, 
			string thumbUrl, 
			string videoUrl, 
			byte[] videoParamsData,
			int videoParamsLen,
			string mediaTagName, 
			string messageAction, 
			string messageExt);
		
		[DllImport("__Internal")]
		public static extern void FeedbackWithBody(string body);

		[DllImport("__Internal")]
		public static extern void EnableCrashReport(bool bRDMEnable, bool bMTAEnable);

		[DllImport("__Internal")]
		public static extern void ReportEvent(
			string name,
			string eventList,
			bool isRealTime
			);

		[DllImport("__Internal")]
		public static extern string GetVersion();

		[DllImport("__Internal")]
		public static extern string GetChannelId();

		[DllImport("__Internal")]
		public static extern string GetPlatformAPPVersion(int platform);

		[DllImport("__Internal")]
		public static extern string GetRegisterChannelId();

		[DllImport("__Internal")]
		public static extern void RefreshWXToken();

		[DllImport("__Internal")]
		public static extern bool IsPlatformInstalled(int platformType);

		[DllImport("__Internal")]
		public static extern string GetPfKey();

		[DllImport("__Internal")]
		public static extern void LogPlatformSDKVersion();

		[DllImport("__Internal")]
		public static extern bool QueryQQMyInfo();

		[DllImport("__Internal")]
		public static extern bool QueryQQGameFriendsInfo();

		[DllImport("__Internal")]
		public static extern bool QueryWXMyInfo();

		[DllImport("__Internal")]
		public static extern bool QueryWXGameFriendsInfo();

		[DllImport("__Internal")]
		public static extern void CreateWXGroup (string unionid, string chatRoomName, string chatRoomNickName);

		[DllImport("__Internal")]
		public static extern void JoinWXGroup (string unionid, string chatRoomNickName);

        [DllImport("__Internal")]
		public static extern void JoinQQGroup (string groupNum, string groupKey);

		[DllImport("__Internal")]
		public static extern void QueryWXGroupInfo (string unionid, string openIdList);

		[DllImport("__Internal")]
		public static extern void SendToWXGroup (
			int msgType,
			int subType,
			string unionid,
			string title,
			string description,
			string messageExt,
			string mediaTagName,
			string imgUrl,
			string msdkExtInfo);

		[DllImport("__Internal")]
		public static extern bool SendToQQGameFriend(
			int act,
			string fopenid,
			string title,
			string summary,
			string targetUrl,
			string imgUrl,
			string previewText,
			string gameTag,
			string msdkExtInfo
			);

		[DllImport("__Internal")]
		public static extern void OpenWeiXinDeeplink (string link);

		[DllImport("__Internal")]
		public static extern bool SendToWXGameFriend(
			string fOpenId,
			string title,
			string description,
			string mediaId,
			string messageExt,
			string mediaTagName,
			string msdkExtInfo
			);

		[DllImport("__Internal")]
		public static extern bool SendToWXWithMiniApp(
			int scene, 
			string title, 
			string desc, 
			byte[] thumbImgData, 
			int thumbImgDataLen, 
			string webpageUrl, 
			string userName, 
			string path, 
			bool withShareTicket, 
			string messageExt, 
			string messageAction);

		[DllImport("__Internal")]
		public static extern void LoginWithLocalInfo();

		[DllImport("__Internal")]
		public static extern void ShowNotice(string scene);

		[DllImport("__Internal")]
		public static extern void HideScrollNotice();

		[DllImport("__Internal")]
		public static extern void OpenUrl(string openUrl);

		[DllImport("__Internal")]
		public static extern void OpenUrlWithScreenDir(string openUrl, int screenDir);

		[DllImport("__Internal")]
		public static extern string GetEncodeUrl (string openUrl);

		[DllImport("__Internal")]
		public static extern string GetNoticeData(string scene);

		[DllImport("__Internal")]
		public static extern bool OpenAmsCenter(string parameters);

		[DllImport("__Internal")]
		public static extern void GetNearbyPersonInfo();

		[DllImport("__Internal")]
		public static extern bool CleanLocation();

		[DllImport("__Internal")]
		public static extern bool GetLocationInfo();

		[DllImport("__Internal")]
		public static extern bool SendMessageToWechatGameCenter(
			string fOpenid,
			string title,
			string content,
			string pTypeInfo,
			string pButtonInfo,
			string msdkExtInfo
		);

		[DllImport("__Internal")]
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
			string messageAction
			);

		[DllImport("__Internal")]
		public static extern void SendToQQWithMusic(
			int scene,
			string title,
			string desc,
			string musicUrl,
			string musicDataUrl,
			string imgUrl
			);

		[DllImport("__Internal")]
		public static extern bool SwitchUser(bool flag);


		[DllImport("__Internal")]
		public static extern void SendToQQ(
			int scene,
			string title,
			string desc,
			string url,
			byte[] imgData,
			int imgDataLen
			);

		[DllImport("__Internal")]
		public static extern void SendToQQWithPhoto(
			int scene,
			byte[] imgData,
			int imgDataLen
			);

		[DllImport("__Internal")]
		public static extern void SendToQQWithPhotoWithParams(
			int scene,
			byte[] imgData,
			int imgDataLen,
			string extraScene,
			string messageExt
			);
		[DllImport("__Internal")]
		public static extern string GetPf();

		[DllImport("__Internal")]
		public static extern int GetPaytokenValidTime();


		[DllImport("__Internal")]
		public static extern void OpenMSDKLog(bool enabled);

		[DllImport("__Internal")]
		public static extern void BindQQGroup(
           string unionid,
           string union_name,
           string zoneid,
           string signature);

		[DllImport("__Internal")]
		public static extern void AddGameFriendToQQ(
			string fopenid,
			string desc,
			string message);

		[DllImport("__Internal")]
		public static extern string GetGuestID();

		[DllImport("__Internal")]
		public static extern void ResetGuestID();

		[DllImport("__Internal")]
		public static extern void StartGameStatus (string cGameStatus);

		[DllImport("__Internal")]
		public static extern void EndGameStatus (string cGameStatus, int succ, int errorCode);

		[DllImport("__Internal")]
		public static extern void ClearLocalNotifications ();

		[DllImport("__Internal")]
		public static extern long AddLocalNotification(string localMsgJsonString);

		[DllImport("__Internal")]
		public static extern long AddLocalNotificationAtFront(string localMsgJsonString);

		[DllImport("__Internal")]
		public static extern void ClearLocalNotification(string localMsgJsonString);

		[DllImport("__Internal")]
		public static extern void SetPushTag (string tag);

		[DllImport("__Internal")]
		public static extern void DeletePushTag (string tag);

        [DllImport("__Internal")]
        public static extern void BuglyLog (int level, string log);

		[DllImport("__Internal")]
		public static extern bool QueryWXGroupStatus(string unionid, int type);

		[DllImport("__Internal")]
		public static extern void CreateQQGroupV2(
			string guildId,
			string guildName,
			string zoneId,
			string roleId,
			string partition
		);

		[DllImport("__Internal")]
		public static extern void JoinQQGroupV2(
			string guildId,
			string zoneId,
			string groupId,
			string roleId,
			string partition);

		[DllImport("__Internal")]
		public static extern void UnbindQQGroupV2(
			string guildId,
			string guildName,
			string zoneId);

		[DllImport("__Internal")]
		public static extern void QueryQQGroupInfoV2(string groupId);

		[DllImport("__Internal")]
		public static extern void GetQQGroupCodeV2(string guildId,string zoneId);

		[DllImport("__Internal")]
		public static extern void QueryQQGroupKey(string groupId);

		[DllImport("__Internal")]
		public static extern void BindExistQQGroupV2(string groupId, string groupName, string guildId, string zoneId,string roleId);

		[DllImport("__Internal")]
		public static extern void QueryBindGuildV2(string groupId, int type);

		[DllImport("__Internal")]
		public static extern void GetQQGroupListV2();

		[DllImport("__Internal")]
		public static extern void RemindGuildLeaderV2(string guildId,string zoneId, string roleId, string roleName,
			string partition,string userZoneId, string type, string leaderOpenId,
			string leaderRoleId, string leaderZoneId, string areaId);

		[DllImport("__Internal")]
		public static extern void ShareToWXGameline(byte[] data, int lens, string gameExtra);

		[DllImport("__Internal")]
		public static extern void UnbindWeiXinGroup(string groupId);

		[DllImport("__Internal")]
		public static extern void ReportUnityData(string name, string jsonData);
	}

}
#endif
