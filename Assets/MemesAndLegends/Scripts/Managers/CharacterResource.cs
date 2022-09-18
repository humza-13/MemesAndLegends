using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResource : MonoBehaviour
{
    private static CharacterResource _instance;
    public static CharacterResource Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CharacterResource>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Header("Level 1 Characters")]
    public List<CharacterObject> characterObjectsLevel1;

    [Header("Level 2 Characters")]
    public List<CharacterObject> characterObjectsLevel2;

    public CharacterObject FindCharacterWithID(int ID)
    {
       foreach(CharacterObject character in characterObjectsLevel1)
            if(character.ID == ID)
                return character;
       
        return null;
    }
}
