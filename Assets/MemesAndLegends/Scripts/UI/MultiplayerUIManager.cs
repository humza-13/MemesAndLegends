using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUIManager : MonoBehaviour
{
    public GameObject MultiplayerMenu;
    public GameObject ActionMenu;

    [Header("Room Panels")]
    public GameObject CreateRoom;
    public GameObject JoinRoom;

    [Header("Create Room Fields")]
    public Button createRoomConfirm;
    public TMPro.TMP_InputField lobbyName;

    public void onLobbyValueChanged(string name)
    {
        ServerInfo.LobbyName = name;
        createRoomConfirm.interactable = !string.IsNullOrEmpty(name);
    }

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
    #endregion


}
