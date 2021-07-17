# Unity-SkillSystem
Unity手游技能系统学习

学习：
https://github.com/m969/EGamePlay

1. https://zhuanlan.zhihu.com/p/269901872 数值类Numeric设计的很不错, 战斗总数值 = 基础数值 + 额外加成(装备/符文/伙伴等固定加成) + buff加成(动态加成)
2. 将Update设置为一个Entity的Component, 这种EC模式(中介者模式), 然后UpdateComponent调用Entity的update方法,如果没有updatecomponent,则自然调用不到entity的update
3. 战斗实体CombatEntity其实就是CombatActor, 然后CombatEntity会创建skillEntity, buffEntity, 技能的具体表现通过行为树或者这里的entityExecution实现
4. 命名优雅,CombatContext,Execution之类的
