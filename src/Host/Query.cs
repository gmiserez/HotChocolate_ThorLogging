using System;
using static SwissLife.Fusion.Document.EventSources.ApplicationEventSource;

namespace Demo
{
    public class Query
    {
        public string FieldWithException()
            => throw new Exception("FieldWithException threw an exception.");

        public string NonNullableFieldWithNullResult() => null;

        public string FieldThatLogsAWarning()
        {
            Log.FieldWasInvoked();

            return "foo";
        }
    }
}
