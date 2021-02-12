using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class AnonymousObserverTest
    {
        [Test]
        public void AnonymousObserver_Null_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AnonymousObserverOnNext<object>(null));
            Assert.Throws<ArgumentNullException>(() => new AnonymousObserverOnNextOnError<object>(null, null));
            Assert.Throws<ArgumentNullException>(() => new AnonymousObserverOnNextOnCompleted<object>(null, null));
            Assert.Throws<ArgumentNullException>(() => new AnonymousObserverOnNextOnErrorOnCompleted<object>(null, null, null));
        }
    }

    public abstract class AnonymousObserverTestBase
    {
        [TestCase(0)]
        [TestCase(4)]
        public void OnNext_IntSequence(int onNextCallsCount)
        {
            var results = new List<int>();
            
            var observer = GetObserverInstance<int>(results.Add);

            for (var i = 0; i < onNextCallsCount; i++)
                observer.OnNext(onNextCallsCount - i);
            
            Assert.That(results, Has.Count.EqualTo(onNextCallsCount));
            Assert.That(results, Is.Ordered.Descending);
        }
        
        [Test]
        public void OnNext_ThrowException_ShouldThrowException()
        {
            const string exceptionMsg = "Oops!";
            static void ThrowException(string msg) => throw new Exception(msg);
            
            var observer = GetObserverInstance<string>(ThrowException);

            var ex = Assert.Throws<Exception>(() => observer.OnNext(exceptionMsg));
            Assert.That(ex.Message, Is.EqualTo(exceptionMsg));
        }
        
        protected abstract IObserver<T> GetObserverInstance<T>(Action<T> onNext);
    }

    public class AnonymousObserverOnNextTest : AnonymousObserverTestBase
    {
        [Test]
        public void AnonymousObserverOnNext_Implements_IObserver()
        {
            var observer = CreateEmpty();

            Assert.That(observer, Is.InstanceOf<IObserver<object>>());
            Assert.That(observer, Is.Not.InstanceOf<IObserver<int>>());
            
            Assert.That(observer, Is.Not.InstanceOf<IObservable<int>>());
            Assert.That(observer, Is.Not.InstanceOf<IObservable<object>>());
        }
        
        [Test]
        public void OnError_TestException_ShouldThrowTestException()
        {
            var observer = CreateEmpty();
            Assert.Throws<Exception>(() => observer.OnError(new Exception()));
        }

        [Test]
        public void OnCompleted_Empty_ShouldNotThrow()
        {
            var observer = CreateEmpty();
            
            Assert.DoesNotThrow(() => observer.OnCompleted());
        }
        
        protected override IObserver<T> GetObserverInstance<T>(Action<T> onNext) =>
            new AnonymousObserverOnNext<T>(onNext);

        private static AnonymousObserverOnNext<object> CreateEmpty() =>
            new AnonymousObserverOnNext<object>(_ => { });
    }

    public class AnonymousObserverOnNextOnErrorTest : AnonymousObserverTestBase
    {
        [Test]
        public void AnonymousObserverOnNextOnError_Implements_AnonymousObserverOnNext()
        {
            var observer = CreateEmpty();

            Assert.That(observer, Is.InstanceOf<AnonymousObserverOnNext<object>>());
            Assert.That(observer, Is.Not.InstanceOf<AnonymousObserverOnNext<int>>());

            Assert.That(observer, Is.AssignableTo<AnonymousObserverOnNext<object>>());
        }

        [Test]
        public void OnError_ThrowException_ShouldThrowException()
        {
            const string exceptionMsg = "Oops!";
            var observer = new AnonymousObserverOnNextOnError<int>(_ => { }, e => throw e);
            
            var ex = Assert.Throws<Exception>(() => observer.OnError(new Exception(exceptionMsg)));
            Assert.That(ex.Message, Is.EqualTo(exceptionMsg));
        }
        
        [Test]
        public void OnError_Delegate_ShouldInvokeDelegate()
        {
            const int initialValue = 7;
            const int newValue = 2;
            
            var handle = (object) initialValue;
            void ChangeHandleValue (Exception ex) => handle =  newValue;
            var observer = new AnonymousObserverOnNextOnError<int>(_ => { }, ChangeHandleValue);

            observer.OnError(new Exception());
            
            Assert.That((int)handle, Is.EqualTo(newValue));
        }
        
        [Test]
        public void OnCompleted_Empty_ShouldNotThrow()
        {
            var observer = CreateEmpty();
            
            Assert.DoesNotThrow(() => observer.OnCompleted());
        }
        
        protected override IObserver<T> GetObserverInstance<T>(Action<T> onNext) =>
            new AnonymousObserverOnNextOnError<T>(onNext, _ => { });

        private static AnonymousObserverOnNextOnError<object> CreateEmpty() =>
            new AnonymousObserverOnNextOnError<object>(_ => { }, _ => { });
    }
}