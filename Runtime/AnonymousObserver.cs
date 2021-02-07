using System;

namespace Skibitsky.Urx
{
    public class AnonymousObserverOnNext<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;

        public AnonymousObserverOnNext(Action<T> onNext)
        {
            _onNext = onNext ?? throw new ArgumentNullException(nameof(onNext));
        }

        public virtual void OnCompleted() { }
        public virtual void OnError(Exception error) => throw error;
        public void OnNext(T value) => _onNext.Invoke(value);
    }
    
    public class AnonymousObserverOnNextOnError<T> : AnonymousObserverOnNext<T>
    {
        private readonly Action<Exception> _onError;

        public AnonymousObserverOnNextOnError(Action<T> onNext, Action<Exception> onError) : base(onNext)
        {
            _onError = onError ?? throw new ArgumentNullException(nameof(onError));
        }

        public override void OnError(Exception error) => _onError.Invoke(error);
    }
    
    public class AnonymousObserverOnNextOnCompleted<T> : AnonymousObserverOnNext<T>
    {
        private readonly Action _onCompleted;

        public AnonymousObserverOnNextOnCompleted(Action<T> onNext, Action onCompleted) : base(onNext)
        {
            _onCompleted = onCompleted ?? throw new ArgumentNullException(nameof(onCompleted));
        }

        public override void OnCompleted() => _onCompleted.Invoke();
    }
    
    public class AnonymousObserverOnNextOnErrorOnCompleted<T> : AnonymousObserverOnNextOnError<T>
    {
        private readonly Action _onCompleted;

        public AnonymousObserverOnNextOnErrorOnCompleted(Action<T> onNext, Action<Exception> onError,
            Action onCompleted) : base(onNext, onError)
        {
            _onCompleted = onCompleted ?? throw new ArgumentNullException(nameof(onCompleted));
        }

        public override void OnCompleted() => _onCompleted.Invoke();
    }
}