
/// <summary>
/// Panel接口
/// </summary>
public interface IBasePanel
{
    void InitPanel();    // 初始化UI面板的方法
    void EnterPanel();   // 进入UI面板时触发的事件
    void ExitPanel();    // 离开UI面板时触发的事件
    void UpdatePanel();  // 更新UI内容的方法
    void BackToPanel(); // 返回之前的Panel
}
