using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    public class SubjectTest : SubjectTestBase
    {
        [Test] public void OnCompleted() => TestOnCompleted(new Subject<int>());
        [Test] public void OnError() => TestOnError(new Subject<int>());
        [Test] public void Unsubscribe() => TestUnsubscribe(new Subject<int>());
        [Test] public void SubscribeToCompleted() => TestSubscribeToCompleted(new Subject<int>());
        [Test] public void SubscribeToErrored() => TestSubscribeToErrored(new Subject<int>());
    }
}
