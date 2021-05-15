using UnityEngine;
using UnityEngine.UI;
using System.Linq;
/// <summary>
/// 新建角色面板
/// </summary>
public class CreateRolePanel : BasePanel
{
    public Button btn_Submit;
    public Button btn_back;
    public Transform content;
    public UIRoleClassSlot slotPrefab;

    private Transform viewRoot;
    private GameObject preGo;
    private string preName;
    private UIRoleClassSlot defaultSlot;

    // 玩家职业player类
    private Player[] classes;

    protected override void Awake()
    {
        base.Awake();
        thisPanel = PanelType.CreateRole;

        classes = RPGManager.Instance.playerClasses.ToArray();

        viewRoot = GameObject.Find("PreViewRoot").transform;
    }

    void Start(){
        btn_back.onClick.SetListener(() => {
            base.BackToPanel();
        });
        btn_Submit.onClick.SetListener(() => {
            Player player = classes.ToList().Find(p => p.ClassName == preName);
            RPGManager.Instance.playerList.Add(player);
            base.BackToPanel();
        });
    }

    void PreviewRole(string className){
        if(preGo!=null) preGo.SetActive(false);
        preGo = viewRoot.Find(className).gameObject;
        preName = className;
        preGo.SetActive(true);
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();

        InitPreview();

        // 默认预览职业与预览相机位置
        RPGManager.Instance.CamLocation(RPGManager.Instance.role_PreviewCamLoaction);
        PreviewRole(classes[0].ClassName);
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        ClearPreview();
    }

    void InitPreview(){
        // 生成可选职业列表
        float copy = 0;
        foreach(Player player in classes){
            GameObject go = GameObject.Instantiate(slotPrefab.gameObject, content, false);
            UIRoleClassSlot slot = go.GetComponent<UIRoleClassSlot>();
            slot.nameText.text = player.CNName;
            slot.image.sprite = player.portraitIcon;

            if(copy==0) defaultSlot = slot;

            // 选定职业点击事件
            slot.button.onClick.SetListener(() => {
                PreviewRole(player.ClassName);     
            });

            // posY位置
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -copy);
            copy += 55;

            // 缓存职业角色
            GameObject preGo =  RPGManager.Instance.CreateItem(player.gameObject);
            preGo.transform.parent = viewRoot;
            preGo.transform.position = RPGManager.Instance.role_PreviewLoaction.position;
            preGo.transform.rotation = RPGManager.Instance.role_PreviewLoaction.rotation;
            preGo.GetComponent<CharacterMovement>().enabled = false;
            preGo.name = player.ClassName;
            preGo.SetActive(false);
        }
    }

    void ClearPreview(){
        for (int i = viewRoot.childCount - 1; i >= 0; i--) {
            Destroy(viewRoot.GetChild(i).gameObject);
        }
    }

}