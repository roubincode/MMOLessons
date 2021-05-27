using UnityEngine;
using UnityEngine.UI;

//角色生命与蓝条UI组件
public partial class UIAbility : MonoBehaviour
{
    public GameObject panel;
    public GameObject abilityContent;
    public GameObject exAbilityContent;

    UIAbilitySlot uiAbility;
    UIAbilitySlot uiExAbility;
    void Start(){
        RefreshAbility();
    }
    public void RefreshAbility(){
        ClearChild(abilityContent.transform);
        ClearChild(exAbilityContent.transform);

        Player player = Player.localPlayer;
        if (player !=null)
        {
            GameObject go = Instantiate(player.ability.abilitySlot.gameObject, abilityContent.transform, false);
            uiAbility = go.GetComponent<UIAbilitySlot>();
            GameObject goex = Instantiate(player.exAbility.abilitySlot.gameObject, exAbilityContent.transform, false);
            uiExAbility = goex.GetComponent<UIAbilitySlot>();
        }
    }
    void ClearChild(Transform content){
        foreach(Transform item in content)
            Destroy(item.gameObject);
    }
    void Update()
    {
        Player player = Player.localPlayer;
        if (player !=null)
        {
            panel.SetActive(true);
            if(uiAbility ==null) return;

            // 调用角色生命属性,当前生命与最大生命比值
            uiAbility.slider.fillAmount = player.ability.Percent();
            if(uiAbility.text!=null) uiAbility.text.text = player.ability.current + " / " + player.ability.max;

            // 调用角色蓝量属性,当前蓝量与最大蓝量比值
            uiExAbility.slider.fillAmount = player.exAbility.Percent();
            if(uiExAbility.text!=null) uiExAbility.text.text = player.exAbility.current + " / " + player.exAbility.max;
        }
        else panel.SetActive(false);
    }
}
