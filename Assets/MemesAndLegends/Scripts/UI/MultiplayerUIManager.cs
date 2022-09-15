using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MultiplayerUIManager : MonoBehaviour
{
    public GameObject MultiplayerMenu;
    public GameObject ActionMenu;

    [Header("Room Panels")]
    public GameObject CreateRoom;
    public GameObject JoinRoom;
    public GameObject Lobby;

    [Header("Create Room Fields")]
    public Button createRoomConfirm;
    public TMP_InputField lobbyName;
    private bool _lobbyIsValid;

    [Header("Join Room Fields")]
    public Button joinRoomConfirm;

    [Header("Network Manager")]
    public GameObject NetworkManager;

    #region Create Room
    public void onLobbyValueChanged(string name)
    {
        ServerInfo.LobbyName = name;
        createRoomConfirm.interactable = !string.IsNullOrEmpty(name);
    }
    
    public void onJoinRoomValueChanged(string name)
    {
        ServerInfo.LobbyName = name;
        joinRoomConfirm.interactable = !string.IsNullOrEmpty(name);
    }
    public void ValidateLobby()
    {
        _lobbyIsValid = string.IsNullOrEmpty(ServerInfo.LobbyName) == false;
    }

    public void TryCreateLobby()
    {
        ValidateLobby();
        if (_lobbyIsValid)
        {
             NetworkManager.GetComponent<NetworkManager>().OnCreateRoomButtonClicked();
            _lobbyIsValid = false;
        }
    }
    public void TryJoinLobby()
    {
        ValidateLobby();
        if (_lobbyIsValid)
        {
            NetworkManager.GetComponent<NetworkManager>().OnJoinRoomButtonClicked();
            _lobbyIsValid = false;
        }
    }
    #endregion

    #region UI
    public void OpenMultiplayerMenu()
    {
        ActionMenu.SetActive(false);
        MultiplayerMenu.SetActive(true);
        NetworkManager.SetActive(true);

    }

    public void CloseMultiplayerMenu()
    {
        MultiplayerMenu.SetActive(false);
        ActionMenu.SetActive(true);
        NetworkManager?.SetActive(false);
    }

    public void OpenJoinRoom()
    {
        JoinRoom.SetActive(true);
    }
    public void CloseJoinRoom()
    {
        JoinRoom.SetActive(false);
    }

    public void OpenCreateRoom()
    {
        lobbyName.text = ServerInfo.LobbyName;
        CreateRoom.SetActive(true);
    }
    public void CloseCreateRoom()
    {
        CreateRoom.SetActive(false);
    }

    public void OpenLobby()
    {
       Lobby.SetActive(true);
    }

  
    public void CloseLobby()
    {
        Lobby.SetActive(false);
    }


    #endregion

}
