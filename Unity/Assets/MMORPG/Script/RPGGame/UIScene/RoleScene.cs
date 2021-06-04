using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleScene : UIScene
{
    public RoleScene(UIFacade uIFacade) : base(uIFacade) { }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.SelectRolePanel);
        mUIFacade.AddPanelToDict(StringManager.CreateRolePanel);

        
        base.EnterScene();
        
        // 打开SelectRolePanel
        mUIFacade.GetUI(StringManager.SelectRolePanel).EnterPanel();
        Camera.main.GetComponent<Animator>().enabled = false;
    }

}
