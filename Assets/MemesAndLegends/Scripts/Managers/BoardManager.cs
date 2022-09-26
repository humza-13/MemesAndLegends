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

    [SerializeField] public List<List<GameObject>> Board;
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
        foreach (var row in Board)
        {
            foreach (var col in row)
            {
                if (col.GetComponent<CubeSyncer>().ID == ID)
                    return col;
            }
        }
        return null;
    }
    public Vector2 FindLocation(BlockID ID)
    {
        for (int row = 0; row < Board.Count; row++)
        {
            for (int col = 0; col < Board[row].Count; col++)
            {
                if (Board[row][col].GetComponent<CubeSyncer>().ID == ID)
                    return new Vector2(row, col);
            }
        }
        return Vector2.zero;
    }
    private void ActivateSyncer(CubeSyncer syncer, CharacterNetworked character, Player p, ActionType action)
    {
        ActiveSyncers.Add(syncer);

        switch (action) {
            
            case ActionType.Move:
                syncer.action = () => { character.Move(syncer.ID); ResetActiveSyncers(); };
                syncer.SetGolden(true, p);
                break;
            case ActionType.Attack:
                syncer.action = () => { Debug.Log("ATTACKING"); ResetActiveSyncers(); };
                syncer.SetRed(true, p);
                break;
        }
}

    public void CalculateMove(Vector2 location, CharacterNetworked character, Player p, ActionType action)
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
                            ActivateSyncer(syncer, character, p, action);
                    }
                    //backward
                    if (back >= 0)
                    {
                        var syncer = Board[_row - i][_col].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }
                    //left
                    if (left >= 0)
                    {
                        var syncer = Board[_row][_col - i].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }
                    //right
                    if (right < 8)
                    {
                        var syncer = Board[_row][_col + i].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }
                    break;

                case CharacterObject.MovementType.Omni_Directional:
                    Vector2 fr = new Vector2((_row + i), (_col + DEPTH));
                    Vector2 fl = new Vector2((_row + i), (_col - DEPTH));
                    Vector2 dr = new Vector2((_row - i), (_col + DEPTH));
                    Vector2 dl = new Vector2((_row - i), (_col - DEPTH));
                    int l = _col - i;
                    int r = _col + i;

              
                    //left
                    if (l >= 0)
                    {
                        var syncer = Board[_row][l].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }
                    //right
                    if (r < 8)
                    {
                        var syncer = Board[_row][r].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    // forward right
                    if (fr.x < 8 && fr.y < 8)
                    {
                        for (int j = _col; j <= fr.y; j++)
                        {
                            var syncer = Board[(int)fr.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerMoveBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // forward left
                    if (fl.x < 8 && fl.y >= 0)
                    {
                        for (int j = _col - 1; j >= fl.y; j--)
                        {
                            var syncer = Board[(int)fl.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerMoveBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // back right
                    if (dr.x >= 0 && dr.y < 8)
                    {
                        for (int j = _col; j <= dr.y; j++)
                        {
                            var syncer = Board[(int)dr.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerMoveBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // down left
                    if (dl.x >= 0 && dl.y >= 0)
                    {
                        for (int j = _col - 1; j >= dl.y; j--)
                        {
                            var syncer = Board[(int)dl.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerMoveBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    break;

                case CharacterObject.MovementType.Diagnole:
                    Vector2 ur = new Vector2((_row + i), (_col + i));
                    Vector2 ul = new Vector2((_row + i), (_col - i));
                    Vector2 br = new Vector2((_row - i), (_col + i));
                    Vector2 bl = new Vector2((_row - i), (_col - i));

                    // Upper right
                    if (ur.x < 8 && ur.y < 8)
                    {
                        var syncer = Board[(int)ur.x][(int)ur.y].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    // Upper left
                    if (ul.x < 8 && ul.y >= 0)
                    {
                        var syncer = Board[(int)ul.x][(int)ul.y].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    // Down right
                    if (br.x >= 0 && br.y < 8)
                    {
                        var syncer = Board[(int)br.x][(int)br.y].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    // Down left
                    if (bl.x >= 0 && bl.y >= 0)
                    {
                        var syncer = Board[(int)bl.x][(int)bl.y].GetComponent<CubeSyncer>();
                        if (CheckPlayerMoveBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    break;
            }
            
        }


    }
    public void CalculateAttack(Vector2 location, CharacterNetworked character, Player p, ActionType action)
    {
        int _row = (int)location.x;
        int _col = (int)location.y;
        int DEPTH = character.character_controller.characterProps.Attack_Range;
        ResetActiveSyncers();

        for (int i = 1; i <= DEPTH; i++)
        {
            switch (character.character_controller.characterProps.Attack)
            {
                case CharacterObject.AttackType.Omni_Directional:
                    Vector2 fr = new Vector2((_row + i), (_col + DEPTH));
                    Vector2 fl = new Vector2((_row + i), (_col - DEPTH));
                    Vector2 dr = new Vector2((_row - i), (_col + DEPTH));
                    Vector2 dl = new Vector2((_row - i), (_col - DEPTH));
                    int l = _col - i;
                    int r = _col + i;
                    //left
                    if (l >= 0)
                    {
                        var syncer = Board[_row][l].GetComponent<CubeSyncer>();
                        if (CheckPlayerAttackBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }
                    //right
                    if (r < 8)
                    {
                        var syncer = Board[_row][r].GetComponent<CubeSyncer>();
                        if (CheckPlayerAttackBlockValid(syncer.ID))
                            ActivateSyncer(syncer, character, p, action);
                    }

                    // forward right
                    if (fr.x < 8 && fr.y < 8)
                    {
                        for (int j = _col; j <= fr.y; j++)
                        {
                            var syncer = Board[(int)fr.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerAttackBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // forward left
                    if (fl.x < 8 && fl.y >= 0)
                    {
                        for (int j = _col - 1; j >= fl.y; j--)
                        {
                            var syncer = Board[(int)fl.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerAttackBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // back right
                    if (dr.x >= 0 && dr.y < 8)
                    {
                        for (int j = _col; j <= dr.y; j++)
                        {
                            var syncer = Board[(int)dr.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerAttackBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    // down left
                    if (dl.x >= 0 && dl.y >= 0)
                    {
                        for (int j = _col - 1; j >= dl.y; j--)
                        {
                            var syncer = Board[(int)dl.x][j].GetComponent<CubeSyncer>();
                            if (CheckPlayerAttackBlockValid(syncer.ID))
                                ActivateSyncer(syncer, character, p, action);
                        }
                    }
                    break;
                case CharacterObject.AttackType.None:
                    return;

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

    public enum ActionType
    {
        Move = 1,
        Attack = 2,
        Special = 3

    }

    private bool CheckPlayerMoveBlockValid(BlockID ID)
    {
        foreach(var p in players)
            foreach(var c in p.characters)
                if(c.blockID == ID)
                    return false;
        return true;
    }
    private bool CheckPlayerAttackBlockValid(BlockID ID)
    {
        foreach (var p in players)
            foreach (var c in p.characters)
                if (c.blockID == ID && !c.pv.IsMine)
                    return true;
        return false;
    }
}
