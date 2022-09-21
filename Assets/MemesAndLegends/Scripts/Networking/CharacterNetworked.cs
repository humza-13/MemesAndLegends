using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterNetworked : MonoBehaviour
{
    private PhotonView pv;
    private List<Transform> self_spawn;
    private List<Transform> enemy_spawn;
    public PlayerController player_controller;
    public Charactercontroller character_controller;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        var go = GameObject.FindGameObjectWithTag("CharacterSpawn").gameObject.GetComponent<SpawnPoints>();
        self_spawn = go.SelfSpawnPoints;
        enemy_spawn = go.EnemySpawnPoints;

       
    }
    [PunRPC]
    public void InitCharacter(int ID, int SpawnIndex, int index)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
     
        CharacterObject characterProps = GameObject.FindGameObjectsWithTag("CharacterResource")[0].GetComponent<CharacterResource>().FindCharacterWithID(ID).ShallowCopy();
        this.GetComponentInChildren<Image>().sprite = characterProps.Character_Sprite;

        if (pv.IsMine)
        {
            this.GetComponent<Transform>().SetParent(self_spawn[SpawnIndex]);
            self_spawn[SpawnIndex].gameObject.name = characterProps.Name;
            
            foreach (GameObject p in players)
                if (p.GetComponent<PlayerController>().pv.IsMine)
                    player_controller = p.GetComponent<PlayerController>();
        }
        else
        {
            this.GetComponent<Transform>().SetParent(enemy_spawn[SpawnIndex]);
            enemy_spawn[SpawnIndex].gameObject.name = characterProps.Name;
            foreach (GameObject p in players)
                if (!p.GetComponent<PlayerController>().pv.IsMine)
                    player_controller = p.GetComponent<PlayerController>();

        }
        this.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        character_controller = player_controller.characters[index];
    }
}
