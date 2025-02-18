using System.Diagnostics.CodeAnalysis;

namespace Domain.SharedKernel.Exceptions.ArgumentException;

[ExcludeFromCodeCoverage]
public class ValueOutOfRangeException(string message = "Symbol out of range") : ArgumentException(message);