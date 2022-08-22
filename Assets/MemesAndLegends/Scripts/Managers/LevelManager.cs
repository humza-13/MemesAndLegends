using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

	public class LevelManager : NetworkSceneManagerBase
	{	
		public static LevelManager Instance => Singleton<LevelManager>.Instance;

		protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
		{
			Debug.Log($"Loading scene {newScene}");

			PreLoadScene(newScene);

			List<NetworkObject> sceneObjects = new List<NetworkObject>();

			if (newScene >= 1)
			{
				yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
				Scene loadedScene = SceneManager.GetSceneByBuildIndex(newScene);
				Debug.Log($"Loaded scene {newScene}: {loadedScene}");
				sceneObjects = FindNetworkObjects(loadedScene, disable: false);
			}

			finished(sceneObjects);

			// Delay one frame, so we're sure level objects has spawned locally
			yield return null;

			// Now we can safely spawn karts
			//if ( != null && newScene>LOBBY_SCENE)
			//{
			//	if (Runner.GameMode == GameMode.Host)
			//	{
			//		foreach (var player in RoomPlayer.Players)
			//		{
			//			player.GameState = RoomPlayer.EGameState.GameCutscene;
			//			GameManager.CurrentTrack.SpawnPlayer(Runner, player);
			//		}
			//	}
			//}

			PostLoadScene();
		}

		private void PreLoadScene(int scene)
		{
		
		
				//foreach (RoomPlayer player in RoomPlayer.Players)
				//{
				//	player.IsReady = false;
				//}
				
		
		
		}
	
		private void PostLoadScene()
		{
			
		}
	}
