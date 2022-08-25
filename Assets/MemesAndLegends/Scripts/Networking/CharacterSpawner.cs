using Fusion;
using UnityEngine;

public class CharacterSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft, ISpawned
{
    // References to the NetworkObject prefab to be used for the players
    [SerializeField] private NetworkPrefabRef _characterNetworkPrefab = NetworkPrefabRef.Empty;

    public GameObject[] _spawnPoints = null;
    
    public void Spawned()
    {
        if (Object.HasStateAuthority == false) return;
    }
    
    public void StartSpawner()
    {
        
        foreach (var player in Runner.ActivePlayers)
        {
            SpawnCharacter(player);
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
       
        SpawnCharacter(player);   
    }
    
    
    private void SpawnCharacter(PlayerRef player)
    {
        int index = player % _spawnPoints.Length;
        var spawnPosition = _spawnPoints[index].transform.position; 
        
        var playerObject = Runner.Spawn(_characterNetworkPrefab, spawnPosition, Quaternion.identity, player);
        // Set Player Object to facilitate access across systems.
        Runner.SetPlayerObject(player, playerObject);
        
    }
    
    // Despawns the spaceship associated with a player when their client leaves the game session.
    public void PlayerLeft(PlayerRef player)
    {
        DespawnSpaceship(player);
    }
    
    private void DespawnSpaceship(PlayerRef player)
    {
        if (Runner.TryGetPlayerObject(player, out var spaceshipNetworkObject))
        {
            Runner.Despawn(spaceshipNetworkObject);
        }
        // Reset Player Object
        Runner.SetPlayerObject(player, null);
    }
}
