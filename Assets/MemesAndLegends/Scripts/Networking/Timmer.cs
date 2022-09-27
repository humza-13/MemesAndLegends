using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timmer : MonoBehaviour, IPunObservable
{
    public PhotonView pv;
    float seconds = 95;
    public TMPro.TMP_Text Timetext;


    private void Update()
    {
        seconds -= Time.deltaTime;
        UpdateTime(seconds);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(seconds);
        }
        if (stream.IsReading)
        {
            UpdateTime((float)stream.ReceiveNext());
        }
    }
    public void OnEndTurn()
    {
        if (!BoardManager.Instance.EndTurn.GetComponent<PhotonView>().IsMine)
            return;
        BoardManager.Instance.ResetPlayerMovesData();
        BoardManager.Instance.ResetActiveSyncers();
        if (PhotonNetwork.PlayerListOthers.Length > 0)
            BoardManager.Instance.EndTurn.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerListOthers[0]);

        seconds = 95;
        UpdateTime(seconds);
    }
    public void UpdateTime(float time)
    {
        if (seconds <= 0)
        {
            OnEndTurn();
            seconds = 95;
            UpdateTime(seconds);
        }
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        Timetext.text = "Turn Time: "+ String.Format("{0:00}:{1:00}", t.Minutes,t.Seconds);
    }
}
