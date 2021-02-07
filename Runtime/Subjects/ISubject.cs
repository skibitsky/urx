using System;

namespace Skibitsky.Urx
{
    public interface ISubject<in TSource, out TResult> : IObserver<TSource>, IObservable<TResult> { }
    public interface ISubject<T> : ISubject<T, T> { }
}