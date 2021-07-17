using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;

namespace EGamePlay.Combat {
    public enum ConditionType {
        [LabelText("当x秒内没有受伤")] WhenInTimeNoDamage = 0,
        [LabelText("当生命值低于x")] WhenHPLower = 1,
        [LabelText("当生命值低于百分比x")] WhenHPPctLower = 2,
    }
}
