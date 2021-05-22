using UnityEngine;
using UnityEngine.UI;
using System;
public partial class UITab : MonoBehaviour
{
    public Transform tab;
    public Sprite downSprite;
    public Sprite normalSprite;
    public Color downColor;
    public Color normalColor;
    public Action<string> action;

    // 刷新tab按钮事件状态
    // 此方法之所以交由外部调用（而不是放在本脚本的Start中自动调用），
    // 是因为有的tab按钮是动态生成的，存在用完清除的情况，这样当再次创建时需要调用。
    public void RefreshTab(){
        // 第一个tab为默认状态
        if(tab.childCount>0) {
            if(downSprite!=null) tab.GetChild(0).GetComponent<Image>().sprite = downSprite;
            else  tab.GetChild(0).GetComponent<Image>().color = downColor;
        }
        // 按钮按下时状态
        foreach(Transform go in tab){
            Button btn = go.gameObject.GetComponent<Button>();
            btn.onClick.SetListener(() => {
                NoneTabDown();

                if(downSprite!=null) go.GetComponent<Image>().sprite = downSprite;
                else go.GetComponent<Image>().color = downColor;

                btn.Select();
                if(action!=null) action(btn.name);
            });
        }
    }

    void NoneTabDown(){
        foreach(Transform go in tab){
            if(downSprite!=null) go.GetComponent<Image>().sprite = normalSprite;
            else go.GetComponent<Image>().color = normalColor;
        }
    }
}