using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

	public class LevelManager : NetworkSceneManagerBase
	{
		public const int LAUNCH_SCENE = 0;
		public const int HOME_SCENE = 1;
		
		public static LevelManager Instance => Singleton<LevelManager>.Instance;
		
		public static void LoadMenu()
		{
			Instance.Runner.SetActiveScene(HOME_SCENE);
		}

		public static void LoadTrack(int sceneIndex)
		{
			Instance.Runner.SetActiveScene(sceneIndex);
		}
		
		protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
		{
			Debug.Log($"Loading scene {newScene}");

			PreLoadScene(newScene);

			List<NetworkObject> sceneObjects = new List<NetworkObject>();

			if (newScene >= HOME_SCENE)
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
			if (scene > HOME_SCENE)
			{
			
			
			}
			else if(scene==HOME_SCENE)
			{
				foreach (RoomPlayer player in RoomPlayer.Players)
				{
					player.IsReady = false;
				}
				
			}
			else
			{
				
			}
		
		}
	
		private void PostLoadScene()
		{
			
		}
	}
