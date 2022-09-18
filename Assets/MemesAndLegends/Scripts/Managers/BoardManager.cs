using Mono.Cecil.Cil;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerContent;
    public PhotonView pv;
    public void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.Owner.IsMasterClient)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                var temp = PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerContent.position, Quaternion.identity);
                temp.gameObject.transform.SetParent(PlayerContent);
                temp.GetComponent<PlayerController>().Init(p);
            }
        }
    }

}
