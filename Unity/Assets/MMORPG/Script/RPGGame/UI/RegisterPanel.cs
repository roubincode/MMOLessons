using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegisterPanel : UIPanel
{
    public Button btn_submit;
    public Button btn_back; 
    void Start()
    {
        btn_back.onClick.SetListener(() => {
            ExitPanel();
            uIState.EnterLoginPanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
