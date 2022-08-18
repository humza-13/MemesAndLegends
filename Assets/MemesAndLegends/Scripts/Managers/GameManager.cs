using System;
using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
	public static event Action<GameManager> OnLobbyDetailsUpdated;

	public static GameManager Instance { get; private set; }


	[Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public NetworkString<_32> LobbyName { get; set; }
	[Networked(OnChanged = nameof(OnLobbyDetailsChangedCallback))] public int MaxUsers { get; set; }

	private static void OnLobbyDetailsChangedCallback(Changed<GameManager> changed)
	{
		OnLobbyDetailsUpdated?.Invoke(changed.Behaviour);
	}

	private void Awake()
	{
		if (Instance)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public override void Spawned()
	{
		base.Spawned();

		if (Object.HasStateAuthority)
		{
			//LobbyName = ServerInfo.LobbyName;
			//TrackId = ServerInfo.TrackId;
			//GameTypeId = ServerInfo.GameMode;
			//MaxUsers = ServerInfo.MaxUsers;
		}
	}

}