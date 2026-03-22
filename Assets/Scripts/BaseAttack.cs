using UnityEngine;

[CreateAssetMenu(fileName = "BaseMove", menuName = "Create/BaseMove")]
public class BaseAttack : CharacterMove
{
    public override void Execute(BattleCharacter user, BattleCharacter target)
    {
        Debug.Log($"piew!");
    }
}