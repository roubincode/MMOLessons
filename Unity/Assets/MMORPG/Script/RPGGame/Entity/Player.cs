using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public partial class Player : Entity
{
    
    public long CharaId;
    public string nickName;
    /// <summary>
    /// 角色职业图标
    /// </summary>
    public Sprite classIcon; 
    /// <summary>
    /// 角色头像
    /// </summary>
    public Sprite portraitIcon;

    /// <summary>
    /// 全局本地玩家
    /// </summary>
    public static Player localPlayer;

    [TextArea(1, 30)] public string toolTip; 

    GameObject _nextTarget;
    public Entity nextTarget
    {
        get { return _nextTarget != null  ? _nextTarget.GetComponent<Entity>() : null; }
        set { _nextTarget = value != null ? value.gameObject : null; }
    }


    void Start()
    {
        // 暂时就一个角色放在场景里，localPlayer就是当前对象
        localPlayer = this;

        // Animator ani = GetComponent<Animator>();
        // foreach(AnimatorControllerParameter p in ani.parameters){
        //    // Debug.Log(p.name);
        // }
        
    }
    
    /// -><summary> 是否允许移动中释放技能</summary>
    public bool IsMovementAllowed()
    {
        // 当前技能允许移动中施放

        // 是否有任何输入
        return (state == "IDLE" || state == "MOVING") &&
               !UIUtils.AnyInputActive();
    }

    // death /////////////////////////////////////////////////////////////
    /// -><summary><c>OnDeath</c> 玩家死亡时调用的方法，重写了基类方法。</summary>
    public override void OnDeath()
    {
        //调用基类方法
        base.OnDeath();
        //输出自己的死亡宣言
        Debug.Log("光荣战死...");
    }
}

