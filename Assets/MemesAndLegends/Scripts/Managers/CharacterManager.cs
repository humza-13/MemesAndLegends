using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Character Select Attributes")]
    public GameObject characterSelectPanel;
    
    [Header("Character Add Attributes")]
    public GameObject characterPanel;
    public List<CharacterPanel> rooms;

  

    [Header("Character Select Section")]
    public TMPro.TMP_Text characterName;
    public TMPro.TMP_Text characterRole;
    public TMPro.TMP_Text characterDescription;
    public TMPro.TMP_Text characterlevel;
    public TMPro.TMP_Text characterlimit;
    public TMPro.TMP_Text characterSpecial;
    public TMPro.TMP_Text playerXp;
    public Slider characterlevelFill;
    public Slider characterHealth;
    public Slider characterAttack;
    public Slider characterDefence;
    public Image character;
    public Image characterPowerIcon;
    public Image characterPower;
    public CharacterResource resource;
    private int _currentSelected = 0;

    private void Awake()
    {
        resource = GameObject.FindGameObjectWithTag("CharacterResource").GetComponent<CharacterResource>();
    }
    private void Start()
    {
        SetCharacterStage(resource.characterObjectsLevel1[_currentSelected]);
        playerXp.text = ClientInfo.XP.ToString();
    }
    public void MoveNext()
    {
        _currentSelected = _currentSelected >= resource.characterObjectsLevel1.Count-1 ? 0 : (_currentSelected + 1);
        SetCharacterStage(resource.characterObjectsLevel1[_currentSelected]);
    }
    public void SetCharacterStage(CharacterObject obj)
    {
        characterName.text = obj.Name;
        characterRole.text = obj.Role;
        characterDescription.text = obj.Description;
        characterlevel.text = obj.Level.ToString();
        characterlimit.text = obj.Limit == 0 ? "None" : obj.Limit.ToString();
        characterSpecial.text = obj.Special_Description;
        characterlevelFill.value = obj.Level * 10;
        characterHealth.value = obj.Health;
        characterDefence.value = obj.Defence;
        characterAttack.value = obj.Attack_Power;
        character.sprite = obj.Character_Sprite;
        characterPowerIcon.sprite = obj.Character_Power_Sprite;
        characterPower.sprite = obj.Character_Power_Sprite;
    }
    public void UpdateSelected()
    {
        for(int i = 0; i < ClientInfo.PlayerCharacters.Count; i++)
        {
            rooms[i].AssignCharacter(ClientInfo.PlayerCharacters[i]);
        }
    }

    public void OpenCharacterSelect()
    {
        characterSelectPanel.SetActive(true);
        playerXp.text = ClientInfo.XP.ToString();
    }
    public void CloseCharacterSelect()
    {
        characterSelectPanel.SetActive(false);
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
        ClientInfo.SetCharacters(resource.characterObjectsLevel1[_currentSelected].ID);
        UpdateSelected();
        CloseCharacterSelect();
    }

    [System.Serializable]
    public class CharacterPanel
    {
        public int ID;
        public Button Delete;
        public Image Icon;
        public Image Player;
        public TMPro.TMP_Text Name;
        public TMPro.TMP_Text Level;

        [Header("Inactive Character Slots Components")]
        public List<GameObject> defaultLayout;

        [Header("Active Character Slots Components")]
        public List<GameObject> assignedLayout;

        public void AssignCharacter(int ID)
        {
            var _char = GameObject.FindGameObjectWithTag("CharacterResource").GetComponent<CharacterResource>().FindCharacterWithID(ID);
            this.ID = ID;

            Icon.sprite = _char.Character_Power_Sprite;
            Player.sprite = _char.Character_Sprite;
            Level.text = "Level: " + _char.Level.ToString();
            Name.text = _char.Name;
            
            Delete.onClick.RemoveAllListeners();
            Delete.onClick.AddListener(() => RemoveCharacter(this.ID));

            foreach (var component in defaultLayout)
                component.SetActive(false);
            foreach (var component in assignedLayout)
                component.SetActive(true);
        }

        public void RemoveCharacter(int ID)
        {
            ClientInfo.DeleteCharacters(ID);
          
            foreach (var component in defaultLayout)
                component.SetActive(true);
            foreach (var component in assignedLayout)
                component.SetActive(false);

        }


    }
}
