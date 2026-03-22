using UnityEngine;
using System.Collections.Generic;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] private string playerId;
    [SerializeField] private string username;

    [SerializeField] private List<BattleCharacter> characterGroup = new List<BattleCharacter>();

    public string PlayerId => playerId;
    public string Username => username;
    public List<BattleCharacter> CharacterGroup => characterGroup;
    public void Init(string id, string name)
    {
        playerId = id;
        username = name;
    }

    public bool HasLivingCharacters()
    {
        foreach (var character in characterGroup)
        {
            if (character.CurrentHP > 0) return true;
        }
        return false;
    }
    public void AddCharacterToGroup(BattleCharacter newCharacter)
    {

        characterGroup.Add(newCharacter);

    }
}