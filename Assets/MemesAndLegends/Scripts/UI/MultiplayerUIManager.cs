using Fusion.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

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


    #region Create Room
    public void onLobbyValueChanged(string name)
    {
        ServerInfo.LobbyName = name;
        createRoomConfirm.interactable = !string.IsNullOrEmpty(name);
    }
    
    public void onJoinRoomValueChanged(string name)
    {
        joinRoomConfirm.interactable = !string.IsNullOrEmpty(name);
    }
    public void ValidateLobby()
    {
        _lobbyIsValid = string.IsNullOrEmpty(ServerInfo.LobbyName) == false;
    }

    public void TryCreateLobby(NetworkManager launcher)
    {
        ValidateLobby();
        if (_lobbyIsValid)
        {
            launcher.JoinOrCreateLobby();
            _lobbyIsValid = false;
        }
    }
    #endregion
  
    #region UI
    public void OpenMultiplayerMenu()
    {
        ActionMenu.SetActive(false);
        MultiplayerMenu.SetActive(true);
    }

    public void CloseMultiplayerMenu()
    {
        MultiplayerMenu.SetActive(false);
        ActionMenu.SetActive(true);
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
        if (NetworkManager.ConnectionStatus == ConnectionStatus.Connected)
            Lobby.SetActive(true);
    }

  
    public void CloseLobby()
    {
        Lobby.SetActive(false);
    }


    #endregion

}
