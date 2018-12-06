//
//  WXApiObject+Private.h
//  WeChatSDK
//
//  Created by liuyunxuan on 2017/2/6.
//
//

/*! @brief 第三方向微信终端发起拆企业红包的消息结构体
 *
 *  第三方向微信终端发起拆企业红包的消息结构体，微信终端处理后会向第三方返回处理结果
 * @see HBReq
 */
@interface HBReq : BaseReq

/** 随机串，防重发 */
@property (nonatomic, retain) NSString *nonceStr;
/** 时间戳，防重发 */
@property (nonatomic, assign) UInt32 timeStamp;
/** 商家根据微信企业红包开发文档填写的数据和签名 */
@property (nonatomic, retain) NSString *package;
/** 商家根据微信企业红包开发文档对数据做的签名 */
@property (nonatomic, retain) NSString *sign;

@end



#pragma mark - HBResp
/*! @brief 微信终端返回给第三方的关于拆企业红包结果的结构体
 *
 *  微信终端返回给第三方的关于拆企业红包结果的结构体
 */
@interface HBResp : BaseResp

@end


#pragma mark - WXHandleScanResultReq
/** ! @brief 调用微信处理扫码结果
 */
@interface WXHandleScanResultReq : BaseReq
@property (nonatomic, strong) NSString *scanResult;
@end


#pragma mark - CreateChatRoomReq
/*! @brief 游戏方公会建群请求
 *
 * 第三方程序向微信终端发送游戏方公会建群请求
 */
@interface CreateChatRoomReq : BaseReq
/** 公会id
 * @attention 长度不能超过1024字节
 */
@property (nonatomic, retain) NSString *groupId;

/** 公会名称(群名称)
 * @attention 长度不能超过512字节
 */
@property (nonatomic, retain) NSString *chatRoomName;

/** 玩家名称(群里展示)
 * @attention 长度不能超过512字节
 */
@property (nonatomic, retain) NSString *chatRoomNickName;

/** 第三方程序自定义简单数据，微信终端会回传给第三方程序处理
 * @attention 长度不能超过2k
 */
@property (nonatomic, retain) NSString *extMsg;

@end

#pragma mark - CreateChatRoomResp
/*! @brief 微信终端向第三方程序返回的CreateChatRoomReq处理结果。
 *
 * 第三方程序向微信终端发送CreateChatRoomReq后，微信发送回来的处理结果，该结果用CreateChatRoomResp表示。
 */
@interface CreateChatRoomResp : BaseResp

@property (nonatomic, retain) NSString *extMsg;

@end

#pragma mark - JoinChatRoomReq
/*! @brief 游戏方公会加群请求
 *
 * 第三方程序向微信终端发送游戏方公会加群请求
 */
@interface JoinChatRoomReq : BaseReq

/** 公会id
 * @attention 长度不能超过1024字节
 */
@property (nonatomic, retain) NSString *groupId;

/** 公会名称(群名称)
 * @attention 长度不能超过512字节
 */
@property (nonatomic, retain) NSString *chatRoomNickName;

/** 第三方程序自定义简单数据，微信终端会回传给第三方程序处理
 * @attention 长度不能超过2k
 */
@property (nonatomic, retain) NSString *extMsg;
@end

#pragma mark - JoinChatRoomResp
/*! @brief 微信终端向第三方程序返回的JoinChatRoomReq处理结果。
 *
 * 第三方程序向微信终端发送JoinChatRoomReq后，微信发送回来的处理结果，该结果用JoinChatRoomResp表示。
 */
@interface JoinChatRoomResp : BaseResp
/** 第三方程序自定义简单数据，微信终端会回传给第三方程序处理
 * @attention 长度不能超过2k
 */
@property (nonatomic, retain) NSString *extMsg;
@end

#pragma mark - WXVideoFileObject
/*! @brief 多媒体消息中包含的文件数据对象
 *
 * @see WXMediaMessage
 */
@interface WXVideoFileObject : NSObject

/*! @brief 返回一个WXVideoFileObject对象，目前只支持分享朋友圈的场景
 *
 * @note 返回的WXVideoFileObject对象是自动释放的
 */
+(WXVideoFileObject *) object;

/** 文件真实数据内容
 * @note 大小不能超过10M，时间不超过10s
 */
@property (nonatomic, retain) NSData  *videoFileData;


@end

#pragma mark - WXVideoFileObject
/*! @brief 多媒体消息中包含的文件数据对象
 *
 * @see WXMediaMessage
 */
@interface WXGameVideoFileObject : NSObject

/*! @brief 返回一个WXGameVideoFileObject对象，目前只支持分享朋友圈的场景
 *
 * @note 返回的WXGameVideoFileObject对象是自动释放的
 */
+(WXGameVideoFileObject *) object;

/** 文件真实数据内容
 * @note 大小不能超过10M，时间不超过10s
 */
@property (nonatomic, retain) NSData  *videoFileData;


/** video的url
 * @note
 */
@property (nonatomic, retain) NSString  *videoUrl;

/** 缩略图的url
 * @note
 */
@property (nonatomic, retain) NSString  *thumbUrl;

@end
