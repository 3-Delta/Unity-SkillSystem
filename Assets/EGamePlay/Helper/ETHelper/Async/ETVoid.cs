using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ET {
    // 学习资源 https://www.cnblogs.com/raytheweak/p/9130594.html
    [AsyncMethodBuilder(typeof(AsyncETVoidMethodBuilder))]
    public struct ETVoid : ICriticalNotifyCompletion {
        [DebuggerHidden]
        public void Coroutine() { }

        [DebuggerHidden] public bool IsCompleted => true;

        [DebuggerHidden]
        public void OnCompleted(Action continuation) { }

        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation) { }
    }
}
