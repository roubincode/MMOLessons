using System;
using System.Threading;
using UnityEngine;
using ETModel;
using Mirror;
namespace MMOGame{
	public class Init : MonoBehaviour
	{
		public void Start()
        {
			Time.fixedDeltaTime = 1f / 60;
			
			this.StartAsync().Coroutine();
        }
		
		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				ClientConfigHelper.SetConfigHelper();
				Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
				Game.EventSystem.Add(DLLType.Model, typeof(Model).Assembly);

				Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<GlobalConfigComponent>();
				Game.Scene.AddComponent<NetOuterComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();

				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();

				// 实体管理组件
				Game.Scene.AddComponent<UnitComponent>();
				Game.Scene.AddComponent<UIComponent>();

				// 下载ab包
				await BundleHelper.DownloadBundle();

				// 加载配置
				Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");

				// 加载角色unit资源
				Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("unit.unity3d");

				// 网络层调用游戏层，只需要获取到对象就能调用
				// RPGManager.Instance.playerClasses = MMOManager.Instance.FindPlayerClasses();

				UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
				Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void FixedUpdate()
        {
            Game.EventSystem.FixedUpdate();
        }

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}