using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGamePlay;

namespace EGamePlay.Combat {
    public class MotionActionAbilityEntity : ActionAbilityEntity<MotionActionAbilityExecution> { }

    /// <summary>
    /// 动作行动
    /// </summary>
    public class MotionActionAbilityExecution : ActionAbilityExecution<MotionActionAbilityEntity> {
        public int MotionType { get; set; }

        //前置处理
        private void PreProcess() { }

        public void ApplyMotion() {
            PreProcess();

            PostProcess();
        }

        //后置处理
        private void PostProcess() {
            //Creator.TriggerActionPoint(ActionPointType.PostGiveCure, this);
            //Target.TriggerActionPoint(ActionPointType.PostReceiveCure, this);
        }
    }
}
