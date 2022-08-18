using UnityEngine;


#region Models
[System.Serializable]
public class PlayerDataPref
{
	[System.Serializable]
	public class RecordPref
	{
		public string username;
		public int xp;
	}
	public RecordPref player;
}
#endregion 

[System.Serializable]
public static class DataManager
{
    #region Set Data
 //   public static void SetPlayer()
	//{
	//	PlayerDataPref data = new PlayerDataPref();
	//	data.player = new PlayerDataPref.RecordPref();
	//	data.player.username = PlayerData.Instance.username;
	//	data.player.xp = PlayerData.Instance.xp;

	//	string rawData = JsonUtility.ToJson(data);
	//	PlayerPrefs.SetString("Player_Attributes", rawData);
	//	PlayerPrefs.Save();
	//}
 //   #endregion
 //   #region Get Data
 //   public static void GetPlayer()
	//{
	//	if (PlayerPrefs.HasKey("Player_Attributes"))
	//	{
	//		string rawData = PlayerPrefs.GetString("Player_Attributes");
	//		PlayerDataPref data = ParsePlayer(rawData);

	//		PlayerData.Instance.username = data.player.username;
	//		PlayerData.Instance.xp = data.player.xp;

	//	}
	//	else
	//	{
	//		ApplyDefaultPlayerSettings();
	//	}
	//}

    #endregion

    #region Defaults
 //   static void ApplyDefaultPlayerSettings()
	//{
	//	PlayerData.Instance.username = "Default User";
	//	PlayerData.Instance.xp = 0;
	//	SetPlayer();
	//}

    #endregion

    #region Parsers
    private static PlayerDataPref ParsePlayer(string rawData)
	{
		PlayerDataPref data;
		try
		{
			data = JsonUtility.FromJson<PlayerDataPref>(rawData);
		}
		catch
		{
			data = null;
		}
		return data;
	}
    #endregion
}
