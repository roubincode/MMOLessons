using UnityEngine;

// 怒气组件
[DisallowMultipleComponent]
public class Rage : ExAbility
{
    
    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}