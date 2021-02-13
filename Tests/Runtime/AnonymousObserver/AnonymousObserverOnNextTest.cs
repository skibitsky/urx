using System;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
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
}