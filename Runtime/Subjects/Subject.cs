using System;

namespace Skibitsky.Urx
{
    public sealed class Subject<T> : SubjectBase<T>
    {
        private Subscription[] _subscriptions = Array.Empty<Subscription>();

        private readonly object _locker = new object();
        
        public override void OnCompleted()
        {
            ThrowIfDisposed();
            
            lock (_locker)
            {
                foreach (var subscription in _subscriptions)
                    subscription.Observer.OnCompleted();
                
                IsCompleted = true;
            }
        }

        public override void OnError(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            
            ThrowIfDisposed();
            
            lock (_locker)
            {
                foreach (var subscription in _subscriptions)
                    subscription.Observer.OnError(error);
                
                IsCompleted = true;
            }
        }

        public override void OnNext(T value)
        {
            lock (_locker)
            {
                if (IsCompleted) return;
                
                for (var i = 0; i < _subscriptions.Length; i++)
                    _subscriptions[i].Observer.OnNext(value);
            }
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            var subject = new Subscription(this, observer);
            
            ThrowIfDisposed();

            lock (_locker)
            {
                var l = _subscriptions.Length;
                var newArray = new Subscription[l + 1];
                
                Array.Copy(_subscriptions, 0, newArray, 0, l);
                
                newArray[l] = subject;
                _subscriptions = newArray;

                return subject;
            }
        }

        public override void Dispose()
        {
            lock (_locker)
            {
                foreach (var subscription in _subscriptions)
                    subscription.Dispose();
                
                _subscriptions = null;
                Disposed = true;
            }
        }

        internal override void Unsubscribe(Subscription subscription)
        {
            lock (_locker)
            {
                var i = Array.IndexOf(_subscriptions, subscription);
                if (i < 0) return;
                
                var l = _subscriptions.Length;

                Subscription[] newArray;
                if (i == 0) newArray = Array.Empty<Subscription>();
                else
                {
                    newArray = new Subscription[l - 1];

                    Array.Copy(_subscriptions, 0, newArray, 0, i);
                    Array.Copy(_subscriptions, i + 1, newArray, i, l - i - 1);
                }
                
                _subscriptions = newArray;
            }
        }
    }
}