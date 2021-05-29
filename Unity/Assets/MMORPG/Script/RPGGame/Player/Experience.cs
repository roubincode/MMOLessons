using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
//角色经验组件
[RequireComponent(typeof(Level))]
[DisallowMultipleComponent]
public class Experience : NetworkBehaviour
{
    /// <summary>
    /// 绑定角色等级组件
    /// </summary>
    public Level level;

    /// <summary>
    /// 角色当前等级下的现有经验值 current
    /// </summary>
    [SerializeField] long _current = 0;
    public long current
    {
        get { return _current; }
        set
        {
            if (value <= _current)
            {
                // =>写法一：约束当前等级的经验不能为负,也就是怎么扣经验都不会降级
                //_current = Math.Max(value, 0);

                // =>写法二：计算出降级后等级的总经验数值，减去负经验值即是降级后等级的current经验值。
                // 比如当前等级是4级，这级的current经验是20，死亡扣经验 20-50=-30
                // 降一级等级3级，3级总经验 144-30=114,降级后current经验是114
                if (value <0 && level.current>=2){
                    --level.current;
                    long ex = Convert.ToInt64(100*Mathf.Pow(1.13f,level.current));
                    _current = ex + value;
                }else _current = Math.Max(value, 0);  
            }else
            {
                _current = value;

                // 本级经验满，升级（多出来留到下一级）
                // 如果是达到最高等级则不能再升级
                while (_current >= max && level.current < level.max)
                {
                    _current -= max;
                    ++level.current;
                }
                if (_current > max) _current = max;
            }
        }
    }

    // 升级所需经验每级增加13%
    [SerializeField] protected ExponentialLong _max = new ExponentialLong{multiplier=100, baseValue=1.13f};
    public long max { get { return _max.Get(level.current); } }

    [Header("Death")]
    public float deathLossPercent = 0.05f;

    // helper functions ////////////////////////////////////////////////////////
    public float Percent() =>
        (current != 0 && max != 0) ? (float)current / (float)max : 0;

    // events //////////////////////////////////////////////////////////////////
    public virtual void OnDeath()
    {
        // 死亡失去经验，当然也可以设置死亡不掉经验，deathLossPercent属性设置为0
        current -= Convert.ToInt64(max * deathLossPercent);
    }
}

