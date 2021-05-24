using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateRolePanel : UIPanel
{
    public Button btn_back; 
    public Button btn_submit;

    void Start()
    {
        btn_back.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterSelectRolePanel();
        });
        btn_submit.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterSelectRolePanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
