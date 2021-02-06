using System;

namespace Skibitsky.Urx
{
    public abstract class SubjectBase<T> : ISubject<T>, IDisposable
    {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(T value);
        public abstract IDisposable Subscribe(IObserver<T> observer);
        public abstract void Dispose();
    }
}