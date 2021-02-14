using System;
using NUnit.Framework;
using Skibitsky.Urx.Disposables;

namespace Skibitsky.Urx.Tests
{
    public class DisposableTest
    {
        [Test]
        public void Create()
        {
            var flag = false;
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            IDisposable disposable = Disposable.Create(() => flag = true);
            disposable.Dispose();
            
            Assert.That(flag, Is.True);
        }

        [Test]
        public void CreateWithState()
        {
            const int stateTarget = 27;
            var state = 0;
            
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            IDisposable disposable = Disposable.CreateWithState(stateTarget, x => state = x);
            disposable.Dispose();

            Assert.That(state, Is.EqualTo(stateTarget));
        }
    }
}