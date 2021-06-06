using System.Collections.Generic;
using UnityEngine;
using MMOGame;
using ETModel;
// MMOManager单例，对网络层的管理与调用
namespace Mirror
{
    
    public partial class MMOManager : MonoBehaviour,IMMOManager
    {
        public static MMOManager Instance { get; private set; }
        
        public void Awake()
        {
            if (Instance == null) Instance = this;
            RPGManager.MMOMgr = Instance as IMMOManager;
        }

        public void Start(){
            
        }

        /// 从资源中取得玩家,怪物等角色预制对象
        public List<Entity> FindEntityClasses()
        {
			GameObject[] prefabs = UnitResources.GetAll("Unit");

            List<Entity> classes = new List<Entity>();
            foreach (GameObject prefab in prefabs)
            {
                Entity entity = prefab.GetComponent<Entity>();
                if (entity != null)
                    classes.Add(entity);
            }
            return classes;
        }

        public List<Player> FindPlayerClasses()
        {
			GameObject[] prefabs = UnitResources.GetAll("Unit");

            List<Player> classes = new List<Player>();
            foreach (GameObject prefab in prefabs)
            {
                Player player = prefab.GetComponent<Player>();
                if (player != null)
                    classes.Add(player);
            }
            return classes;
        }

    }
}