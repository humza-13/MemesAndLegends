using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour, IPunObservable
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

    public CharacterObject characterProps;
    public PhotonView pv;

    public void Init(int ID)
    {
        if(pv == null)
            pv = GetComponent<PhotonView>();
        characterProps = CharacterResource.Instance.FindCharacterWithID(ID).ShallowCopy();
        Debug.Log(ID);
        Debug.Log(characterProps.ID);
      //  if(pv.IsMine)
      //  {
            Name.text = characterProps.Name;
            Icon.sprite = characterProps.Character_Sprite;
      //  }
        UpdateHealth(characterProps.Health);
        SetAttack(characterProps.Attack_Power);
        SetDefence(characterProps.Defence);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
