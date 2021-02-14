using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    public class ReplaySubjectTests : SubjectTestBase
    {
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(int.MaxValue)]
        public void OnCompleted(int bufferSize) => TestOnCompleted(new ReplaySubject<int>(bufferSize));
        
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(int.MaxValue)]
        public void OnError(int bufferSize) => TestOnError(new ReplaySubject<int>(bufferSize));
        
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(int.MaxValue)]
        public void Unsubscribe(int bufferSize) => TestUnsubscribe(new ReplaySubject<int>(bufferSize));
        
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(int.MaxValue)]
        public void SubscribeToCompleted(int bufferSize) => TestSubscribeToCompleted(new ReplaySubject<int>(bufferSize));

        [Test]
        public void ReplayOnce()
        {
            var expected = new List<int> {1, 2, 3};
            var onNext = new List<int>();
            var subject = new ReplaySubject<int>(1);

            subject.OnNext(0);
            subject.OnNext(1);
            subject.Subscribe(x => onNext.Add(x));
            subject.OnNext(2);
            subject.OnNext(3);
            
            Assert.That(onNext, Is.EqualTo(expected));
        }
        
        [Test]
        public void ReplayAll()
        {
            var expected = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var onNext = new List<int>();
            var subject = new ReplaySubject<int>(int.MaxValue);

            for (var i = 0; i < 10; i++)
                subject.OnNext(i);

            subject.Subscribe(x => onNext.Add(x));

            Assert.That(onNext, Is.EqualTo(expected));
        }
        
        [Test]
        public void ReplayMany()
        {
            var expected = new List<int> {1, 2, 3, 4};
            var onNext = new List<int>();
            var subject = new ReplaySubject<int>(3);

            subject.OnNext(0);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.Subscribe(x => onNext.Add(x));
            subject.OnNext(4);
            
            Assert.That(onNext, Is.EqualTo(expected));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void SignedOrZeroBufferSize_ShouldThrowArgumentException(int bufferSize)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new ReplaySubject<int>(bufferSize));
        }
    }
}