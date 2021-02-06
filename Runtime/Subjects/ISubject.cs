using System;

namespace Skibitsky.Urx
{
    public interface ISubject<T> : IObserver<T>, IObservable<T> { }
}