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

    // Update is called once per frame
    void Update()
    {
        
    }
}
