using System.Text.Json.Serialization;
using NJsonSchema.Annotations;

namespace CarboxylicBoron;

/// <summary>
/// Represents a domain problem that can be encountered during the execution of a command. Supposed to map to the ProblemDetails
/// RFC 7807 specification. See https://datatracker.ietf.org/doc/html/rfc7807
/// </summary>
public interface IDomainProblem
{
    /// <summary>
    /// Gets the unique identifier for the problem.
    /// </summary>
    [JsonIgnore, JsonSchemaIgnore]
    public string Id { get; }

    /// <summary>
    /// The Title of the problem.
    /// </summary>
    [JsonIgnore, JsonSchemaIgnore]
    public string Title { get; }

    /// <summary>
    /// The Status of the problem.
    /// </summary>
    [JsonIgnore, JsonSchemaIgnore]
    public string Detail { get; }

    /// <summary>
    /// The Namespace or version of the problem.
    /// </summary>
    [JsonIgnore, JsonSchemaIgnore]
    public string Namespace { get; }
}
