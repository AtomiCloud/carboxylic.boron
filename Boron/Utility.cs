namespace CarboxylicBoron;

public static class Utility
{
    /// <summary>
    /// Converts a domain problem to an exception.
    /// </summary>
    /// <param name="p">Problem to convert.</param>
    /// <returns></returns>
    public static DomainProblemException ToException(this IDomainProblem p)
    {
        return new DomainProblemException(p);
    }
}
