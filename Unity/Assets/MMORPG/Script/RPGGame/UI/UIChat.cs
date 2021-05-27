using UnityEngine;
using UnityEngine.UI;

public partial class UIChat : MonoBehaviour
{
    public static UIChat singleton;
    public GameObject panel;
    public InputField messageInput;
    public Button sendButton;
    public Transform content;
    public ScrollRect scrollRect;

    [Header("Channels")]
    public ChannelInfo worldChannel = new ChannelInfo("/g", "", "[世界]", null);
    public ChannelInfo miChannel = new ChannelInfo("/m", "", "[私聊]", null);
    public ChannelInfo teamChannel = new ChannelInfo("/t", "", "[队伍]", null);
    public ChannelInfo partyChannel = new ChannelInfo("/p", "", "[公会]", null);
    public Transform tab;
    private ChannelInfo localChannel;

    public KeyCode[] activationKeys = {KeyCode.Return, KeyCode.KeypadEnter};
    public int keepHistory = 100; 

    bool eatActivation;

    public UIChat()
    {
        if (singleton == null) singleton = this;
    }

    void Start(){
        localChannel = worldChannel;
        SwitchChannel("W");
        messageInput.textComponent.color = localChannel.textColor;

        UITab uiTab = GetComponent<UITab>();
        uiTab.action = SwitchChannel;
        uiTab.RefreshTab();
    }
    
    void SwitchChannel(string chl){
        switch(chl){
            case "W" :
                localChannel = worldChannel;
            break;
            case "M" :
                localChannel = miChannel;
            break;
            case "T" :
                localChannel = teamChannel;
            break;
            case "P" :
                localChannel = partyChannel;
            break;
        }
        // 刷新输入区文字颜色
        messageInput.textComponent.color = localChannel.textColor;
    }
    void Update()
    {
        Player player = Player.localPlayer;
        if (player)
        {
            panel.SetActive(true);

            // character limit
            messageInput.characterLimit = 40;

            if (Utils.AnyKeyDown(activationKeys) && !eatActivation)
                messageInput.Select();
            eatActivation = false;

            messageInput.onEndEdit.SetListener((value) => {
                if (Utils.AnyKeyDown(activationKeys)) {
                    //发送聊天消息
                    AddMessage(new ChatMessage(player.nickName, localChannel.identifierOut, value,  localChannel.textPrefab));
                    messageInput.text = "";
                    messageInput.MoveTextEnd(false);
                    eatActivation = true;
                }

                UIUtils.DeselectCarefully();
            });
        }
        else panel.SetActive(false);
    }

    void AutoScroll()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }

    public void AddMessage(ChatMessage message)
    {
        if (content.childCount >= keepHistory) {
            for (int i = 0; i < content.childCount / 2; ++i)
                Destroy(content.GetChild(i).gameObject);
        }

        // instantiate and initialize text prefab with parent
        GameObject go = Instantiate(message.textPrefab, content.transform, false);
        go.GetComponent<Text>().text = message.Construct();
        go.GetComponent<UIMessageSlot>().message = message;

        AutoScroll();
    }

    void MoveTextEnd()
    {
        messageInput.MoveTextEnd(false);
    }
}
