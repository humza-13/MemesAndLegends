using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }
    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conected");
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
}
