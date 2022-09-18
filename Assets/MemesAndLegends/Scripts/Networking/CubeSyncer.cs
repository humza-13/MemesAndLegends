using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSyncer : MonoBehaviour, IPunObservable
{
    public GameObject Red_Glow;
    public GameObject Golden_Glow;

    private bool glowGolden = false;
    private bool glowRed = false;

    public PhotonView pv;
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(glowGolden);
            stream.SendNext(glowRed);
        }
        if(stream.IsReading)
        {
            SetGolden((bool)stream.ReceiveNext());
            SetRed((bool)stream.ReceiveNext());
        }
    }

    public void SetGolden(bool status, Player player = null)
    {
        if (player != null && !pv.IsMine)
        {
            this.TransferOwnerShip(player);
        }
        glowGolden = status;
            Golden_Glow.SetActive(glowGolden);
        
    }
    public void SetRed(bool status, Player player = null)
    {
        if(player != null && !pv.IsMine)
        {
            this.TransferOwnerShip(player);
        }
            glowRed = status;
            Red_Glow.SetActive(glowRed);   
    }
   
    public void TransferOwnerShip(Player player)
    {
        pv.TransferOwnership(player);
    }
}
