using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScene : SceneState
{
    public CityScene(UIFacade uIFacade) : base(uIFacade) { }
    public override void EnterScene()
    {
        //mUIFacade.AddPanelToDict(StringManager.LoginPanel);
        //mUIFacade.AddPanelToDict(StringManager.RegisterPanel);
        base.EnterScene();
    }

}
