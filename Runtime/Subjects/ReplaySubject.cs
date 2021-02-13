using System;
using System.Collections.Generic;

namespace Skibitsky.Urx
{
    public class ReplaySubject<T> : SubjectBase<T>
    {
        private readonly SubjectBase<T> _subject;

        private readonly Action<IObserver<T>> _replay;
        private readonly Action<T> _add;
        private readonly Action? _trim; 
        
        public ReplaySubject(uint bufferSize)
        {
            if (bufferSize == 0) throw new ArgumentException("Buffer size cannot be 0");
            
            _subject = new Subject<T>();

            (_replay, _add, _trim) = bufferSize switch
            {
                1 => CreateReplayOnce(),
                uint.MaxValue => CreateReplayAll(),
                _ => CreateReplayMany(bufferSize)
            };
        }

        private static (Action<IObserver<T>>, Action<T>, Action) CreateReplayMany(uint bufferSize)
        {
            var replay = new ReplayMany(bufferSize);
            return (replay.Replay, replay.Add, replay.Trim);
        }        
        
        private static (Action<IObserver<T>>, Action<T>, Action) CreateReplayOnce()
        {
            var replay = new ReplayOnce();
            return (replay.Replay, replay.Add, null);
        }
        
        private static (Action<IObserver<T>>, Action<T>, Action) CreateReplayAll()
        {
            var replay = new ReplayAll();
            return (replay.Replay, replay.Add, null);
        }

        public override void OnCompleted() => _subject.OnCompleted();
        public override void OnError(Exception error) => _subject.OnError(error);
        public override void OnNext(T value)
        {
            if (_subject.Terminated) return;
            
            _add(value);
            _trim?.Invoke();
            
            _subject.OnNext(value);
        }

        public override IDisposable Subscribe(IObserver<T> observer)
        {
            var disposable = _subject.Subscribe(observer);
            _replay(observer);
            return disposable;
        }

        public override void Dispose() => _subject.Dispose();
        internal override void Unsubscribe(Subscription subscription) => _subject.Unsubscribe(subscription);

        private sealed class ReplayMany
        {
            private readonly uint _bufferSize;
            private readonly Queue<T> _queue = new Queue<T>();

            public ReplayMany(uint bufferSize)
            {
                _bufferSize = bufferSize;
            }
            
            public void Add(T value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                
                _queue.Enqueue(value);
            }

            public void Replay(IObserver<T> observer)
            {
                foreach (var item in _queue)
                    observer.OnNext(item);
            }

            public void Trim()
            {
                if (_queue.Count > _bufferSize) _queue.Dequeue();
            }
        }

        private sealed class ReplayOnce
        {
            private T _value;
            
            public void Add(T value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                
                _value = value;
            }

            public void Replay(IObserver<T> observer) => observer.OnNext(_value);
        }

        private sealed class ReplayAll
        {
            private readonly IList<T> _list = new List<T>();
            
            public void Add(T value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _list.Add(value);
            }

            public void Replay(IObserver<T> observer)
            {
                foreach (var item in _list)
                    observer.OnNext(item);
            }
        }
    }
}