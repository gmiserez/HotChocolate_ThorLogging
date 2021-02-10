using System;
using System.Diagnostics.Tracing;
using Thor.Core;
using Thor.Core.Abstractions;
using Thor.Core.Transmission.Abstractions;

namespace SwissLife.Fusion.Document.EventSources
{
/// <summary>
    /// Represents the bpm engine component tracing events.
    /// </summary>

    [EventSource(Name = "SwissLife-Fusion-Document-Application")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("thor-generator", "1.0.0")]
    public sealed class ApplicationEventSource
        : EventSourceBase
        , IApplicationEventSource
    {
        private ApplicationEventSource() { }

        public static IApplicationEventSource Log { get; } = new ApplicationEventSource();

/// <summary>
        /// This event signals that the application started
        /// </summary>

        [NonEvent]
        public void ApplicationStarted()
        {
            if (IsEnabled())
            {

				ApplicationStarted(Application.Id, ActivityStack.Id);
                
            }
        }

        [Event(1, Level = EventLevel.Informational, Message = "Application started.", Version = 1)]
        private void ApplicationStarted(int applicationId, Guid activityId)
        {
            WriteCore(1, applicationId, activityId);
        }

/// <summary>
        /// This event signals that the field that logs warnings has been invoked
        /// </summary>

        [NonEvent]
        public void FieldWasInvoked()
        {
            if (IsEnabled())
            {

				FieldWasInvoked(Application.Id, ActivityStack.Id);
                
            }
        }

        [Event(2, Level = EventLevel.Warning, Message = "The field that logs warnings has been invoked.", Version = 1)]
        private void FieldWasInvoked(int applicationId, Guid activityId)
        {
            WriteCore(2, applicationId, activityId);
        }

        [NonEvent]
        private unsafe void WriteCore(int eventId, int applicationId, Guid activityId)
        {
            if (IsEnabled())
            {

                    const short dataCount = 2;
                    EventData* data = stackalloc EventData[dataCount];
                    data[0].DataPointer = (IntPtr)(&applicationId);
                    data[0].Size = 4;
                    data[1].DataPointer = (IntPtr)(&activityId);
                    data[1].Size = 16;

                    WriteEventCore(eventId, dataCount, data);
            }
        }

    }
}
