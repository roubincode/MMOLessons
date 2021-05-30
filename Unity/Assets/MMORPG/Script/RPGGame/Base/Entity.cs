using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
//Player，NPC，Monster等实体的父对象类
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))] 
[RequireComponent(typeof(AudioSource))]

[Serializable] public class UnityEventEntity : UnityEvent<Entity> {}
public abstract partial class Entity : NetworkBehaviour
{
    public long UnitId;
    public string CNName;
    public string ClassName;

    [Header("Components")]
    public Animator animator;
    new public Collider collider;
    public AudioSource audioSource;
    public Movement movement;

    /// <summary>
    /// 角色等级  
    /// </summary>
    public Level level;
    /// <summary>
    /// 角色主能力（生命）
    /// </summary>
    public Ability ability;
    /// <summary>
    /// 角色扩展能力（蓝，怒气，能量，护甲生命）  
    /// </summary>
    public ExAbility exAbility;  

    /// <summary>
    /// 角色战斗或脱战状态  
    /// </summary>
    public bool inbattle = false; 

    [SerializeField] long _gold = 0;
    /// <summary>
    /// 角色拥有的金币  
    /// </summary>
    public long gold { 
        get { return _gold; } 
        set { _gold = Math.Max(value, 0); }
    }

    
    [SerializeField] string _state = "IDLE";
    public string state => _state;
    
    GameObject _target;
    /// <summary>
    /// 角色目标 
    /// </summary>
    public Entity target
    {
        get { return _target != null  ? _target.GetComponent<Entity>() : null; }
        set { _target = value != null ? value.gameObject : null; }
    }

    void Update()
    {
        
    }

    // death /////////////////////////////////////////////////////////////
    /// -><summary><c>OnDeath</c> 角色死亡时可以被子类调用的虚方法,
    /// 清除角色的目标属性值。</summary>
    public virtual void OnDeath()
    {
        //调用死亡动画，声音，躺了
        Debug.Log("啊!我躺了"); //暂时用在输出面板打印一句话代替
        // 清除目标
        target = null;
    }

    // Revive ///////////////////////////////////////////////////////////////////
    public void Revive(float healthPercentage = 1,float manaPercentage = 1)
    {
        
    }
}
