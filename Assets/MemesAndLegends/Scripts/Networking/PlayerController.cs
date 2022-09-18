using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public TMP_Text PlayerName;
    public Slider XpSlider;
    public Transform characterContent;
    public List<CharacterController> characters;
    public GameObject CharacterPrefab;

    public PhotonView pv;

    public void Init(Player p)
    {
        if(pv == null)
            pv = GetComponent<PhotonView>();
        
        if (!pv.IsMine)
            return;
        
        PlayerName.text = p.NickName;
        UpdateXp((int)p.CustomProperties["XP"]);
        InitCharacter((int)p.CustomProperties["c1"]);
        InitCharacter((int)p.CustomProperties["c2"]);
        InitCharacter((int)p.CustomProperties["c3"]);
        InitCharacter((int)p.CustomProperties["c4"]);
        
    }

    void InitCharacter(int ID)
    {
        var temp =  PhotonNetwork.Instantiate(CharacterPrefab.name, new Vector2(characterContent.position.x, characterContent.position.y), Quaternion.identity);
        temp.gameObject.transform.SetParent(characterContent);
        temp.gameObject.GetComponent<CharacterController>().Init(ID);
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
        if (Xp > XpSlider.maxValue)
            XpSlider.maxValue *= 10;
        XpSlider.value = Xp; 
    }
}
