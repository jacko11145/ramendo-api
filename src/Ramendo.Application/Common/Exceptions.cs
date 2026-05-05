namespace Ramendo.Application.Common;

public class NotFoundException(string name, object key)
    : Exception($"{name} with key '{key}' was not found.");

public class ValidationException(IEnumerable<string> errors)
    : Exception("Validation failed.")
{
    public IReadOnlyList<string> Errors { get; } = errors.ToList();
}

public class UnauthorizedException(string message = "Unauthorized.")
    : Exception(message);

public class ForbiddenException(string message = "Forbidden.")
    : Exception(message);

public class ConflictException(string message)
    : Exception(message);
