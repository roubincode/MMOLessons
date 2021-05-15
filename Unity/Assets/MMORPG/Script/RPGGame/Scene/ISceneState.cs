
/// <summary>
/// 场景状态的接口
/// </summary>
public interface ISceneState
{
    void EnterScene(); //进入该场景监听的事件
    void ExitScene(); // 离开该场景监听的事件
}
