using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingPanel : UIPanel
{
    public Button btn_back; 
    public Button btn_backSelectRole;

    void Start()
    {
        btn_back.onClick.SetListener(() => {
            ExitPanel();
        });
        btn_backSelectRole.onClick.SetListener(() => {
            ExitPanel();
            RPGManager.Instance.ClearLoaclPlayer();
            uIState.ExitMapUIFramePanel();
            uIState.EnterSelectRolePanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
