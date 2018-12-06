//
//  信鸽核心接口
//  XG-SDK
//
//  Created by xiangchen on 13-10-18.
//  Copyright (c) 2013年 mta. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface MXGPush : NSObject

#pragma mark- 初始化相关
/**
 初始化信鸽

 @param appId 通过前台申请的应用ID
 @param appKey 通过前台申请的appKey
 */
+(void)startApp:(uint32_t)appId appKey:(nonnull NSString *)appKey;

/**
 注册设备

 @param deviceToken 通过app delegate的didRegisterForRemoteNotificationsWithDeviceToken回调的获取
 @param successCallback 成功回调
 @param errorCallback 失败回调
 @return  获取的 deviceToken 字符串
 */
+(nullable NSString *)registerDevice:(nonnull NSData *)deviceToken successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 注册设备并且设置账号

 @param deviceToken 通过app delegate的didRegisterForRemoteNotificationsWithDeviceToken回调的获取
 @param account 需要设置的账号,长度为2个字节以上，不要使用"test","123456"这种过于简单的字符串,
 				若不想设置账号,请传入nil
 @param successCallback 成功回调
 @param errorCallback 失败回调
 @return  获取的 deviceToken 字符串
 */
+(nullable NSString *)registerDevice:(nonnull NSData *)deviceToken account:(nullable NSString *)account successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 * deviceToken类型转换
 * @param deviceToken NSData格式的deviceToken
 * @return none
 */
+(nullable NSString *)getDeviceToken:(nonnull NSData *)deviceToken;

/**
 注册设备并且设置账号, 字符串 token 版本

 @param deviceToken  NSString *类型的 token
 @param account  需要设置的账号,若不想设置账号,请传入 nil
 @param successCallback  成功回调
 @param errorCallback 失败回调
 @return 获取的 deviceToken 字符串
 */
+(nullable NSString *)registerDeviceStr:(nonnull NSString *)deviceToken account:(nullable NSString *) account successCallback:(nullable void(^)(void)) successCallback errorCallback:(nullable void(^)(void))errorCallback;

#pragma mark- 帐号相关

/**
 设置设备的帐号. 设置账号前需要调用一次registerDevice

 @param account 需要设置的账号,长度为2个字节以上，不要使用"test","123456"这种过于简单的字符串,
 				若不想设置账号,请传入nil
 @param successCallback 成功回调
 @param errorCallback 失败回调
 */
+(void)setAccount:(nonnull NSString *)account successCallback:(nullable void(^)(void)) successCallback errorCallback:(nullable void(^)(void)) errorCallback;


/**
 删除已经设置的账号

 @param successCallback 成功回调
 @param errorCallback 失败回调
 */
+(void)delAccount:(nullable void(^)(void)) successCallback errorCallback:(nullable void(^)(void)) errorCallback;

#pragma mark- 标签相关

/**
 设置 tag

 @param tag 需要设置的 tag
 @param successCallback  成功回调
 @param errorCallback 失败回调
 */
+(void)setTag:(nonnull NSString *)tag successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 删除tag

 @param tag 需要删除的 tag
 @param successCallback  成功回调
 @param errorCallback 失败回调
 */
+(void)delTag:(nonnull NSString *)tag successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

#pragma mark- 推送效果:点击的统计
/**
 在didFinishLaunchingWithOptions中调用，用于推送反馈.(app没有运行时，点击推送启动时)
 
 @param launchOptions didFinishLaunchingWithOptions中的userinfo参数
 @param successCallback 成功回调
 @param errorCallback 失败回调
 */
+(void)handleLaunching:(nonnull NSDictionary *)launchOptions successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 在didReceiveRemoteNotification中调用，用于推送反馈。(app在运行时)

 @param userInfo 苹果 apns 的推送信息
 @param successCallback 成功回调
 @param errorCallback 失败回调
 */
+(void)handleReceiveNotification:(nonnull NSDictionary *)userInfo successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 * 获取userInfo里的bid信息(bid:信鸽的推送消息id)
 * @param userInfo 苹果apns的推送信息
 * @return none
 */
+(nullable NSString *)getBid:(nonnull NSDictionary *)userInfo;

#pragma mark- 获取自定义参数的方法
/* 注:如果使用了自定义参数，可以从handleLaunching的launchOptions和handleReceiveNotification的userInfo中获取  */

#pragma mark - 调试功能
/**
 获取当前设备的推送deviceToken,
 可以用于问题定位，建议开发人员埋点，方便产品经理和用户获取deviceToken，定位推送问题。
 
 @return 苹果下发的deviceToken
 */
+(nullable NSString *)getCurrentDeviceToken;

/**
 查询推送是否打开(以通知中心的弹窗是否开启为判断条件)
 
 @param result 查询回调.若推送打开,回调参数为 YES, 否则为 NO
 */
+(void)isPushOn:(nullable void(^)(BOOL isOn)) result;

#pragma mark- 其他功能

/**
 注销设备，设备不再进行推送
 如果只是注销帐号，推荐使用delAccount方法
 
 @param successCallback 成功回调
 @param errorCallback 失败回调
 */
+(void)unRegisterDevice:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback;

/**
 判断当前是否是已注销状态
 
 @return 是否已经注销推送
 */
+(BOOL)isUnRegisterStatus;





#pragma mark- 以下接口已经不在维护,请尽快替换
//以下接口已经不在维护,请尽快替换
+(void)localNotification:(nonnull NSDate *)fireDate alertBody:(nonnull NSString *)alertBody badge:(int)badge alertAction:(nonnull NSString *)alertAction userInfo:(nonnull NSDictionary *)userInfo DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用苹果提供的 API 删除本地推送");
+(void)localNotificationAtFrontEnd:(nonnull UILocalNotification *)notification userInfoKey:(nonnull NSString *)userInfoKey userInfoValue:(nonnull NSString *)userInfoValue  DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,若需要弹窗,请用 AlertViewController 手动弹出");
+(void)delLocalNotification:(nonnull NSString *)userInfoKey userInfoValue:(nonnull NSString *)userInfoValue DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用苹果提供的 API 删除本地推送");
+(void)delLocalNotification:(nonnull UILocalNotification *)myUILocalNotification DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用苹果提供的 API 删除本地推送");
+(void)clearLocalNotifications DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用苹果提供的 API 删除本地推送");
+(uint32_t)getAccessID DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用带回调的方法替代");
+(void)initForReregister:(nullable void (^)(void)) successCallback DEPRECATED_MSG_ATTRIBUTE("方法已经废弃,请不要使用此方法");
+(void)setAccount:(nonnull NSString *)account DEPRECATED_MSG_ATTRIBUTE("方法已经废弃,请使用带回调的方法替代");
+(nullable NSString *)registerDevice:(nonnull NSData *)deviceToken DEPRECATED_MSG_ATTRIBUTE("方法已经废弃,请使用带回调的方法替代");
+(nullable NSString *)registerDeviceStr:(nonnull NSString *)deviceToken DEPRECATED_MSG_ATTRIBUTE("方法已经废弃,请使用带回调的方法替代");
+(void)unRegisterDevice DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用带回调的方法替代");
+(void)setTag:(nonnull NSString *)tag DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用带回调的方法替代");
+(void)delTag:(nonnull NSString *)tag DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用带回调的方法替代");
+(void)handleReceiveNotification:(nonnull NSDictionary *)userInfo DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃");
+(void)handleReceiveNotification:(nonnull NSDictionary *)userInfo completion:(nullable void (^)(void)) completion  DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃");
+(void)handleReceiveNotification:(nonnull NSDictionary *)userInfo successCallback:(nullable void (^)(void)) successCallback errorCallback:(nullable void (^)(void)) errorCallback completion:(nullable void (^)(void)) completion  DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃");
+(void)handleLaunching:(nonnull NSDictionary *)launchOptions DEPRECATED_MSG_ATTRIBUTE("此方法已经废弃,请使用带回调的方法替代");

@end
