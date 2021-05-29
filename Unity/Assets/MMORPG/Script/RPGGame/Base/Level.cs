using UnityEngine;
using Mirror;
//角色等级组件，经验的增长改变等级
[DisallowMultipleComponent]
public class Level : NetworkBehaviour
{
    /// <summary>
    /// 角色当前等级
    /// </summary>
    public int current = 1;
    /// <summary>
    /// 角色最高等级
    /// </summary>
    public int max = 1;

    //编辑器模式下OnValidate 仅在下面两种情况下被调用：
    //脚本被加载时
    //Inspector 中的任何值被修改时
    void OnValidate()
    {
        current = Mathf.Clamp(current, 1, max);
    }
}