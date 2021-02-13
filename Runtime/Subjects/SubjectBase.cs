using System;

namespace Skibitsky.Urx
{
    public abstract class SubjectBase<T> : ISubject<T>, IDisposable
    {
        public bool Disposed { get; protected set; }
        public bool Terminated { get; protected set; }
        
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(T value);
        public abstract IDisposable Subscribe(IObserver<T> observer);
        public abstract void Dispose();
        internal abstract void Unsubscribe(Subscription subscription);
        
        protected void ThrowIfDisposed()
        {
            if (Disposed) throw new ObjectDisposedException(nameof(SubjectBase<T>));
        }
        
        internal sealed class Subscription : IDisposable
        {
            private SubjectBase<T> _subject;
            private readonly object _locker = new object();
            
            public IObserver<T> Observer { get; private set; }

            public Subscription(SubjectBase<T> subject, IObserver<T> observer)
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