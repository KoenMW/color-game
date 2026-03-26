using UnityEngine;

[CreateAssetMenu(fileName = "NewStatMove", menuName = "Create/StatBuffMove")]
public class StatChangeMove : CharacterMove
{
    [SerializeField] private StatType statToChange;

    // If true, changes the user's stats. If false, changes the target's stats
    [SerializeField] private bool targetSelf = true;

    public override void Execute(BattleCharacter user, BattleCharacter target)
    {
        // 1. Figure out who is getting their stats changed
        BattleCharacter receiver = targetSelf ? user : target;

        // 2. Apply the specific stat change
        switch (statToChange)
        {
            case StatType.Attack:
                receiver.ChangeAttack(Power);
                break;
            case StatType.Speed:
                receiver.ChangeSpeed(Power);
                break;
        }
        // 3. Print a nice message to the console
        string effectType = Power >= 0 ? "raised" : "lowered";
        Debug.Log($"{user.gameObject.name} used {Name}! {receiver.gameObject.name}'s {statToChange} was {effectType} by {Mathf.Abs(Power)}!");
    }
}
