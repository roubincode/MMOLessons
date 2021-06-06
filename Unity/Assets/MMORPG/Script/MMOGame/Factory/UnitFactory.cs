using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UnitFactory
    {
        public static Unit Create(long id, GameObject go)
        {
	        Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(id, go);
			UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            unitComponent.Add(unit);
            return unit;
        }

        public static void Remove(Unit unit){
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            unitComponent.Remove(unit.Id);
            unit.Dispose();
        }
    }
}