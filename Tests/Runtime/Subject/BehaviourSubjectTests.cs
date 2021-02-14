using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    public class BehaviourSubjectTests : SubjectTestBase
    {
        [Test] public void SubscribeToCompleted() => TestSubscribeToCompleted(new BehaviorSubject<int>(7));
        [Test] public void SubscribeToErrored() => TestSubscribeToErrored(new BehaviorSubject<int>(7));
        
        [Test]
        public void OnCompleted()
        {
            var expectedOnNext = new List<int> {7, 1, 10, 100};
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;
            var subject = new BehaviorSubject<int>(7);

            subject.Subscribe(x => onNext.Add(x), x => exceptions.Add(x), () => onCompletedCallCount++);
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            Assert.That(onNext, Is.EqualTo(expectedOnNext));
            Assert.That(exceptions, Has.Count.EqualTo(0));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));

            subject.OnCompleted();
            Assert.That(onCompletedCallCount, Is.EqualTo(1));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(200);
            Assert.That(onNext, Is.EqualTo(expectedOnNext));
            Assert.That(exceptions, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestOnError()
        {
            var expectedOnNext = new List<int> {7, 1, 10, 100};
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;
            var subject = new BehaviorSubject<int>(7);
            
            subject.Subscribe(x => onNext.Add(x), x => exceptions.Add(x), () => onCompletedCallCount++);
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            Assert.That(onNext, Is.EqualTo(expectedOnNext));
            Assert.That(exceptions, Has.Count.EqualTo(0));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));
            
            subject.OnError(new Exception());
            Assert.That(exceptions, Has.Count.EqualTo(1));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(200);
            Assert.That(onNext, Is.EqualTo(expectedOnNext));
            Assert.That(onCompletedCallCount, Is.EqualTo(0));
        }

        [Test]
        public void TestUnsubscribe()
        {
            var onNextFirst = new List<int>();
            var onNextSecond = new List<int>();
            var subject = new BehaviorSubject<int>(7);

            var subscriptionFirst = subject.Subscribe(x => onNextFirst.Add(x));
            var subscriptionSecond = subject.Subscribe(x => onNextSecond.Add(x));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            
            subscriptionSecond.Dispose();
            
            subject.OnNext(2);
            subject.OnNext(20);
            subject.OnNext(200);

            Assert.That(onNextFirst, Is.EqualTo(new List<int> {7, 1, 10, 100, 2, 20, 200}));
            Assert.That(onNextSecond, Is.EqualTo(new List<int> {7, 1, 10, 100}));
        }
    }
}