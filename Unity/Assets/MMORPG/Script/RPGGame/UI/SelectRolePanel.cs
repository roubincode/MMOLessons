using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectRolePanel : UIPanel
{
    public Button btn_back; 
    public Button btn_create;
    public Button btn_enter;
    void Start()
    {
        btn_back.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterLoginPanel();
        });
        btn_create.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterCreateRolePanel();
        });
        btn_enter.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterMapUIFramePanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
