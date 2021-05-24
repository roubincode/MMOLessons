using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : MonoBehaviour
{
    public UIPanel login;
    public UIPanel register;
    public UIPanel selectRole;
    public UIPanel createRole;
    public UIPanel mapUIFrame;
    public UIPanel setting;

    void Start()
    {
        // 插曲练习：比如 123123123 这样一个整数，如何把其中每一位数字作为一个元素放到一个数组中。
        // 不看下面的答案自己写出来（新人一定要多练这样的，才能解决明明有思路但不会用代码表达出来）
        // List<int> list = new List<int>();
        // int ss = 123123123;
        // int lth = ss.ToString().Length;

        // for(int i = 0;i<lth;i++){
        //     int a = ss%10; // 余
        //     ss = ss/10;  // 商
        //     list.Add(a);
        // }

        // foreach(int item in list.ToArray()){
        //     Debug.Log(item);
        // }
        
    }

    public void EnterSelectRolePanel(){
        selectRole.EnterPanel();
    }

    public void EnterCreateRolePanel(){
        createRole.EnterPanel();
    }

    public void EnterRegisterPanel(){
        register.EnterPanel();
    }
    public void EnterSettingPanel(){
        setting.EnterPanel();
    }

    public void EnterLoginPanel(){
        login.EnterPanel();
    }

    public void EnterMapUIFramePanel(){
        mapUIFrame.EnterPanel();
    }
    public void ExitMapUIFramePanel(){
        mapUIFrame.ExitPanel();
    }

    public static char[] getChar(string s){
        return s.ToCharArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
