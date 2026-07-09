namespace Lagrange.Core.Exceptions;

/// <summary>
/// Thrown when protocol service registrations are invalid or conflict.
/// </summary>
public class ServiceRegistrationException(string message) : LagrangeException(message);
