using Mono.Cecil.Cil;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform PlayerContent;
    public PhotonView pv;
    public GameObject LoadingUI;

    [Header("Board Rows")]
    public List<GameObject> R1;
    public List<GameObject> R2;
    public List<GameObject> R3;
    public List<GameObject> R4;
    public List<GameObject> R5;
    public List<GameObject> R6;
    public List<GameObject> R7;
    public List<GameObject> R8;

    [SerializeField]public List<List<GameObject>> Board;
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

        PopulateBoard();
    
    }
    private void PopulateBoard()
    {
        Board.Add(R1);
        Board.Add(R2);
        Board.Add(R3);
        Board.Add(R4);
        Board.Add(R5);
        Board.Add(R6);
        Board.Add(R7);
        Board.Add(R8);
    }

}
