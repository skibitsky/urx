using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    public class SubjectTests
    {
        [Test]
        public void OnCompleted()
        {
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;
            var subject = new Subject<int>();
            
            subject.Subscribe(x => onNext.Add(x), x => exceptions.Add(x), () => onCompletedCallCount++);
            
            subject.OnNext(0);
            subject.OnNext(10);
            subject.OnNext(100);
            Assert.That(onNext, Is.EqualTo(new List<int>{0,10,100}));
            Assert.That(exceptions, Has.Count.EqualTo(0));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));

            subject.OnCompleted();
            Assert.That(onCompletedCallCount, Is.EqualTo(1));
            
            subject.OnNext(0);
            subject.OnNext(10);
            subject.OnNext(200);
            Assert.That(onNext, Is.EqualTo(new List<int>{0,10,100}));
            Assert.That(exceptions, Has.Count.EqualTo(0));
        }

        [Test]
        public void OnError()
        {
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;
            var subject = new Subject<int>();
            
            subject.Subscribe(x => onNext.Add(x), x => exceptions.Add(x), () => onCompletedCallCount++);
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            Assert.That(onNext, Is.EqualTo(new List<int>{0,10,100}));
            Assert.That(exceptions, Has.Count.EqualTo(0));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));
            
            subject.OnError(new Exception());
            Assert.That(exceptions, Has.Count.EqualTo(1));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(200);
            Assert.That(onNext, Is.EqualTo(new List<int>{0,10,100}));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));
        }

        [Test]
        public void Unsubscribe()
        {
            var onNextFirst = new List<int>();
            var onNextSecond = new List<int>();
            var subject = new Subject<int>();

            var subscriptionFirst = subject.Subscribe(x => onNextFirst.Add(x));
            var subscriptionSecond = subject.Subscribe(x => onNextSecond.Add(x));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            
            subscriptionSecond.Dispose();
            
            subject.OnNext(2);
            subject.OnNext(20);
            subject.OnNext(200);
            
            Assert.That(onNextFirst, Is.EqualTo(new List<int>{1,10,100,2,20,200}));
            Assert.That(onNextSecond, Is.EqualTo(new List<int>{1,10,100}));
        }
    }
}
