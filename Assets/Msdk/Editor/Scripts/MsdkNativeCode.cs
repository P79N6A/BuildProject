using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class MsdkNativeCode
{
    public static readonly string IOS_SRC_HEADER = @"#include ""PluginBase/AppDelegateListener.h""";

    public static readonly string IOS_SRC_FINISH =
@"- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{";

    public static readonly string IOS_SRC_OPENURL =
@"- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation";

    public static readonly string IOS_SRC_REGISTER =
@"- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{";

    public static readonly string IOS_SRC_REGISTER_FAIL =
@"- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{";

    public static readonly string IOS_SRC_RECEIVE =
@"- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo
{";

    public static readonly string IOS_SRC_ACTIVE = @"- (void)applicationDidBecomeActive:(UIApplication*)application
{";


    public static readonly string IOS_HEADER = @"
#import <MSDK/MSDK.h>
#import <MSDKAdapter/UnityObserver.h>
";

    public static readonly string IOS_HANDLE_URL = @"
    // 处理平台拉起游戏
    NSLog(@""MSDK handle url == %@"",url);
    WGPlatform::GetInstance();
    return [WGInterface HandleOpenURL:url];
";

    public static readonly string IOS_XG_REGISTER = @"    // 注册信鸽推送
	[MSDKXG WGRegisterAPNSPushNotification:launchOptions];
";

    public static readonly string IOS_XG_SUCC = @"    // 注册信鸽成功
    NSLog(@""deviceToken = %@"",deviceToken);
    [MSDKXG WGSuccessedRegisterdAPNSWithToken:deviceToken];
";

    public static readonly string IOS_XG_FAIL = @"    // 注册信鸽失败
    NSLog(@""regisger failed:%@"",[error description]);
    [MSDKXG WGFailedRegisteredAPNS];
";

    public static readonly string IOS_XG_RECEIVE = @"    // 接收信鸽推送消息
    NSLog(@""userinfo:%@"",userInfo);
    [MSDKXG WGReceivedMSGFromAPNSWithDict:userInfo];
";

    public static readonly string IOS_XG_CLEAR = @"    // 清空应用桌面图标右上角的推送条目
    [MSDKXG WGCleanBadgeNumber];
";

    public static readonly string IOS_BECAME_ACTIVE = @"    // 自动登录&验证本地票据
    static bool msdkAppfirstLaunch = true;
    LoginRet loginRet;
    if(msdkAppfirstLaunch == true){
        WGPlatform::GetInstance()->WGGetLoginRecord(loginRet);
        WGPlatform::GetInstance()->WGLogin();
        msdkAppfirstLaunch = false;
        NSLog(@""MSDK autologin"");
    }
    
";

}
