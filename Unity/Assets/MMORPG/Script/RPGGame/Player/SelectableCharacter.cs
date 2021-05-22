using UnityEngine;

[DisallowMultipleComponent]
public class SelectableCharacter : MonoBehaviour
{
    public  bool selected ;
    private Player player;
    void Awake(){
        player = GetComponent<Player>();

        RPGManager.Instance.NoneSelectd();
        RPGManager.Instance.selectClass = player.ClassName;
        selected  = true;
    }
    void OnMouseDown()
    {
        if(Utils.IsCursorOverUserInterface()) return;
        RPGManager.Instance.NoneSelectd();
        RPGManager.Instance.selectClass = player.ClassName;
        RPGManager.Instance.selectName = player.nickName;
        selected  = true;
        
        Debug.Log(player.nickName);
    }

    void Update()
    {
        // 移除选中标识
        // ...
    }
}
