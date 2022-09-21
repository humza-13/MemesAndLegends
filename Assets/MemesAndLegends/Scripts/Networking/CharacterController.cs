using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Charactercontroller : MonoBehaviour, IPunObservable
{
    [Header("Character UI Elements")]
    public TMP_Text Name;
    public Image Icon;
    public TMP_Text Health;
    public TMP_Text Defence;
    public TMP_Text Attack;
    public Slider HealthSlider;
    public Slider DefenceSlider;
    public Slider AttackSlider;

    public int SpawnIndex;
    private Transform SpawnPoint;
    public CharacterObject characterProps;
    public PhotonView pv;
    public CharacterResource resource;
    private int ID;
   

    private void Awake()
    {
        if(resource == null)
        {
            var _res = GameObject.FindGameObjectsWithTag("CharacterResource")[0].GetComponent<CharacterResource>();
            resource = _res;
        }
        pv = GetComponent<PhotonView>();
        SpawnPoint = GameObject.FindGameObjectWithTag("CharacterSpawn").GetComponent<Transform>();
      

    }
    public void Init(int ID, int index)
    {
        if(pv == null)
            pv = GetComponent<PhotonView>();
        this.ID = ID;
        characterProps = resource.FindCharacterWithID(ID).ShallowCopy();
     
        Name.text = characterProps.Name;
        Icon.sprite = characterProps.Character_Sprite;
 
        UpdateHealth(characterProps.Health);
        SetAttack(characterProps.Attack_Power);
        SetDefence(characterProps.Defence);
        SpawnBody(index);
    }
    public PhotonView _pv;
    private void SpawnBody(int index)
    {
        if (!pv.IsMine)
            return;
        GameObject character;
        character = PhotonNetwork.Instantiate("Character", SpawnPoint.position, Quaternion.identity,0);
        _pv = character.GetComponent<PhotonView>();
        _pv.RPC("InitCharacter", RpcTarget.All, ID, SpawnIndex, index);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (characterProps != null)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(characterProps.Health);
                stream.SendNext(characterProps.Attack_Power);
                stream.SendNext(characterProps.Defence);
            }
            if (stream.IsReading)
            {
                UpdateHealth((int)stream.ReceiveNext());
                SetAttack((int)stream.ReceiveNext());
                SetDefence((int)stream.ReceiveNext());
            }
        }
    }

    public void UpdateHealth(int health)
    {
//if(pv.IsMine)
      //  {
            // add actual health
            Health.text = health.ToString();
            HealthSlider.value = health;
     //   }
    }
    public void SetAttack(int attack)
    {
       // if (pv.IsMine)
      //  {
            // add actual attack
            Attack.text = attack.ToString();
            AttackSlider.value = attack;
     //   }
    }
    public void SetDefence(int defence)
    {
      //  if (pv.IsMine)
      //  {
            // add actual defence
            Defence.text = defence.ToString();
            DefenceSlider.value = defence;
      //  }
    }
}
