using UnityEngine;
using System;
using Mirror;

//角色能力基本类，派生类有Mana,Health,Combat等
public abstract class Ability : NetworkBehaviour
{
    public UIAbilitySlot abilitySlot;
    int _current = 0;
    /// <summary>
    /// 能力值的当前数值
    /// </summary>
    public int current
    {
        get { return Mathf.Min(_current, max); }
        set
        {
            _current = Mathf.Clamp(value, 0, max);
        }
    }
    /// <summary>
    /// 角色重生后,能力值是否立即为满值
    /// </summary>
    public bool spawnFull = true;
    /// <summary>
    /// 能力值的最大值,基类定义为抽象属性
    /// </summary>
    public abstract int max { get; }

    /// <summary>
    /// 能力组件中获取所属角色Entity组件
    /// </summary>
    Entity entity;
    /// <summary>
    /// 获取所属角色的Health组件,public但不序列化
    /// </summary>
    [NonSerialized]
    public Health health;
    /// <summary>
    /// 获取所属角色的Level组件,public但不序列化
    /// </summary>
    [NonSerialized]
    public Level level;

    ////////////////////////////////////////////////////////////////////////////
    // 能力值自动恢复相关的属性
    // -> 通过这些属性可以决定此项能力值是否可自动恢复，恢复间隔，单位时间恢复量
    /// <summary>
    /// 可否自动恢复
    /// </summary>
    public bool canRecover = false;
    /// <summary>
    /// 单位时间恢复数量，基类定义为抽象属性
    /// </summary>
    public abstract int recovery { get; }
    /// <summary>
    /// 脱战恢复间隔
    /// </summary>
    public int timeRate = 2;
    /// <summary>
    /// 战斗恢复间隔
    /// </summary>
    public int battleTimeRate = 5;
    /// <summary>
    /// 用于比较角色战斗状态
    /// </summary>
    bool sts = true;

    void Awake(){
        entity = GetComponent<Entity>();
        health = GetComponent<Health>();
        level = GetComponent<Level>();

        // 重生时满数值
        if (spawnFull) current = max;
    }

    // Percent /////////////////////////////////////////////////////////////
    /// -> <summary>获取当前值与最大值的百分比。</summary>
    public float Percent() =>
        (current != 0 && max != 0) ? (float)current / (float)max : 0;


    void Update()
    {
        // 如果能力值可恢复，并且当前能力值小于最大能力值
        if(canRecover && current < max){
            // 如果战斗状态发生改变，调用DoRepeat方法
            if(sts != entity.inbattle){
                sts = entity.inbattle;
                DoRepeat("Recover");
            } 
        }
    }

    // DoRepeat /////////////////////////////////////////////////////////////
    /// -> <summary>根据战斗状态切换恢复间隔频率，执行InvokeRepeating。</summary>
    private void DoRepeat(string recover){
        // 根据战斗状态切换恢复间隔频率
        int rate;
        if(sts) rate = battleTimeRate;
        else rate = timeRate;
        
        // 取消已有的Invoke方法，并用新的间隔频率执行InvokeRepeating
        if(IsInvoking(recover))
            CancelInvoke(recover);
        InvokeRepeating(recover, 0, rate);     
    }

    // Recover /////////////////////////////////////////////////////////////
    /// -> <summary>自动恢复，间隔调用的方法。</summary>
    public virtual void Recover()
    {
        if (enabled && health.current > 0)
            current += recovery;
    }

}
