using UnityEngine;

[CreateAssetMenu(fileName = "SwitchMove", menuName = "Create/SwitchMove")]
public class SwitchMove : CharacterMove
{
    public override void Execute(BattleCharacter user, BattleCharacter _)
    {
        user.playerOwner.PerformSwitch(user.queuedSwitchIndex);
        Debug.Log($"{user.gameObject.name} retreated!");
    }
}
