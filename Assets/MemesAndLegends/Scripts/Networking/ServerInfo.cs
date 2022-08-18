using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServerInfo {

    public const int UserCapacity = 2;
    public static string LobbyName;

    public static int MaxUsers {
        get => PlayerPrefs.GetInt("S_MaxUsers", 2);
        set => PlayerPrefs.SetInt("S_MaxUsers", Mathf.Clamp(value, 1, UserCapacity));
    }
}