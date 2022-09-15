using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("UI Components")]
    public GameObject loadingPanel;
    public MultiplayerUIManager uiManger;
    public GameObject errorUI;
    public GameObject LobbyList;
    public TMPro.TMP_Text errorUIText;


    [Header("Create Room Panel")]
    public TMPro.TMP_InputField RoomNameInputField;
    
    [Header("Join Room Panel")]
    public GameObject JoinRoomPanel;
    public TMPro.TMP_InputField JoinNameInputField;

    [Header("Lobby Panel")]
    public Button StartGameButton;
    public GameObject PlayerEntryPrefab;


    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY

    public void Awake()
    {
        SetActivePanel(loadingPanel.name);
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        PhotonNetwork.LocalPlayer.NickName = ClientInfo.Username;
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        loadingPanel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorUIText.text = message;
        SetActivePanel(errorUI.name);
        loadingPanel.SetActive(false);
        uiManger.OpenMultiplayerMenu();
        uiManger.CloseCreateRoom();

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
          
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
           
    }

    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();
        loadingPanel.SetActive(false);
        uiManger.OpenLobby();

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerEntryPrefab);
            entry.transform.SetParent(LobbyList.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerRoomEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                 entry.GetComponent<PlayerRoomEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnLeftRoom()
    {
        uiManger.OpenMultiplayerMenu();

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerEntryPrefab);
        entry.transform.SetParent(LobbyList.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerRoomEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                entry.GetComponent<PlayerRoomEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion

    #region UI CALLBACKS

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        uiManger.OpenMultiplayerMenu();
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = ServerInfo.LobbyName;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers;
        maxPlayers = (byte)ServerInfo.MaxUsers;

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };
        PhotonNetwork.CreateRoom(roomName, options, null);
        SetActivePanel(loadingPanel.name);
    }
    public void OnJoinRoomButtonClicked()
    {
        string roomName = ServerInfo.LobbyName;
        PhotonNetwork.JoinRoom(roomName);
        SetActivePanel(loadingPanel.name);
    }


    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

   
  
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //PhotonNetwork.LoadLevel("DemoAsteroids-GameScene");
    }

    #endregion

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    private void SetActivePanel(string activePanel)
    {
        loadingPanel.SetActive(activePanel.Equals(loadingPanel.name));
        JoinRoomPanel.SetActive(activePanel.Equals(JoinRoomPanel.name));
        errorUI.SetActive(activePanel.Equals(errorUI.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            //GameObject entry = Instantiate(RoomListEntryPrefab);
            //entry.transform.SetParent(RoomListContent.transform);
            //entry.transform.localScale = Vector3.one;
            //entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            //  roomListEntries.Add(info.Name, entry);
        }
    }
}
