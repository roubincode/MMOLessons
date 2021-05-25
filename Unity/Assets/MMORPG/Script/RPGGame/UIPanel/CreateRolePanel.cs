using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
public class CreateRolePanel : UIPanel
{
    public Button btn_back; 
    public Button btn_submit;

    public Transform content;
    public UIRoleClassSlot slotPrefab;
    private UIRoleClassSlot defaultSlot;
    public InputField inputName;
    public Text info;

    private GameObject preGo;
    private string preName;
    private Transform viewRoot;

    // 玩家职业player类
    private Player[] classes;

    void Awake()
    {
        classes = RPGManager.Instance.playerClasses.ToArray();

        viewRoot = GameObject.Find("PreViewRoot").transform;
    }
    void Start()
    {
        btn_back.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterSelectRolePanel();
        });
        btn_submit.onClick.SetListener(() => {
            string nickName = Regex.Replace(inputName.text, @"\s", "");
            if(nickName =="") return;
            Player player = classes.ToList().Find(p => p.ClassName == preName);
            RPGManager.Instance.selectName = inputName.text;
            RPGManager.Instance.playerList.Add(player);
            RPGManager.Instance.playerNicks.Add(nickName);

            inputName.text = "";

            ExitPanel();
            uIState.EnterSelectRolePanel();
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();

        // 动态生成内容不放在Start方法中，要放在EnterPanel方法中
        InitPreview();
        UITab uiTab = GetComponent<UITab>();
        uiTab.action = PreviewRole;
        uiTab.RefreshTab();
        // 默认预览职业与预览相机位置
        RPGManager.Instance.CamLocation(RPGManager.Instance.create_camLoaction);
        PreviewRole(classes[0].ClassName);
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        // 移除动态内容
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
            go.name = player.ClassName;

            if(copy==0) defaultSlot = slot;
            // posY位置
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -copy);
            copy += 55;

            // 缓存职业角色
            GameObject preGo =  RPGManager.Instance.CreateItem(player.gameObject);
            preGo.transform.parent = viewRoot;
            preGo.transform.position = RPGManager.Instance.create_spawnLoaction.position;
            preGo.transform.rotation = RPGManager.Instance.create_spawnLoaction.rotation;
            
            preGo.name = player.ClassName;
            preGo.SetActive(false);
        }
    }

    void ClearPreview(){
        for (int i = viewRoot.childCount - 1; i >= 0; i--) {
            Destroy(viewRoot.GetChild(i).gameObject);
            Destroy(content.GetChild(i).gameObject);
        }
    }

    void PreviewRole(string className){
        if(preGo!=null) preGo.SetActive(false);
        preGo = viewRoot.Find(className).gameObject;
        preName = className;
        preGo.SetActive(true);
        info.text = preGo.GetComponent<Player>().toolTip;
    }
}
