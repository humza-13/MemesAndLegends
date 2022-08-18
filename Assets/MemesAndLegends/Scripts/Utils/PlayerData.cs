using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Player Data", order = 51)]
public class PlayerData : ScriptableObject
{
	private static PlayerData _instance = null;
	public static PlayerData Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (PlayerData)Resources.Load("Player Data");
				if (_instance == null)
				{
					throw new UnityException("Asset can't found");
				}
			}
			return _instance;
		}
	}

	[Header("Player Attributes")]
	public string username;
	public int xp;
}
