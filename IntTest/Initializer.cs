using CarboxylicLithiumTestHelper;
using FluentAssertions;
using FluentAssertions.Extensibility;
using IntTest;

[assembly: AssertionEngineInitializer(typeof(Initializer), nameof(Initializer.Initialize))]

namespace IntTest;

public static class Initializer
{
    public static void Initialize()
    {
        AssertionConfiguration.Current.Equivalency.Modify(o =>
            o.Using(new ResultEquivalencyStep())
        );
    }
}
