using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class PlayerController : MonoBehaviour, IPunObservable
{
    public TMP_Text PlayerName;
    public TMP_Text PlayerXPText;
    public Slider XpSlider;
    public Transform characterContent;
    public List<Charactercontroller> characters;
    public GameObject CharacterPrefab;
    public int XP;

    public PhotonView pv;
    void Awake()
    {
        var spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>();
        this.transform.SetParent(spawnPoint, false);
        BoardManager.Instance.players.Add(this);
        XP = ClientInfo.XP;
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
        if (p.IsMasterClient)
            this.transform.SetAsFirstSibling();
      
        PlayerName.text = p.NickName;
        UpdateXp((int)p.CustomProperties["XP"]);
        InitCharacter((int)p.CustomProperties["c1"],0);
        InitCharacter((int)p.CustomProperties["c2"],1);
        InitCharacter((int)p.CustomProperties["c3"],2);
        InitCharacter((int)p.CustomProperties["c4"],3);
        
    }

    [PunRPC]
    public void RewardPlayerXP(int xp)
    {
          XP += xp;
            UpdateXp(XP);
    }

    void InitCharacter(int ID, int index)
    {
        characters[index].Init(ID, index);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(XP);
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
        XpSlider.DOValue(Xp, 1.5f, true);
        PlayerXPText.text = Xp.ToString();

    }

    public void AssignXP()
    {
        if(!pv.IsMine)
            return;

        ClientInfo.XP = XP;
    }

    public void CheckGameEnd()
    {
        if (characters.Count <= 0)
            Invoke(nameof(EndGame), 1f);
          
    }
    private void EndGame()
    {
        BoardManager.Instance.pv.RPC("EndGame", RpcTarget.All, PhotonNetwork.LocalPlayer);
        AssignXP();
    }
}
