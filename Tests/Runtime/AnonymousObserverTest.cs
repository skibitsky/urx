using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public class AnonymousObserverTest<T>
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

    public class AnonymousObserverOnNextTest
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
            Assert.Throws<AnonymousObserverOnNext_TestException>(() => observer.OnError(new AnonymousObserverOnNext_TestException()));
        }

        [Test]
        public void OnNext_IntSequence()
        {
            var results = new List<int>();
            const int expectedCount = 3;
            
            var observer = new AnonymousObserverOnNext<int>(results.Add);

            observer.OnNext(2);
            observer.OnNext(1);
            observer.OnNext(0);

            Assert.That(results, Has.Count.EqualTo(expectedCount));
            Assert.That(results, Is.Ordered.Descending);
        }

        [Test]
        public void OnNext_ThrowTextException_ShouldThrowTestException()
        {
            const string expectedMsg = "Oops!";
            static void ThrowTestException(string msg) => throw new AnonymousObserverOnNext_TestException(msg);
            
            var observer = new AnonymousObserverOnNext<string>(ThrowTestException);

            var ex = Assert.Throws<AnonymousObserverOnNext_TestException>(() => observer.OnNext(expectedMsg));
            Assert.That(ex.Message, Is.EqualTo(expectedMsg));
        }

        [Test]
        public void OnCompleted_Empty_ShouldNotThrow()
        {
            var observer = CreateEmpty();
            
            Assert.DoesNotThrow(() => observer.OnCompleted());
        }

        private static AnonymousObserverOnNext<object> CreateEmpty() =>
            new AnonymousObserverOnNext<object>(_ => { });
        
        // ReSharper disable once InconsistentNaming
        private class AnonymousObserverOnNext_TestException : Exception
        {
            public AnonymousObserverOnNext_TestException() { }
            public AnonymousObserverOnNext_TestException(string msg) : base(msg) { }
        }
    }

    public class AnonymousObserverOnNextOnErrorTest
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
        public void OnNext_IntSequence()
        {
            var results = new List<int>();
            const int expectedCount = 3;
            
            var observer = new AnonymousObserverOnNextOnError<int>(results.Add, e => {});

            observer.OnNext(2);
            observer.OnNext(1);
            observer.OnNext(0);

            Assert.That(results, Has.Count.EqualTo(expectedCount));
            Assert.That(results, Is.Ordered.Descending);
        }

        private static AnonymousObserverOnNextOnError<object> CreateEmpty() =>
            new AnonymousObserverOnNextOnError<object>(_ => { }, _ => { });

        // ReSharper disable once InconsistentNaming
        private class AnonymousObserverOnNextOnError_TestException : Exception
        {
            public AnonymousObserverOnNextOnError_TestException() { }
            public AnonymousObserverOnNextOnError_TestException(string msg) : base(msg) { }
        }
    }
}