using System.Diagnostics.CodeAnalysis;

namespace Lagrange.Core.Common.Entity;

public class OperationResult<T>
{
    public int Retcode { get; init; }

    public string? Message { get; init; }

    public T? Data { get; init; }

    [MemberNotNullWhen(true, nameof(Data))]
    [MemberNotNullWhen(false, nameof(Message))]
    public bool IsSuccess => Retcode == 0;
}