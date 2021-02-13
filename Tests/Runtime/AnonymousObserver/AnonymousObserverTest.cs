using System;
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
}