using System;

namespace Skibitsky.Urx.Subjects
{
    public interface ISubject<in TSource, out TResult> : IObserver<TSource>, IObservable<TResult> { }
    public interface ISubject<T> : ISubject<T, T> { }
}