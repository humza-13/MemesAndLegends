using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public TMP_Text PlayerName;
    public Slider XpSlider;
    public Transform characterContent;
    public List<CharacterController> characters;
    public GameObject CharacterPrefab;

    public PhotonView pv;

    public void Init()
    {
        ClientInfo.XP = 200;
        if(pv == null)
            pv = GetComponent<PhotonView>();
        
      //  if (pv.IsMine)
       // {
            PlayerName.text = PhotonNetwork.LocalPlayer.NickName;
            UpdateXp(ClientInfo.XP);

            foreach (int ID in ClientInfo.PlayerCharacters)
                InitCharacter(ID);
       // }
    }

    void InitCharacter(int ID)
    {
       // if(pv.IsMine)
        //{
            var temp =  PhotonNetwork.Instantiate(CharacterPrefab.name, new Vector2(characterContent.position.x, characterContent.position.y), Quaternion.identity);
            temp.gameObject.GetComponent<CharacterController>().Init(ID);
            temp.gameObject.transform.SetParent(characterContent);

        //}
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ClientInfo.XP);
        }
        if (stream.IsReading)
        {
            UpdateXp((int)stream.ReceiveNext());
        }
    }

    public void UpdateXp(int Xp)
    {
       // if (pv.IsMine)
      //  {
            if (Xp > XpSlider.maxValue)
                XpSlider.maxValue *= 10;
            XpSlider.value = Xp;
      //  }
    }
}
