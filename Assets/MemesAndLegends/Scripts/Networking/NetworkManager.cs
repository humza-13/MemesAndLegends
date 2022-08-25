using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ConnectionStatus
{
	Disconnected,
	Connecting,
	Failed,
	Connected
}


[RequireComponent(typeof(LevelManager))]
public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
	[SerializeField] private GameManager _gameManagerPrefab;
	[SerializeField] private RoomPlayer _roomPlayerPrefab;
	[SerializeField] private GameObject _loadingUI;

    public static CharacterSpawner Spawner { get; private set; }

    public static ConnectionStatus ConnectionStatus = ConnectionStatus.Disconnected;
	public static ShutdownReason FailedStatus = ShutdownReason.Ok;
	public static bool shutDown = false;

	private NetworkRunner _runner;
	private FusionObjectPoolRoot _pool;
	private GameMode _gameMode;
	private LevelManager _levelManager;

    [Header("Error Fields")]
    [SerializeField] private GameObject _errorPopup;
    [SerializeField] private TextMeshProUGUI _errorTxt;

	[Header("Multiplayer UI Manager")]
    [SerializeField] private MultiplayerUIManager _multiplayerUIManager;

    private void Start()
	{
		Application.runInBackground = true;
		Application.targetFrameRate = Screen.currentResolution.refreshRate;
		QualitySettings.vSyncCount = 1;

		_levelManager = GetComponent<LevelManager>();
	
	}

	public void SetCreateLobby() => _gameMode = GameMode.Host;
	public void SetJoinLobby() => _gameMode = GameMode.Client;

	public void JoinOrCreateLobby()
	{
		SetConnectionStatus(ConnectionStatus.Connecting);

		if (_runner != null)
			LeaveSession();

		GameObject go = new GameObject("Session");
		DontDestroyOnLoad(go);

		_runner = go.AddComponent<NetworkRunner>();
		_runner.ProvideInput = _gameMode != GameMode.Server;
		_runner.AddCallbacks(this);

		_pool = go.AddComponent<FusionObjectPoolRoot>();

		Debug.Log($"Created gameobject {go.name} - starting game");
		_runner.StartGame(new StartGameArgs
		{
			GameMode = _gameMode,
			SessionName = _gameMode == GameMode.Host ? ServerInfo.LobbyName : ClientInfo.LobbyName,
			ObjectPool = _pool,
			SceneManager = _levelManager,
			PlayerCount = ServerInfo.MaxUsers,
			DisableClientSessionCreation = true
		});
		_loadingUI.SetActive(true);
        _multiplayerUIManager.OpenLobby();
    }

	private void SetConnectionStatus(ConnectionStatus status)
	{
		Debug.Log($"Setting connection status to {status}");

		ConnectionStatus = status;

		if (!Application.isPlaying)
			return;

		if (status == ConnectionStatus.Disconnected || status == ConnectionStatus.Failed)
		{
			_loadingUI?.SetActive(false);
		}
	}

	public void LeaveSession()
	{
		if (_runner != null)
			_runner.Shutdown();
		else
			SetConnectionStatus(ConnectionStatus.Disconnected);
	}

	public void OnConnectedToServer(NetworkRunner runner)
	{
		Debug.Log("Connected to server");
		SetConnectionStatus(ConnectionStatus.Connected);
	}
	public void OnDisconnectedFromServer(NetworkRunner runner)
	{
		Debug.Log("Disconnected from server");
		LeaveSession();
		SetConnectionStatus(ConnectionStatus.Disconnected);
	}
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
		if (runner.CurrentScene > 0)
		{
			Debug.LogWarning($"Refused connection requested by {request.RemoteAddress}");
			request.Refuse();
		}
		else
			request.Accept();
	}
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
		Debug.Log($"Connect failed {reason}");
		LeaveSession();
		SetConnectionStatus(ConnectionStatus.Failed);
		(string status, string message) = ConnectFailedReasonToHuman(reason);
		_errorTxt.text = "";
		_errorTxt.text = status + "\n" + message;
		_errorPopup.SetActive(true);
	}
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
	{
		_loadingUI?.SetActive(false);
		Debug.Log($"Player {player} Joined!");
		if (runner.IsServer)
		{
			if (_gameMode == GameMode.Host)
				runner.Spawn(_gameManagerPrefab, Vector3.zero, Quaternion.identity);
			var roomPlayer = runner.Spawn(_roomPlayerPrefab, Vector3.zero, Quaternion.identity, player);
			roomPlayer.GameState = RoomPlayer.EGameState.GameReady;
			
		}
		SetConnectionStatus(ConnectionStatus.Connected);

    }

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		Debug.Log($"{player.PlayerId} disconnected.");

		RoomPlayer.RemovePlayer(runner, player);

		SetConnectionStatus(ConnectionStatus);
	}
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
		Debug.Log($"OnShutdown {shutdownReason}");
		FailedStatus = shutdownReason;
		shutDown = true;
		SetConnectionStatus(ConnectionStatus.Disconnected);

		(string status, string message) = ShutdownReasonToHuman(shutdownReason);
        _errorTxt.text = "";
        _errorTxt.text = status + "\n" + message;
        _errorPopup.SetActive(true);

        RoomPlayer.Players.Clear();

		if (_runner)
			Destroy(_runner.gameObject);

		// Reset the object pools
		_pool.ClearPools();
		_pool = null;

		_runner = null;
	}
	public void OnInput(NetworkRunner runner, NetworkInput input) { }
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
	public void OnSceneLoadDone(NetworkRunner runner) { }
	public void OnSceneLoadStart(NetworkRunner runner) { }

	private static (string, string) ShutdownReasonToHuman(ShutdownReason reason)
	{
		switch (reason)
		{
			case ShutdownReason.Ok:
				return (null, null);
			case ShutdownReason.Error:
				return ("Error", "Shutdown was caused by some internal error");
			case ShutdownReason.IncompatibleConfiguration:
				return ("Incompatible Config", "Mismatching type between client Server Mode and Shared Mode");
			case ShutdownReason.ServerInRoom:
				return ("Room name in use", "There's a room with that name! Please try a different name or wait a while.");
			case ShutdownReason.DisconnectedByPluginLogic:
				return ("Disconnected By Plugin Logic", "You were kicked, the room may have been closed");
			case ShutdownReason.GameClosed:
				return ("Game Closed", "The session cannot be joined, the game is closed");
			case ShutdownReason.GameNotFound:
				return ("Game Not Found", "This room does not exist");
			case ShutdownReason.MaxCcuReached:
				return ("Max Players", "The Max CCU has been reached, please try again later");
			case ShutdownReason.InvalidRegion:
				return ("Invalid Region", "The currently selected region is invalid");
			case ShutdownReason.GameIdAlreadyExists:
				return ("ID already exists", "A room with this name has already been created");
			case ShutdownReason.GameIsFull:
				return ("Game is full", "This lobby is full!");
			case ShutdownReason.InvalidAuthentication:
				return ("Invalid Authentication", "The Authentication values are invalid");
			case ShutdownReason.CustomAuthenticationFailed:
				return ("Authentication Failed", "Custom authentication has failed");
			case ShutdownReason.AuthenticationTicketExpired:
				return ("Authentication Expired", "The authentication ticket has expired");
			case ShutdownReason.PhotonCloudTimeout:
				return ("Cloud Timeout", "Connection with the Photon Cloud has timed out");
			default:
				Debug.LogWarning($"Unknown ShutdownReason {reason}");
				return ("Unknown Shutdown Reason", $"{(int)reason}");
		}
	}

	private static (string, string) ConnectFailedReasonToHuman(NetConnectFailedReason reason)
	{
		switch (reason)
		{
			case NetConnectFailedReason.Timeout:
				return ("Timed Out", "");
			case NetConnectFailedReason.ServerRefused:
				return ("Connection Refused", "The lobby may be currently in-game");
			case NetConnectFailedReason.ServerFull:
				return ("Server Full", "");
			default:
				Debug.LogWarning($"Unknown NetConnectFailedReason {reason}");
				return ("Unknown Connection Failure", $"{(int)reason}");
		}
	}
    public void CloseConnectionError()
    {
        _errorPopup.SetActive(false);
    }
    public static void SetSpawner(CharacterSpawner spawner)
    {
        Spawner = spawner;
    }
}