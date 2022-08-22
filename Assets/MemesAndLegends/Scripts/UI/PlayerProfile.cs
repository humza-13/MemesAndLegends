using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
	public TMPro.TMP_InputField nicknameInput;

	public GameObject ProfilePanel;
    public GameObject userNamePanel;
   public void onProfileMenu()
    {
        ProfilePanel.SetActive(true);
    }
    public void ExitProfile()
    {
        ProfilePanel.SetActive(false);
    }

    public void onUserDetailsChanged()
    {
        ClientInfo.Username = nicknameInput.text;
        nicknameInput.text = ClientInfo.Username;
        userNamePanel.SetActive(false);

    }

    public void onSetUserName()
    {
        userNamePanel.SetActive(true);
        nicknameInput.text= ClientInfo.Username;
    }
    public void closeUserName()
    {
        userNamePanel.SetActive(false);
    }
}
