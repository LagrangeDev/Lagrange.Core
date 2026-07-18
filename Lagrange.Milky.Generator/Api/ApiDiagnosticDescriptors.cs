using Microsoft.CodeAnalysis;

namespace Lagrange.Milky.Generator.Api;

public class ApiDiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor MustImplementInterfaceError = new(
        id: "LMA001",
        title: "ApiHandler attribute usage error",
        messageFormat: "Type '{0}' marked with ApiHandlerAttribute must implement IApiHandler<TRequest, TResult> or INoRequestApiHandler<TResult> or INoResultApiHandler<TRequest>",
        category: "Lagrange.Milky.Api",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateApiNameError = new(
        id: "LMA002",
        title: "Duplicate api name",
        messageFormat: "Type '{0}' cannot be used as a handler with name '{1}' because this name is already associated with another handler",
        category: "Lagrange.Milky.Api",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}