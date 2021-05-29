using UnityEngine;
using UnityEngine.UI;
using System;

//角色经验条UI
//能否切换显示角色某声望条，技能修练条呢
public partial class UIExperienceBar : MonoBehaviour
{
    public GameObject panel;
    public Image slider;
    public Text statusText;

    void Start(){
       
    }

    void Update()
    {
        Player player = Player.localPlayer;
        if (player !=null)
        {
            
            panel.SetActive(true);
            slider.fillAmount = player.experience.Percent();
            statusText.text = "Lv." + player.level.current + " (" + (player.experience.Percent() * 100).ToString("F2") + "%)";
            
            // ToString 格式符说明：Axx，其中 A 为格式说明符，指定格式化类型，
            // xx 为精度说明符，控制格式化输出的有效位数或小数位数
            // F2 即是固定点，2位小数
        }
        else panel.SetActive(false);
    }
}
