using System;

namespace Skibitsky.Urx
{
    public static class ObservableExtensions
    {
        // The onNext Action provided is invoked for each value
        // OnError notifications are re-thrown as Exceptions
        public static IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
            Action<TSource> onNext)
        { 
            return source.Subscribe(new AnonymousObserverOnNext<TSource>(onNext));
        }

        // The onNext Action is invoked for each value
        // The onError Action is invoked for errors
        public static IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
            Action<TSource> onNext,
            Action<Exception> onError)
        {
            return source.Subscribe(new AnonymousObserverOnNextOnError<TSource>(onNext, onError));
        }
        
        // The onNext Action is invoked for each value
        // The onCompleted Action is invoked when the source completes
        // OnError notifications are re-thrown as Exceptions
        public static IDisposable Subscribe<TSource>(this IObservable<TSource> source, 
            Action<TSource> onNext,
            Action onCompleted)
        {
            return source.Subscribe(new AnonymousObserverOnNextOnCompleted<TSource>(onNext, onCompleted));
        }

        public static IDisposable Subscribe<TSource>(this IObservable<TSource> source,
            Action<TSource> onNext,
            Action<Exception> onError,
            Action onCompleted)
        {
            return source.Subscribe(new AnonymousObserverOnNextOnErrorOnCompleted<TSource>(onNext, onError, onCompleted));
        }
    }
}