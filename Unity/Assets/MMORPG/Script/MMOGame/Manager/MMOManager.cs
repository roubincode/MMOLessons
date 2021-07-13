using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace MMOGame
{
    public class MMOManager : MonoBehaviour, IManager
    {
        public MMOManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null) Instance = this;
            RPGManager.MMOMgr = Instance as IManager;
        }

        public List<Entity> FindEntityClasses()
        {
            UnityEngine.Object[] prefabs = UnitResources.GetAll("unit.unity3d");
            List<Entity> entities = new List<Entity>();
            foreach (GameObject item in prefabs)
            {
                Entity entity = item.GetComponent<Entity>();
                if (entity)
                    entities.Add(entity);
            }
            return entities;
        }
        public List<Player> FindPlayerClasses()
        {
            UnityEngine.Object[] prefabs = UnitResources.GetAll("unit.unity3d");
            List<Player> players = new List<Player>();
            foreach (GameObject item in prefabs)
            {
                Player player = item.GetComponent<Player>();
                if (player)
                    players.Add(player);
            }
            return players;
        }
    }
}
