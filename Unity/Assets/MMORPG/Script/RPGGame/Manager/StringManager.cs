/// <summary>
/// 字符串常量的管理者,存储常用到的字符常量
/// </summary>
public class StringManager
{
    // UIPanel常量
    public const string LoadScenePanel = "LoadScene";
    public const string CreateRolePanel = "CreateRole";
    public const string SelectRolePanel = "SelectRole";
    public const string LoginPanel = "Login";
    public const string RegisterPanel = "Register";
    public const string SetPanel = "Setting";
    public const string HelpPanel = "Help";
}

public enum PanelType{
    Login,
    Register,
    Setting,
    SelectRole,
    CreateRole,
    Help,
    Null
}