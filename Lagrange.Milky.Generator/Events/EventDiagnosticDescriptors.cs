using Microsoft.CodeAnalysis;

namespace Lagrange.Milky.Generator.Events;

public class EventDiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor MustImplementInterfaceError = new(
        id: "LME001",
        title: "EventSerializer attribute usage error",
        messageFormat: "Type '{0}' marked with EventSerializerAttribute must implement IEventSerializer<TEvent, TData>",
        category: "Lagrange.Milky.Events",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateEventSerializerError = new(
        id: "LME002",
        title: "Duplicate event serializer",
        messageFormat: "Type '{1}' (priority {2}) cannot be used as a serializer for event '{0}' because this event is already associated with another serializer",
        category: "Lagrange.Milky.Events",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}