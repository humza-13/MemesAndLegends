using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileManager : MonoBehaviour
{
    [Header("Profile Elements")]
    public GameObject profilePanel;
    private void Awake()
    {
        DataManager.GetPlayer();
    }

    public void OnProfileClicked()
    {
       profilePanel.SetActive(true);
    }
    public void CloseProfile()
    {
        profilePanel.SetActive(false);
    }
}
