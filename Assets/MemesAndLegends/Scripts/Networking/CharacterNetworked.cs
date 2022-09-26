using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

public class CharacterNetworked : MonoBehaviour
{
    private PhotonView pv;
    private List<Transform> self_spawn;
    private List<Transform> enemy_spawn;
    public PlayerController player_controller;
    public Charactercontroller character_controller;
    public const byte MoveUnitsToTargetPositionEventCode = 1;
    public GameObject SPAWN;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
       
        byte eventCode = photonEvent.Code;
        if (eventCode == MoveUnitsToTargetPositionEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 targetPosition = (Vector2)data[0];
            BlockID blockID = (BlockID)data[1];
            var _pv = (int)data[2];


            if (this.pv.ViewID == _pv)
            {
                character_controller.blockID = blockID;

                SPAWN.GetComponent<RectTransform>().DOAnchorPos(new Vector3(targetPosition.x + 150, targetPosition.y, 1), 0.3f)
                    .SetEase(Ease.Linear);
            }
        }
    }
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        var go = GameObject.FindGameObjectWithTag("CharacterSpawn").gameObject.GetComponent<SpawnPoints>();

        if((pv.Owner.IsMasterClient && pv.IsMine) || (!pv.Owner.IsMasterClient && !pv.IsMine))
        {
            self_spawn = go.SelfSpawnPoints;
            enemy_spawn = go.EnemySpawnPoints;
        }
        else if((pv.Owner.IsMasterClient && !pv.IsMine) || (!pv.Owner.IsMasterClient && pv.IsMine))
        {
            self_spawn = go.EnemySpawnPoints;
            enemy_spawn = go.SelfSpawnPoints;
        }
    }
    public void OnActionTaken()
    {
        if (!pv.IsMine)
            return;

        var _current = BoardManager.Instance.FindLocation(character_controller.blockID);
        BoardManager.Instance.CalculateAttack(_current,this ,PhotonNetwork.LocalPlayer, BoardManager.ActionType.Move);
      
        
    }
    public void Move(BlockID ID)
    {
        if (!pv.IsMine)
            return;

        byte evCode = 1;
        var _block = BoardManager.Instance.GetBlockWithID(ID);

        Vector2 vector2 = _block.GetComponent<RectTransform>().anchoredPosition;
        object[] content = new object[] { vector2, (int)ID, pv.ViewID };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    [PunRPC]
    public void InitCharacter(int ID, int SpawnIndex, int index)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        BlockID BLOCK_ID;
     
        CharacterObject characterProps = GameObject.FindGameObjectsWithTag("CharacterResource")[0].GetComponent<CharacterResource>().FindCharacterWithID(ID).ShallowCopy();
        this.GetComponent<Image>().sprite = characterProps.Character_Sprite;

        if (pv.IsMine)
        {
            this.GetComponent<Transform>().SetParent(self_spawn[SpawnIndex]);
            SPAWN = self_spawn[SpawnIndex].gameObject;

            self_spawn[SpawnIndex].gameObject.name = characterProps.Name;
            BLOCK_ID = self_spawn[SpawnIndex].gameObject.GetComponent<PlayerLocation>().PlayerCurrentBlock;

            foreach (GameObject p in players)
                if (p.GetComponent<PlayerController>().pv.IsMine)
                    player_controller = p.GetComponent<PlayerController>();
        }
        else
        {
            this.GetComponent<Transform>().SetParent(enemy_spawn[SpawnIndex]);
            enemy_spawn[SpawnIndex].gameObject.name = characterProps.Name;
            SPAWN = enemy_spawn[SpawnIndex].gameObject;


            BLOCK_ID = enemy_spawn[SpawnIndex].gameObject.GetComponent<PlayerLocation>().PlayerCurrentBlock;

            foreach (GameObject p in players)
                if (!p.GetComponent<PlayerController>().pv.IsMine)
                    player_controller = p.GetComponent<PlayerController>();

        }
        this.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        character_controller = player_controller.characters[index];
        character_controller.blockID = BLOCK_ID;
    }
}
