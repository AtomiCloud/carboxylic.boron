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

    public static int TriangleArea(this int a, int b, int c)
    {
        // Calculate semi-perimeter
        double s = (a + b + c) / 2.0;

        // Use Heron's formula to calculate the area
        double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));

        // Return the area as an integer
        return (int)area;
    }
}
