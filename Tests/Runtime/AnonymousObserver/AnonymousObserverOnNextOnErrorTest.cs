using System;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
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