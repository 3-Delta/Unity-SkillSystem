using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Combat {
    public class RangeNumeric<T> where T : struct {
        public T Min { get; }
        public T Max { get; }

        protected T _value;
        public virtual T Value { get; set; }

        public RangeNumeric(T min, T max, T target) {
            this.Min = min;
            this.Max = max;
            this._value = target;
        }

        public virtual float Percent { get; }
        public virtual bool IsPercentFull { get; }
    }

    public class IntRangeNumeric : RangeNumeric<int> {
        public override int Value {
            get {
                _value = Math.Min(Min, _value);
                _value = Math.Max(Max, _value);
                return _value;
            }
            set => _value = value;
        }

        public IntRangeNumeric(int min, int max, int target) : base(min, max, target) { }

        public override float Percent {
            get { return 1f * Value / Max; }
        }

        public override bool IsPercentFull {
            get { return Percent >= 0; }
        }
    }

    public class FloatRangeNumeric : RangeNumeric<float> {
        public override float Value {
            get {
                _value = Mathf.Min(Min, _value);
                _value = Mathf.Max(Max, _value);
                return _value;
            }
            set => _value = value;
        }

        public FloatRangeNumeric(float min, float max, float target) : base(min, max, target) { }

        public override float Percent {
            get { return 1f * Value / Max; }
        }

        public override bool IsPercentFull {
            get { return Percent >= 0; }
        }
    }

    #region Numeric
    public class Numeric<T, TModifier, TModifierCollection>
        where T : struct
        where TModifier : NumericModifier<T>
        where TModifierCollection : NumericModifierCollection<T, TModifier>, new() {
        protected bool dirty = false;

        #region 最终值
        protected T _finalValue = default;

        public T FinalValue {
            get {
                if (dirty) {
                    Calc();
                }

                return _finalValue;
            }
        }
        #endregion

        #region 基础值
        public T baseValue { get; protected set; }
        #endregion

        #region 装备/武器/伙伴/符文等添加的固定数值
        public T fixedAdd { get; protected set; }
        public int fixedAddPercent { get; protected set; }

        protected TModifierCollection fixedAddCollection { get; } = new TModifierCollection();
        protected IntModifierCollection fixedAddPercentCollection { get; } = new IntModifierCollection();
        #endregion

        #region buff等添加的动态数值
        public T dynamicAdd { get; protected set; }
        public int dynamicAddPercent { get; protected set; }

        protected TModifierCollection dynamicAddCollection { get; } = new TModifierCollection();
        protected IntModifierCollection dynamicAddPercentCollection { get; } = new IntModifierCollection();
        #endregion

        public void Reset() {
            baseValue = fixedAdd = dynamicAdd = default;
            fixedAddPercent = dynamicAddPercent = 0;
        }

        public void AddFixedAddModifier(TModifier modifier) {
            fixedAdd = fixedAddCollection.AddModifier(modifier);
            Update();
        }

        public void AddFixedAddPercentModifier(IntModifier modifier) {
            fixedAddPercent = fixedAddPercentCollection.AddModifier(modifier);
            Update();
        }

        public void AddDynamicAddModifier(TModifier modifier) {
            dynamicAdd = dynamicAddCollection.AddModifier(modifier);
            Update();
        }

        public void AddDynamicAddPercentModifier(IntModifier modifier) {
            dynamicAddPercent = dynamicAddPercentCollection.AddModifier(modifier);
            Update();
        }

        public void RemoveAddModifier(TModifier modifier) {
            fixedAdd = fixedAddCollection.RemoveModifier(modifier);
            Update();
        }

        public void RemovePctAddModifier(IntModifier modifier) {
            fixedAddPercent = fixedAddPercentCollection.RemoveModifier(modifier);
            Update();
        }

        public void RemoveFinalAddModifier(TModifier modifier) {
            dynamicAdd = dynamicAddCollection.RemoveModifier(modifier);
            Update();
        }

        public void RemoveFinalPctAddModifier(IntModifier modifier) {
            dynamicAddPercent = dynamicAddPercentCollection.RemoveModifier(modifier);
            Update();
        }

        private void Update() {
            dirty = true;
        }

        protected virtual void Calc() { }
    }

    public class IntNumeric : Numeric<int, IntModifier, IntModifierCollection> {
        protected virtual void Calc() {
            var value1 = baseValue;
            var value2 = (value1 + fixedAdd) * (100 + fixedAddPercent) * 0.01f;
            var value3 = (value2 + dynamicAdd) * (100 + dynamicAddPercent) * 0.01f;
            _finalValue = (int) value3;
        }
    }

    public class FloatNumeric : Numeric<float, FloatModifier, FloatModifierCollection> {
        protected virtual void Calc() {
            var value1 = baseValue;
            var value2 = (value1 + fixedAdd) * (100 + fixedAddPercent) * 0.01f;
            var value3 = (value2 + dynamicAdd) * (100 + dynamicAddPercent) * 0.01f;
            _finalValue = (float) value3;
        }
    }
    #endregion

    #region NumericModifier
    public class NumericModifier<T> where T : struct {
        public T Value { get; set; }
    }

    public class IntModifier : NumericModifier<int> { }

    public class FloatModifier : NumericModifier<float> { }
    #endregion

    #region NumericModifierCollection
    public class NumericModifierCollection<T, TModifier>
        where T : struct
        where TModifier : NumericModifier<T> {
        public virtual T Value { get; } = default;
        protected readonly List<TModifier> modifierList = new List<TModifier>(0);

        public void Reset() {
            modifierList.Clear();
        }

        public T AddModifier(TModifier modifier) {
            modifierList.Add(modifier);
            return Value;
        }

        public T RemoveModifier(TModifier modifier) {
            modifierList.Remove(modifier);
            return Value;
        }
    }

    public class IntModifierCollection : NumericModifierCollection<int, IntModifier> {
        public override int Value {
            get {
                int rlt = 0;
                for (int i = modifierList.Count - 1; i >= 0; --i) {
                    rlt += modifierList[i].Value;
                }

                return rlt;
            }
        }
    }

    public class FloatModifierCollection : NumericModifierCollection<float, FloatModifier> {
        public override float Value {
            get {
                float rlt = 0;
                for (int i = modifierList.Count - 1; i >= 0; --i) {
                    rlt += modifierList[i].Value;
                }

                return rlt;
            }
        }
    }
    #endregion
}
