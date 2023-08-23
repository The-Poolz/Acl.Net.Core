using System.Runtime.Serialization;

namespace Acl.Net.Core.Managers.Tests.Exceptions.Testable;

public abstract class ExceptionSerializationTestBase<TException, TTestableException>
    where TTestableException : Exception
{
    protected SerializationInfo infoForGetObjectData = null!;

    protected void RunSerializationTest(string message)
    {
        var info = new SerializationInfo(typeof(TException), new FormatterConverter());
        info.AddValue("Message", message);
        info.AddValue("InnerException", null, typeof(Exception));
        info.AddValue("HelpURL", null, typeof(string));
        info.AddValue("StackTraceString", null, typeof(string));
        info.AddValue("RemoteStackTraceString", null, typeof(string));
        info.AddValue("HResult", int.MinValue, typeof(int));
        info.AddValue("Source", null, typeof(string));
        info.AddValue("ClassName", "Exception");
        info.AddValue("RemoteStackIndex", 0, typeof(int));
        info.AddValue("ExceptionMethod", null, typeof(string));

        var context = new StreamingContext();

        var exception = CreateTestableException(info, context);

        infoForGetObjectData = new SerializationInfo(typeof(TException), new FormatterConverter());
        exception.GetObjectData(infoForGetObjectData, context);
    }

    protected abstract TTestableException CreateTestableException(SerializationInfo info, StreamingContext context);
}
