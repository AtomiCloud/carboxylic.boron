namespace CarboxylicBoron;

/// <summary>
/// Represents an exception that is thrown when a domain problem is encountered.
/// </summary>
/// <param name="problem"></param>
public class DomainProblemException(IDomainProblem problem) : Exception
{
    /// <summary>
    /// Gets the domain problem that was encountered.
    /// </summary>
    public IDomainProblem Problem { get; set; } = problem;
}
