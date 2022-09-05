using Fusion;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    // References to the NetworkObject prefab to be used for the players
    [SerializeField] private NetworkPrefabRef _characterNetworkPrefab = NetworkPrefabRef.Empty;

    public List<GameObject> _spawnPoints;
    private void Awake()
    {
        NetworkManager.SetSpawner(this);
    }
    public override void Spawned()
    {
        base.Spawned();
    }
    private void OnDestroy()
    {
        NetworkManager.SetSpawner(null);
    }

    public void SpawnPlayer(NetworkRunner runner, RoomPlayer player)
    {
        Debug.Log("IN INSTANTIATE METHOD");
    
            var index = RoomPlayer.Players.IndexOf(player);
            var point = _spawnPoints[index].transform;
        // Spawn player
       // if (player.Object.HasInputAuthority)
       // {
            var entity = runner.Spawn(
                _characterNetworkPrefab,
                point.position,
                point.rotation,
                player.Object.InputAuthority
            );
            Debug.Log("HALF DONEE--------------");
            Debug.Log(entity.transform + "+++++++++++++++++++++++++");
            Debug.Log(point + "+++++++++++++++++++++++++");
            entity.gameObject.transform.SetParent(point);
         

            Debug.Log("HALF DONEE 2--------------" + entity);

            player.GameState = RoomPlayer.EGameState.GameReady;

            Debug.Log("HALF DONEE 3--------------" + player);



            Debug.Log($"Spawning character for {player.Username} as {entity.name}");
            entity.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"({player.Username})";
           
            Debug.Log($"RESTTED");

      //  }
    }
}
