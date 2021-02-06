using System;

namespace Skibitsky.Urx
{
    public sealed class Subject<T> : SubjectBase<T>
    {
        private Subscription[] _subscriptions;
        private bool _disposed;
        
        private readonly object _locker = new object();
        
        public override void OnCompleted()
        {
            ThrowIfDisposed();
            
            lock (_locker)
            {
                foreach (var subscription in _subscriptions)
                    subscription.Observer.OnCompleted();
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
            }
        }

        public override void OnNext(T value)
        {
            ThrowIfDisposed();

            lock (_locker)
            {
                foreach (var subscription in _subscriptions)
                    subscription.Observer.OnNext(value);
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
                
                observer.OnCompleted();
                
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
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(Subject<T>));
        }

        private void Unsubscribe(Subscription subscription)
        {
            lock (_locker)
            {
                var i = Array.IndexOf(_subscriptions, subscription);
                var l = _subscriptions.Length;

                if (i < 0) return;

                Subscription[] newArray;
                if (i == 1) newArray = Array.Empty<Subscription>();
                else
                {
                    newArray = new Subscription[l - 1];

                    Array.Copy(_subscriptions, 0, newArray, 0, i);
                    Array.Copy(_subscriptions, i + 1, newArray, i, l - i - 1);
                }
                
                _subscriptions = newArray;
            }
        }

        private sealed class Subscription : IDisposable
        {
            private Subject<T> _subject;
            private readonly object _locker = new object();
            
            public IObserver<T> Observer { get; private set; }

            public Subscription(Subject<T> subject, IObserver<T> observer)
            {
                _subject = subject;
                Observer = observer;
            }

            public void Dispose()
            {
                _subject.Unsubscribe(this);
                
                lock (_locker)
                {
                    Observer = null;
                    _subject = null;
                }
            }
        }
    }
}