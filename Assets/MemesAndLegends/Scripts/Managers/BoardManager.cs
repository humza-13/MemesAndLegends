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
    public List<PlayerController> players;
    public List<CubeSyncer> ActiveSyncers;

    [SerializeField]public List<List<GameObject>> Board;
    private static BoardManager instance;
    public static BoardManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
        LoadingUI.SetActive(true);
        players = new List<PlayerController>();
        ActiveSyncers = new List<CubeSyncer>();
    }
    IEnumerator Start()
    {
        Board = new List<List<GameObject>>();
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

    public GameObject GetBlockWithID(BlockID ID)
    { 
        foreach(var row in Board)
        {
            foreach (var col in row)
            {
                if(col.GetComponent<CubeSyncer>().ID == ID)
                    return col;
            }
        }
        return null;
    }
    public Vector2 FindLocation(BlockID ID)
    {
        for (int row = 0; row < Board.Count; row++)
        {
            for(int col = 0; col < Board[row].Count; col++)
            {
                if (Board[row][col].GetComponent<CubeSyncer>().ID == ID)
                    return new Vector2(row,col);
            }
        }
        return Vector2.zero;
    }

    public void CalculateMove(Vector2 location, CharacterNetworked character, Player p)
    {
        int _row = (int)location.x;
        int _col = (int)location.y;
        int DEPTH = character.character_controller.characterProps.Movement_Range; 
        ResetActiveSyncers();
        
        for (int i = 1; i <= DEPTH; i++)
        {
            switch(character.character_controller.characterProps.Movement)
                {
                    case CharacterObject.MovementType.Plus:

                    int forward = _row + i;
                    int back = _row - i;
                    int left = _col - i;
                    int right = _col + i;

                    //forward
                    if (forward < 8)
                    {
                        var syncer = Board[_row + i][_col].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                        {
                            ActiveSyncers.Add(syncer);
                            syncer.action = () => { character.Move(syncer.ID); ResetActiveSyncers(); };
                            syncer.SetGolden(true, p);
                        }
                    }
                    //backward
                    if (back >= 0)
                    {
                        var syncer = Board[_row - i][_col].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                        {
                            ActiveSyncers.Add(syncer);
                            syncer.action = () => { character.Move(syncer.ID); ResetActiveSyncers(); };
                            syncer.SetGolden(true, p);
                        }
                    }
                    //left
                    if (left >= 0)
                    {
                        var syncer = Board[_row][_col - i].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                        {
                            ActiveSyncers.Add(syncer);
                            syncer.action = () => { character.Move(syncer.ID); ResetActiveSyncers(); };
                            syncer.SetGolden(true, p);
                        }
                    }
                    //right
                    if (right < 8)
                    {
                        var syncer = Board[_row][_col + i].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                        {
                            ActiveSyncers.Add(syncer);
                            syncer.action = () => { character.Move(syncer.ID); ResetActiveSyncers(); };
                            syncer.SetGolden(true, p);
                        }
                    }
                    break;

            }
            
        }


    }

    private void ResetActiveSyncers()
    {
        for (int x = ActiveSyncers.Count - 1; x > -1; x--)
        {
            ActiveSyncers[x].SetGolden(false);
            ActiveSyncers[x].SetRed(false);
            ActiveSyncers.RemoveAt(x);
        }
    }

    private bool CheckPlayerMoveBlockValid(BlockID ID)
    {
        foreach(var p in players)
            foreach(var c in p.characters)
                if(c.blockID == ID)
                    return false;
        return true;
    }
}
