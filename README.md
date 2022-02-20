# Unity-SkillSystem
## 进度: 20%
Unity手游技能系统学习

学习：
1. https://github.com/m969/EGamePlay
2. https://developer.valvesoftware.com/wiki/Dota_2_Workshop_Tools:zh-cn/Scripting:zh-cn/Abilities_Data_Driven:zh-cn
3. https://www.zhihu.com/question/29545727/answer/786293709 技能是CombatActor的castor对于target施加的瞬间的数值影响,而buff则是一个持续作用的数值变化的作用,装备/符文等施加影响时间更长. 最难的是技能效果, 而不是底层的数值影响.
4. https://zhuanlan.zhihu.com/codingart 
5. https://zhuanlan.zhihu.com/p/147681650
6. https://github.com/lsunky/SkillEditorDemo

1. https://zhuanlan.zhihu.com/p/269901872 数值类Numeric设计的很不错, 战斗总数值 = 基础数值 + 额外加成(装备/符文/伙伴等固定加成) + buff加成(动态加成)
2. 将Update设置为一个Entity的Component, 这种EC模式(中介者模式), 然后UpdateComponent调用Entity的update方法,如果没有updatecomponent,则自然调用不到entity的update
3. https://zhuanlan.zhihu.com/p/340447052 战斗实体CombatEntity其实就是CombatActor, 然后CombatEntity会创建skillEntity, buffEntity, 技能的具体表现通过行为树或者这里的entityExecution实现
4. buff触发的条件机制
5. 伤害公式的动态解析使得公式更加灵活
6. 命名优雅,CombatContext,Execution之类的
7. 期待技能同步机制
