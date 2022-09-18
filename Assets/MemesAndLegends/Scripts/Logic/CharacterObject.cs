
using UnityEngine;


[CreateAssetMenu(fileName = "Character Object", menuName = "ScriptableObjects/CharacterObject", order = 1)]
public class CharacterObject : ScriptableObject
{
    public CharacterObject ShallowCopy()
    {
        return (CharacterObject)this.MemberwiseClone();
    }

    [Header("Character Info")]
    public string Name;
    public string Description;
    public string Role;
    public int Level;
    public int ID;
    public Type Character_Class;
    public Sprite Character_Sprite;
    public Sprite Character_Power_Sprite;

    [Header("Character Stats")]
    public int Attack_Power;
    public int Defence;
    public int Health;
    
    [Header("Movement")]
    public MovementType Movement;
    public int Movement_Range;

    [Header("Attack")]
    public AttackType Attack;
    public int Attack_Range;

    [Header("Special Abillity")]
    public bool HasSpecial;
    public AbillityType Abillity;
    public string Special_Description;
    public int Power;
    public int Power_Range;

    [Header("Limit")]
    public int Limit;

    public enum AbillityType
    {
        Increase_Defence = 1,
        True_Damage = 2,
        GainXP = 3,
        Heal = 4
    }

    public enum MovementType
    {
        Omni_Directional = 1,
        Diagnole = 2,
        Plus = 3
    }

    public enum AttackType
    {
        Omni_Directional = 1,
        None = 2
    }
    public enum Type
    {
        Mortal_Class = 1,
        Monster_Class = 2
    }



}
