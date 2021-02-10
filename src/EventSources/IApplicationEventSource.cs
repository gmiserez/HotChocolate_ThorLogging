using System.Diagnostics.Tracing;
using Thor.Core.Abstractions;

namespace SwissLife.Fusion.Document.EventSources
{
    /// <summary>
    /// Represents the bpm engine component tracing events.
    /// </summary>
    [EventSourceDefinition(Name = "SwissLife-Fusion-Document-Application")]
    public interface IApplicationEventSource
    {
        /// <summary>
        /// This event signals that the application started
        /// </summary>
        [Event(1, Level = EventLevel.Informational,
            Message = "Application started.", Version = 1)]
        void ApplicationStarted();

        /// <summary>
        /// This event signals that the field that logs warnings has been invoked
        /// </summary>
        [Event(2, Level = EventLevel.Warning,
            Message = "The field that logs warnings has been invoked.", Version = 1)]
        void FieldWasInvoked();
    }
}
