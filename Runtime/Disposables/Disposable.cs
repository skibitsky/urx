using System;

namespace Skibitsky.Urx.Disposables
{
    public static class Disposable
    {
        public static readonly IDisposable Empty = EmptyDisposable.Singleton;

        public static IDisposable Create(Action onDisposed)
        {
            if (onDisposed == null) throw new ArgumentNullException(nameof(onDisposed));
            
            return new AnonymousDisposable(onDisposed);
        }

        public static IDisposable CreateWithState<TState>(TState state, Action<TState> onDisposed)
        {
            if (onDisposed == null) throw new ArgumentNullException(nameof(onDisposed));
            
            return new AnonymousDisposable<TState>(state, onDisposed);
        }

        private sealed class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Singleton = new EmptyDisposable();

            private EmptyDisposable() { }

            public void Dispose() { }
        }

        private sealed class AnonymousDisposable : IDisposable
        {
            private readonly Action _onDisposed;
            private bool _disposed;

            public AnonymousDisposable(Action onDisposed)
            {
                _onDisposed = onDisposed;
            }

            public void Dispose()
            {
                if (_disposed) return;

                _disposed = true;
                _onDisposed?.Invoke();
            }
        }
        
        private sealed class AnonymousDisposable<TState> : IDisposable
        {
            private readonly Action<TState> _onDisposed;
            private readonly TState _state;
            private bool _disposed;

            public AnonymousDisposable(TState state, Action<TState> onDisposed)
            {
                _onDisposed = onDisposed;
                _state = state;
            }

            public void Dispose()
            {
                if (_disposed) return;

                _disposed = true;
                _onDisposed?.Invoke(_state);
            }
        }
    }
}