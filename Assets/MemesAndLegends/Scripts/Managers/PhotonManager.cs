using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    public bool isConnected;
    public bool isNetworkError;
    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        
        else
            Instance = this;

        // reset connection status
        isConnected = false;
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }
    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        isConnected = true;
        isNetworkError = false;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        isNetworkError = true;
    }

    public override void OnJoinedLobby()
    {
      
    }

    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
       
    }

    public override void OnJoinedRoom()
    {
      
    }

    public override void OnLeftRoom()
    {
       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
       
    }
    #endregion

    #region Custome Helper Methods
    public void LoadSceneAsync(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }

    public bool IsNetworkError()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return true;
        if(isNetworkError)
            return true;

        return false;
    }

    public void Reconnect()
    {
        isNetworkError = false;
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

}
