using Fusion;
using System.Resources;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    // References to the NetworkObject prefab to be used for the players
    [SerializeField] private NetworkPrefabRef _characterNetworkPrefab = NetworkPrefabRef.Empty;

    public GameObject[] _spawnPoints = null;
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
        var index = RoomPlayer.Players.IndexOf(player);
        var point = _spawnPoints[index].transform;
        // Spawn player
        var entity = runner.Spawn(
            _characterNetworkPrefab,
            point.position,
            point.rotation,
            player.Object.InputAuthority
        );

       
        player.GameState = RoomPlayer.EGameState.GameReady;

        Debug.Log($"Spawning character for {player.Username} as {entity.name}");
        entity.transform.name = $"character ({player.Username})";
    }
}
