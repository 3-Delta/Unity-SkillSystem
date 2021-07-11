using System.Collections.Generic;
using System;
using Sirenix.Utilities;

namespace EGamePlay.Combat
{
    /// <summary>
    /// 浮点型修饰器
    /// </summary>
    public class NumericModifier<T> where T : struct
    {
        public T Value { get; set; }
    }

    public class FloatModifier : NumericModifier<float> { }

    public class NumericModifierCollection<T, TModifier> where T : struct// , TModifier NumericModifier<T> 
    {
        public T value { get; private set; }
        private List<TModifier> modifierList = new List<TModifier>(0);

        public void Reset() {
            value = default;
            modifierList.Clear();
        }

        public T AddModifier(TModifier modifier)
        {
            modifierList.Add(modifier);
            AddValue(modifier);
            return value;
        }

        public T RemoveModifier(TModifier modifier)
        {
            modifierList.Remove(modifier);
            RemoveValue(modifier);
            return value;
        }

        protected virtual void AddValue(TModifier modifier) {
            
        }
        protected virtual void RemoveValue(TModifier modifier) {
            
        }

        public virtual T Calc() {
            foreach (var mod in modifierList) {
                AddValue(mod);
            }
            return value;
        }
    }

    public class FloatModifierCollection : NumericModifierCollection<float, FloatNumeric> {
        
    }

    /// <summary>
    /// 浮点型修饰器集合
    /// </summary>
    // public class FloatModifierCollection
    // {
    //     public float TotalValue { get; private set; }
    //     private List<FloatModifier> Modifiers { get; } = new List<FloatModifier>();
    //
    //     public float AddModifier(FloatModifier modifier)
    //     {
    //         Modifiers.Add(modifier);
    //         Update();
    //         return TotalValue;
    //     }
    //
    //     public float RemoveModifier(FloatModifier modifier)
    //     {
    //         Modifiers.Remove(modifier);
    //         Update();
    //         return TotalValue;
    //     }
    //
    //     public void Update()
    //     {
    //         TotalValue = 0;
    //         foreach (var item in Modifiers)
    //         {
    //             TotalValue += item.Value;
    //         }
    //     }
    // }
    /// <summary>
    /// 浮点型数值
    /// </summary>
    public class FloatNumeric
    {
        public float Value { get; private set; }
        public float baseValue { get; private set; }
        public float add { get; private set; }
        public float pctAdd { get; private set; }
        public float finalAdd { get; private set; }
        public float finalPctAdd { get; private set; }
        private FloatModifierCollection AddCollection { get; } = new FloatModifierCollection();
        private FloatModifierCollection PctAddCollection { get; } = new FloatModifierCollection();
        private FloatModifierCollection FinalAddCollection { get; } = new FloatModifierCollection();
        private FloatModifierCollection FinalPctAddCollection { get; } = new FloatModifierCollection();


        public void Initialize()
        {
            baseValue = add = pctAdd = finalAdd = finalPctAdd = 0f;
        }
        public float SetBase(float value)
        {
            baseValue = value;
            Update();
            return baseValue;
        }
        public void AddAddModifier(FloatModifier modifier)
        {
            add = AddCollection.AddModifier(modifier);
            Update();
        }
        public void AddPctAddModifier(FloatModifier modifier)
        {
            pctAdd = PctAddCollection.AddModifier(modifier);
            Update();
        }
        public void AddFinalAddModifier(FloatModifier modifier)
        {
            finalAdd = FinalAddCollection.AddModifier(modifier);
            Update();
        }
        public void AddFinalPctAddModifier(FloatModifier modifier)
        {
            finalPctAdd = FinalPctAddCollection.AddModifier(modifier);
            Update();
        }
        public void RemoveAddModifier(FloatModifier modifier)
        {
            add = AddCollection.RemoveModifier(modifier);
            Update();
        }
        public void RemovePctAddModifier(FloatModifier modifier)
        {
            pctAdd = PctAddCollection.RemoveModifier(modifier);
            Update();
        }
        public void RemoveFinalAddModifier(FloatModifier modifier)
        {
            finalAdd = FinalAddCollection.RemoveModifier(modifier);
            Update();
        }
        public void RemoveFinalPctAddModifier(FloatModifier modifier)
        {
            finalPctAdd = FinalPctAddCollection.RemoveModifier(modifier);
            Update();
        }

        public void Update()
        {
            var value1 = baseValue;
            var value2 = (value1 + add) * (100 + pctAdd) / 100f;
            var value3 = (value2 + finalAdd) * (100 + finalPctAdd) / 100f;
            Value = value3;
        }
    }
}
