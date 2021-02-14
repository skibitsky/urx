using System;

namespace Skibitsky.Urx
{
    public class BehaviorSubject<T> : SubjectBase<T>
    {
        private T _value;
        
        private readonly SubjectBase<T> _subject;
        private readonly object _locker = new object();

        public BehaviorSubject(T initialValue)
        {
            _value = initialValue;
            _subject = new Subject<T>();
        }
        
        public override void OnCompleted() => _subject.OnCompleted();
        
        public override void OnError(Exception error) => _subject.OnError(error);
        
        public override void OnNext(T value)
        {
            lock (_locker)
            {
                if (!_subject.IsCompleted) _value = value;
                _subject.OnNext(value);
            }
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            lock (_locker)
            {
                var disposable = _subject.Subscribe(observer);
                if (!_subject.IsCompleted) observer.OnNext(_value);
                return disposable;
            }
        }

        public override void Dispose() => _subject.Dispose();

        internal override void Unsubscribe(Subscription subscription) => _subject.Unsubscribe(subscription);
    }
}