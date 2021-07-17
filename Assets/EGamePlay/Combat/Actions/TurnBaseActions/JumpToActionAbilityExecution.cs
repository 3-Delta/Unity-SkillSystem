using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;
using EGamePlay.Combat;
using ET;

namespace EGamePlay.Combat {
    public class JumpToActionAbilityEntity : ActionAbilityEntity<JumpToActionAbilityExecution> { }

    public class JumpToActionAbilityExecution : ActionAbilityExecution<JumpToActionAbilityEntity> {
        //前置处理
        private void PreProcess() {
            Creator.TriggerActionPoint(ActionPointType.PreJumpTo, this);
        }

        public async ETTask ApplyJumpTo() {
            PreProcess();

            await TimeHelper.WaitAsync(Creator.JumpToTime);

            PostProcess();

            if (Creator.AttackActionAbilityEntity.TryCreateAction(out var attackAction)) {
                attackAction.Target = Target;
                await attackAction.ApplyAttackAwait();
            }

            ApplyAction();
        }

        //后置处理
        private void PostProcess() {
            Creator.TriggerActionPoint(ActionPointType.PostJumpTo, this);
        }
    }
}
