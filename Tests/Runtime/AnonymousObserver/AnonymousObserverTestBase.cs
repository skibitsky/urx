using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
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
}