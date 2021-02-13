using NUnit.Framework;

namespace Skibitsky.Urx.Tests
{
    public class ReplaySubjectTests : SubjectTestBase
    {
        [Test] public void OnCompleted() => TestOnCompleted(new ReplaySubject<int>(5));
        [Test] public void OnError() => TestOnError(new ReplaySubject<int>(5));
        [Test] public void Unsubscribe() => TestUnsubscribe(new ReplaySubject<int>(5));
    }
}