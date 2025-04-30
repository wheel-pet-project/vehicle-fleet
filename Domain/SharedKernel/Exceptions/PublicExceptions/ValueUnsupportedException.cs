using System.Diagnostics.CodeAnalysis;

namespace Domain.SharedKernel.Exceptions.PublicExceptions;

[ExcludeFromCodeCoverage]
public class ValueUnsupportedException(string publicMessage = "Value is unsupported")
    : PublicException(publicMessage);