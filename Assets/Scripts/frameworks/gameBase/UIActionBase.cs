//==============================================================
//create by littlefox at 2018/4/23 14:19:24
//==============================================================

using System;
using UnityEngine;

public class UIActionBase : MonoBehaviour
{
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {

    }

    /// <summary>
    /// 刷新
    /// </summary>
    /// <param name="args"></param>
    public virtual void Refresh(params object[] args)
    {

    }

    /// <summary>
    /// 释放=Destroy
    /// </summary>
    public virtual void Release()
    {

    }

    /// <summary>
    /// 获取刷新的参数，简易方法，自动报错，省判断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="args">参数集合</param>
    /// <param name="index">index</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    protected T GetRefreshArg<T>(object[] args, int index, T defaultValue = default(T))
    {
        T t = defaultValue;

        try
        {
            if (args != null && args.Length > index)
            {
                t = (T)args[index];
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return t;
    }
}
