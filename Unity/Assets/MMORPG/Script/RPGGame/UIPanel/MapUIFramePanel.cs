using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapUIFramePanel : UIPanel
{
    public Button btn_setting;
    void Start()
    {
        btn_setting.onClick.SetListener(() => {
            uIState.EnterSettingPanel();
        });
    }

    public override void EnterPanel()
    {
        GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        base.EnterPanel();

        RPGManager.Instance.CreateLocalPlayer();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
    }
}
