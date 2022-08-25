using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Device;
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
        yield return new WaitForEndOfFrame();

        if (NetworkManager.Spawner != null && newScene > 1)
        {
            Debug.Log("+++++++++++++++++++++++++++" + NetworkManager.Spawner);
            if (Runner.GameMode == GameMode.Host)
            {
                foreach (var player in RoomPlayer.Players)
                {
                    player.GameState = RoomPlayer.EGameState.GameReady;
                    NetworkManager.Spawner.SpawnPlayer(NetworkManager.Runner, player);
                }
            }
        }
    }
    private void PreLoadScene(int scene)
    {
        if (scene > 1)
        {
            // Show an empty dummy UI screen - this will stay on during the game so that the game has a place in the navigation stack. Without this, Back() will break
            Debug.Log("Showing Dummy");
        
        }
        else if (scene == 1)
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
    public static void LoadGameScene(int sceneIndex)
    {
        Instance.Runner.SetActiveScene(sceneIndex);
    }
}
