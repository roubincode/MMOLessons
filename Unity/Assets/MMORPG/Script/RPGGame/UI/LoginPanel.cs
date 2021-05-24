using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoginPanel : UIPanel
{
    public Button btn_submit;
    public Button btn_register; 
    void Start()
    {
        btn_submit.onClick.SetListener(() => {
            // 判断登录成功
            // ...

            ExitPanel();
            uIState.EnterSelectRolePanel();
        });

        btn_register.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterRegisterPanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
