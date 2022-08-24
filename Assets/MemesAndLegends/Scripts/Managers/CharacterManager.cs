using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Character Select Attributes")]
    public GameObject characterSelect;
    
    [Header("Character Panel Attributes")]
    public GameObject characterPanel;

    [Header("Inactive Character Slots Components")]
    public List<GameObject> defaultLayout;

    [Header("Active Character Slots Components")]
    public List<GameObject> assignedLayout;

    public void OpenCharacterSelect()
    {
        characterSelect.SetActive(true);
    }
    public void CloseCharacterSelect()
    {
        characterSelect.SetActive(false);
    }
    public void OpenCharacterPanel()
    {
        characterPanel.SetActive(true);
    }
    public void CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
    }
    public void AssignCharacter()
    {
        foreach (var component in defaultLayout)
            component.SetActive(false);
        foreach (var component in assignedLayout)
            component.SetActive(true);
        CloseCharacterSelect();
    }
    public void DeleteSlot()
    {
        foreach (var component in defaultLayout)
            component.SetActive(true);
        foreach (var component in assignedLayout)
            component.SetActive(false);
    }
}
