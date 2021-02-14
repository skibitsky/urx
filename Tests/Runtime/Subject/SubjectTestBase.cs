using System;
using System.Collections.Generic;
using NUnit.Framework;
using Skibitsky.Urx.Subjects;

namespace Skibitsky.Urx.Tests
{
    public class SubjectTestBase
    {
        protected static void TestOnCompleted(ISubject<int> subject)
        {
            var expectedOnNext = new List<int> {1, 10, 100};
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;

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

        protected static void TestOnError(ISubject<int> subject)
        {
            var expectedOnNext = new List<int> {1, 10, 100};
            var onNext = new List<int>();
            var exceptions = new List<Exception>();
            var onCompletedCallCount = 0;
            
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

        protected static void TestUnsubscribe(ISubject<int> subject)
        {
            var onNextFirst = new List<int>();
            var onNextSecond = new List<int>();

            var subscriptionFirst = subject.Subscribe(x => onNextFirst.Add(x));
            var subscriptionSecond = subject.Subscribe(x => onNextSecond.Add(x));
            
            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnNext(100);
            
            subscriptionSecond.Dispose();
            
            subject.OnNext(2);
            subject.OnNext(20);
            subject.OnNext(200);

            Assert.That(onNextFirst, Is.EqualTo(new List<int> {1, 10, 100, 2, 20, 200}));
            Assert.That(onNextSecond, Is.EqualTo(new List<int> {1, 10, 100}));
        }
        
        protected static void TestSubscribeToCompleted(ISubject<int> subject)
        {
            var onNext = new List<int>();

            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnCompleted();
            subject.OnNext(2);
            subject.OnNext(20);
            
            subject.Subscribe(x => onNext.Add(x));
            subject.OnNext(3);
            subject.OnNext(30);
            
            Assert.That(onNext, Has.Count.EqualTo(0));
        }
        
        protected static void TestSubscribeToErrored(ISubject<int> subject)
        {
            var onNext = new List<int>();

            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnError(new Exception());
            subject.OnNext(2);
            subject.OnNext(20);
            
            subject.Subscribe(x => onNext.Add(x), _ => { });
            subject.OnNext(3);
            subject.OnNext(30);
            
            Assert.That(onNext, Has.Count.EqualTo(0));
        }
    }
}