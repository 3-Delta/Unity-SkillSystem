using EGamePlay.Combat.Ability;

namespace EGamePlay.Combat {
    /// <summary>
    /// 普攻执行体
    /// </summary>
    public class AttackExecution : AbilityExecution {
        public AttackActionAbility AttackActionAbility { get; set; }

        public override void Update() { }

        public override void BeginExecute() {
            if (OwnerEntity.DamageActionAbilityEntity.TryCreateAction(out var action)) {
                action.Target = AttackActionAbility.Target;
                action.DamageSource = DamageSource.Attack;
                action.ApplyDamage();
            }

            this.EndExecute();
        }

        public override void EndExecute() {
            base.EndExecute();
            AttackActionAbility = null;
        }
    }
}