using UnityEngine;

// 能量组件
[DisallowMultipleComponent]
public class Energy : ExAbility
{

    // 如果设置为非重生满能力值，这里设置重生血量为20
    void Start(){
        if(!spawnFull) current = 20;
    }

}