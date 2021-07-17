using EGamePlay.Combat.Ability;
using EGamePlay.Combat.Status;
using EGamePlay.Combat.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGamePlay.Combat {
    /// <summary>
    /// 战斗实体
    /// </summary>
    public sealed class CombatEntity : Entity {
        public GameObject ModelObject { get; set; }
        public HealthPoint CurrentHealth { get; private set; } = new HealthPoint();

        public Dictionary<Type, ActionAbilityEntity> TypeActions { get; set; } = new Dictionary<Type, ActionAbilityEntity>();
        
        #region 刚好是Actions/下面的8个ActionExecution
        public SpellActionAbilityEntity SpellActionAbilityEntity { get; private set; }
        public MotionActionAbilityEntity MotionActionAbilityEntity { get; private set; }
        public DamageActionAbilityEntity DamageActionAbilityEntity { get; private set; }
        public CureActionAbilityEntity CureActionAbilityEntity { get; private set; }
        public AttackActionAbilityEntity AttackActionAbilityEntity { get; private set; }
        public AssignEffectActionAbilityEntity AssignEffectActionAbilityEntity { get; private set; }
        public TurnActionAbilityEntity TurnActionAbilityEntity { get; private set; }
        public JumpToActionAbilityEntity JumpToActionAbilityEntity { get; private set; }
        #endregion

        public AttackAbilityEntity AttackAbilityEntity { get; set; }
        public SkillExecution CurrentSkillExecution { get; set; }
        public Dictionary<string, SkillAbility> NameSkills { get; set; } = new Dictionary<string, SkillAbility>();
        public Dictionary<KeyCode, SkillAbility> InputSkills { get; set; } = new Dictionary<KeyCode, SkillAbility>();
        public Dictionary<string, List<StatusAbility>> TypeIdStatuses { get; set; } = new Dictionary<string, List<StatusAbility>>();
        public Dictionary<Type, List<StatusAbility>> TypeStatuses { get; set; } = new Dictionary<Type, List<StatusAbility>>();

        public Vector3 Position { get; set; }
        public float Direction { get; set; }
        public ActionControlType ActionControlType { get; set; }

        public override void Awake() {
            AddComponent<AttributeComponent>();
            AddComponent<ActionPointManageComponent>();
            AddComponent<ConditionMgrComponent>();
            //AddComponent<MotionComponent>();
            CurrentHealth.SetMaxValue((int) GetComponent<AttributeComponent>().HealthPoint.Value);
            CurrentHealth.Reset();
            SpellActionAbilityEntity = AttachActionAbility<SpellActionAbilityEntity>();
            MotionActionAbilityEntity = AttachActionAbility<MotionActionAbilityEntity>();
            DamageActionAbilityEntity = AttachActionAbility<DamageActionAbilityEntity>();
            CureActionAbilityEntity = AttachActionAbility<CureActionAbilityEntity>();
            AttackActionAbilityEntity = AttachActionAbility<AttackActionAbilityEntity>();
            AssignEffectActionAbilityEntity = AttachActionAbility<AssignEffectActionAbilityEntity>();
            TurnActionAbilityEntity = AttachActionAbility<TurnActionAbilityEntity>();
            JumpToActionAbilityEntity = AttachActionAbility<JumpToActionAbilityEntity>();
            AttackAbilityEntity = CreateChild<AttackAbilityEntity>();
        }

        /// <summary>
        /// 创建行动
        /// </summary>
        public T CreateAction<T>() where T : ActionAbilityExecution {
            var action = Parent.GetComponent<CombatActionManageComponent>().CreateAction<T>(this);
            return action;
        }

        #region 行动点事件
        public void ListenActionPoint(ActionPointType actionPointType, Action<ActionAbilityExecution> action) {
            GetComponent<ActionPointManageComponent>().AddListener(actionPointType, action);
        }

        public void UnListenActionPoint(ActionPointType actionPointType, Action<ActionAbilityExecution> action) {
            GetComponent<ActionPointManageComponent>().RemoveListener(actionPointType, action);
        }

        public void TriggerActionPoint(ActionPointType actionPointType, ActionAbilityExecution actionAbility) {
            GetComponent<ActionPointManageComponent>().TriggerActionPoint(actionPointType, actionAbility);
        }
        #endregion

        #region 条件事件
        public void ListenerCondition(ConditionType conditionType, Action action, object paramObj = null) {
            GetComponent<ConditionMgrComponent>().AddListener(conditionType, action, paramObj);
        }

        public void UnListenCondition(ConditionType conditionType, Action action) {
            GetComponent<ConditionMgrComponent>().RemoveListener(conditionType, action);
        }
        #endregion

        public void ReceiveDamage(ActionAbilityExecution combatActionAbility) {
            var damageAction = combatActionAbility as DamageActionAbilityExecution;
            CurrentHealth.Minus(damageAction.DamageValue);
        }

        public void ReceiveCure(ActionAbilityExecution combatActionAbility) {
            var cureAction = combatActionAbility as CureActionAbilityExecution;
            CurrentHealth.Add(cureAction.CureValue);
        }

        public bool CheckDead() {
            return CurrentHealth.Value <= 0;
        }

        /// <summary>
        /// 挂载能力，技能、被动、buff都通过这个接口挂载
        /// </summary>
        /// <param name="configObject"></param>
        private T AttachAbility<T>(object configObject) where T : AbilityEntity {
            var ability = Entity.CreateWithParent<T>(this, configObject);
            return ability;
        }

        public T AttachActionAbility<T>() where T : ActionAbilityEntity {
            var action = AttachAbility<T>(null);
            TypeActions.Add(typeof(T), action);
            return action;
        }

        public T AttachSkill<T>(object configObject) where T : SkillAbility {
            var skill = AttachAbility<T>(configObject);
            NameSkills.Add(skill.SkillConfig.Name, skill);
            return skill;
        }

        public T AttachStatus<T>(object configObject) where T : StatusAbility {
            var status = AttachAbility<T>(configObject);
            if (!TypeIdStatuses.ContainsKey(status.StatusConfigObject.ID)) {
                TypeIdStatuses.Add(status.StatusConfigObject.ID, new List<StatusAbility>());
            }

            TypeIdStatuses[status.StatusConfigObject.ID].Add(status);
            return status;
        }

        public void OnStatusRemove(StatusAbility statusAbility) {
            TypeIdStatuses[statusAbility.StatusConfigObject.ID].Remove(statusAbility);
            if (TypeIdStatuses[statusAbility.StatusConfigObject.ID].Count == 0) {
                TypeIdStatuses.Remove(statusAbility.StatusConfigObject.ID);
            }

            this.Publish(new RemoveStatusEvent() {CombatEntity = this, Status = statusAbility, StatusId = statusAbility.Id});
        }

        public void BindSkillInput(SkillAbility abilityEntity, KeyCode keyCode) {
            InputSkills.Add(keyCode, abilityEntity);
            abilityEntity.TryActivateAbility();
        }

        public bool HasStatus<T>(T statusType) where T : StatusAbility {
            return TypeStatuses.ContainsKey(statusType.GetType());
        }

        public bool HasStatus(string statusTypeId) {
            return TypeIdStatuses.ContainsKey(statusTypeId);
        }

        public StatusAbility GetStatus(string statusTypeId) {
            return TypeIdStatuses[statusTypeId][0];
        }

        #region 回合制战斗
        public int SeatNumber { get; set; }
        public int JumpToTime { get; set; }
        public bool IsHero { get; set; }
        public bool IsMonster => IsHero == false;

        public CombatEntity GetEnemy(int seat) {
            if (IsHero) {
                return GetParent<CombatContext>().GetMonster(seat);
            }
            else {
                return GetParent<CombatContext>().GetHero(seat);
            }
        }

        public CombatEntity GetTeammate(int seat) {
            if (IsHero) {
                return GetParent<CombatContext>().GetHero(seat);
            }
            else {
                return GetParent<CombatContext>().GetMonster(seat);
            }
        }
        #endregion
    }

    public class RemoveStatusEvent {
        public CombatEntity CombatEntity { get; set; }
        public StatusAbility Status { get; set; }
        public long StatusId { get; set; }
    }
}
