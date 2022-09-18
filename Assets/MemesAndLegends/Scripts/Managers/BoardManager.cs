using Mono.Cecil.Cil;
using Photon.Pun;
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
       // if(pv.IsMine)
        //{

            var temp = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(PlayerContent.position.x, PlayerContent.position.y), Quaternion.identity);
            temp.gameObject.transform.SetParent(PlayerContent);
            temp.GetComponent<PlayerController>().Init();
        //}
    }

}
