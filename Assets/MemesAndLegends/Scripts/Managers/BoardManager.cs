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
    public GameObject LoadingUI;

    private void Awake()
    {
        LoadingUI.SetActive(true);
    }
    IEnumerator Start()
    {
        pv = GetComponent<PhotonView>();
        PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerContent.position, Quaternion.identity);
        
        yield return new WaitForSeconds(3);
        LoadingUI.SetActive(false);
    
    }

}
