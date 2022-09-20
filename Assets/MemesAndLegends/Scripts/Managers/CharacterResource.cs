using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResource : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
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
