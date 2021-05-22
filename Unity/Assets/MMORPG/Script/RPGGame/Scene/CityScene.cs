using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScene : SceneState
{
    public CityScene(UIFacade uIFacade) : base(uIFacade) { }
    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.MapUIFramePanel);
        mUIFacade.AddPanelToDict(StringManager.SettingPanel);
        base.EnterScene();

        mUIFacade.GetUI(StringManager.MapUIFramePanel).EnterPanel();     
    }

}
