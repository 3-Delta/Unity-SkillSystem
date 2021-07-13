using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Framework {
    public class EventHandler {
        protected readonly List<Delegate> ls = new List<Delegate>(0);
        public Type actionType => ls.Count > 0 ? ls[0]?.GetType() : null;
        public int Count => ls.Count;

        public void Register(Delegate action, bool toReigster) {
            if (action != null) {
                if (toReigster) {
                    if (Count <= 0 || (actionType == action.GetType())) {
                        ls.Add(action);
                    }
                }
                else {
                    if (Count > 0 && (actionType == action.GetType())) {
                        ls.Remove(action);
                    }
                }
            }
        }

        public void Invoke() {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action)?.Invoke();
            }
        }

        public void Invoke<TArg1>(TArg1 arg1) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1>)?.Invoke(arg1);
            }
        }

        public void Invoke<TArg1, TArg2>(TArg1 arg1, TArg2 arg2) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1, TArg2>)?.Invoke(arg1, arg2);
            }
        }

        public void Invoke<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1, TArg2, TArg3>)?.Invoke(arg1, arg2, arg3);
            }
        }

        #region Tuple
        public void Invoke<TArg1, TArg2, TArg3, TArg4, TArg5>(Tuple<TArg1, TArg2, TArg3, TArg4, TArg5> arg) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<Tuple<TArg1, TArg2, TArg3, TArg4, TArg5>>)?.Invoke(arg);
            }
        }

        public void Invoke(ITuple arg) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<ITuple>)?.Invoke(arg);
            }
        }
        #endregion

        public void Clear() {
            ls.Clear();
        }
    }

    public class EventEmitter<TEnum> where TEnum : Enum {
        private readonly Dictionary<TEnum, EventHandler> _delegates = new Dictionary<TEnum, EventHandler>(0);

        private void _Register(TEnum eventType, Delegate action, bool toReigster) {
            if (toReigster) {
                if (!_delegates.TryGetValue(eventType, out var handler)) {
                    handler = new EventHandler();
                    _delegates.Add(eventType, handler);
                }

                handler.Register(action, true);
            }
            else {
                if (_delegates.TryGetValue(eventType, out var handler)) {
                    handler.Register(action, false);
                }
            }
        }

        public void Register(TEnum eventType, Action action, bool toReigster) {
            _Register(eventType, action, toReigster);
        }

        public void Register<TArg1>(TEnum eventType, Action<TArg1> action, bool toReigster) {
            _Register(eventType, action, toReigster);
        }

        public void Register<TArg1, TArg2>(TEnum eventType, Action<TArg1, TArg2> action, bool toReigster) {
            _Register(eventType, action, toReigster);
        }

        public void Register<TArg1, TArg2, TArg3>(TEnum eventType, Action<TArg1, TArg2, TArg3> action,
            bool toReigster) {
            _Register(eventType, action, toReigster);
        }

        public void Invoke(TEnum eventType) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler.Invoke();
            }
        }

        public void Invoke<TArg1>(TEnum eventType, TArg1 arg1) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler?.Invoke(arg1);
            }
        }

        public void Invoke<TArg1, TArg2>(TEnum eventType, TArg1 arg1, TArg2 arg2) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler?.Invoke(arg1, arg2);
            }
        }

        public void Invoke<TArg1, TArg2, TArg3>(TEnum eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler?.Invoke(arg1, arg2, arg3);
            }
        }

        #region Tuple
        public void Register(TEnum eventType, Action<ITuple> action, bool toReigster) {
            _Register(eventType, action, toReigster);
        }

        public void Invoke(TEnum eventType, ITuple arg) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler?.Invoke(arg);
            }
        }

        public void Invoke<TArg1, TArg2, TArg3, TArg4, TArg5>(TEnum eventType,
            Tuple<TArg1, TArg2, TArg3, TArg4, TArg5> arg) {
            if (_delegates.TryGetValue(eventType, out var handler)) {
                handler?.Invoke(arg);
            }
        }

        public void Register<TArg1, TArg2, TArg3, TArg4, TArg5>(TEnum eventType,
            Action<Tuple<TArg1, TArg2, TArg3, TArg4, TArg5>> action, bool toReigster) {
            _Register(eventType, action, toReigster);
        }
        #endregion

        public void Clear() {
            _delegates.Clear();
        }
    }
}
