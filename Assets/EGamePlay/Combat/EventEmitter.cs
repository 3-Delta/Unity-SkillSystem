using System;
using System.Collections;
using System.Collections.Generic;

public class EventEmitter<TEnum> where TEnum : Enum {
    private Dictionary<TEnum, List<Delegate>> _delegates = new Dictionary<TEnum, List<Delegate>>(0);

    protected void Add(TEnum eventType, Delegate action) {
        if (action != null) {
            if (!_delegates.TryGetValue(eventType, out List<Delegate> ls)) {
                ls = new List<Delegate>(1) {action};
                _delegates.Add(eventType, ls);
                return;
            }

            if (action.GetType() != ls[0].GetType()) {
                ls.Add(action);
            }
        }
    }

    protected void Remove(TEnum eventType, Delegate action) {
        if (action != null) {
            if (_delegates.TryGetValue(eventType, out List<Delegate> ls) && ls.Count > 0) {
                if (action.GetType() == ls[0].GetType()) {
                    ls.Remove(action);
                }
            }
        }
    }

    public void Handle(TEnum eventType, Action action, bool toReigster) {
        if (toReigster) {
            Add(eventType, action);
        }
        else {
            Remove(eventType, action);
        }
    }

    public void Handle<TArg1>(TEnum eventType, Action<TArg1> action, bool toReigster) {
        if (toReigster) {
            Add(eventType, action);
        }
        else {
            Remove(eventType, action);
        }
    }

    public void Handle<TArg1, TArg2>(TEnum eventType, Action<TArg1, TArg2> action, bool toReigster) {
        if (toReigster) {
            Add(eventType, action);
        }
        else {
            Remove(eventType, action);
        }
    }

    public void Handle<TArg1, TArg2, TArg3>(TEnum eventType, Action<TArg1, TArg2, TArg3> action, bool toReigster) {
        if (toReigster) {
            Add(eventType, action);
        }
        else {
            Remove(eventType, action);
        }
    }

    public void Fire(TEnum eventType) {
        if (_delegates.TryGetValue(eventType, out var ls)) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action)?.Invoke();
            }
        }
    }

    public void Fire<TArg1>(TEnum eventType, TArg1 arg1) {
        if (_delegates.TryGetValue(eventType, out var ls)) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1>)?.Invoke(arg1);
            }
        }
    }

    public void Fire<TArg1, TArg2>(TEnum eventType, TArg1 arg1, TArg2 arg2) {
        if (_delegates.TryGetValue(eventType, out var ls)) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1, TArg2>)?.Invoke(arg1, arg2);
            }
        }
    }

    public void Fire<TArg1, TArg2, TArg3>(TEnum eventType, TArg1 arg1, TArg2 arg2, TArg3 arg3) {
        if (_delegates.TryGetValue(eventType, out var ls)) {
            for (int i = ls.Count - 1; i >= 0; i--) {
                (ls[i] as Action<TArg1, TArg2, TArg3>)?.Invoke(arg1, arg2, arg3);
            }
        }
    }
}
