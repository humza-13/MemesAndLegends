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
    public List<Charactercontroller> characters;
    public GameObject CharacterPrefab;

    public PhotonView pv;
    void Awake()
    {
        var spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint")[0].GetComponent<Transform>();
        this.transform.SetParent(spawnPoint, false);

    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        if (pv == null)
            pv = GetComponent<PhotonView>();

        if (pv.IsMine)
            Init(PhotonNetwork.LocalPlayer);
        else
            Init(PhotonNetwork.PlayerListOthers[0]);
    }
    public void Init(Player p)
    {
      
        PlayerName.text = p.NickName;
        UpdateXp((int)p.CustomProperties["XP"]);
        InitCharacter((int)p.CustomProperties["c1"],0);
        InitCharacter((int)p.CustomProperties["c2"],1);
        InitCharacter((int)p.CustomProperties["c3"],2);
        InitCharacter((int)p.CustomProperties["c4"],3);
        
    }

    void InitCharacter(int ID, int index)
    {
        characters[index].Init(ID, index);
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
