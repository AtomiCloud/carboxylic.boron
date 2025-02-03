using CarboxylicBoron;
using FluentAssertions;

namespace UnitTest;

public class UtilityTests
{
    private class ToException_Should_ConvertDomainProblemToException_Data
        : TheoryData<IDomainProblem, DomainProblemException>
    {
        public ToException_Should_ConvertDomainProblemToException_Data()
        {
            Add(
                new DummyProblem("test", "param"),
                new DomainProblemException(new DummyProblem("test", "param"))
            );
            Add(
                new DummyProblem("something went wrong", "really wrong"),
                new DomainProblemException(new DummyProblem("something went wrong", "really wrong"))
            );
            Add(
                new DummyProblem("boom!", "name"),
                new DomainProblemException(new DummyProblem("boom!", "name"))
            );
        }
    }

    [Theory]
    [ClassData(typeof(ToException_Should_ConvertDomainProblemToException_Data))]
    public void ToException_Should_ConvertDomainProblemToException(
        IDomainProblem problem,
        DomainProblemException exception
    )
    {
        var actual = problem.ToException();
        actual.Should().BeEquivalentTo(exception);
    }

    private class DomainProblemException_Should_HaveDomainProblem_Data
        : TheoryData<DomainProblemException, IDomainProblem>
    {
        public DomainProblemException_Should_HaveDomainProblem_Data()
        {
            Add(
                new DomainProblemException(new DummyProblem("test", "param")),
                new DummyProblem("test", "param")
            );
            Add(
                new DomainProblemException(
                    new DummyProblem("something went wrong", "really wrong")
                ),
                new DummyProblem("something went wrong", "really wrong")
            );
            Add(
                new DomainProblemException(new DummyProblem("boom!", "name")),
                new DummyProblem("boom!", "name")
            );
        }
    }

    [Theory]
    [ClassData(typeof(DomainProblemException_Should_HaveDomainProblem_Data))]
    public void DomainProblemException_Should_HaveDomainProblem(
        DomainProblemException exception,
        IDomainProblem problem
    )
    {
        var actual = exception.Problem;
        actual.Should().BeEquivalentTo(problem);
    }

    // allow mutation of the problem in problem exceptions
    private class DomainProblemException_Should_HaveMutableDomainProblem_Data
        : TheoryData<DomainProblemException, IDomainProblem, DomainProblemException>
    {
        public DomainProblemException_Should_HaveMutableDomainProblem_Data()
        {
            Add(
                new DomainProblemException(new DummyProblem("test", "param")),
                new DummyProblem("test1", "param1"),
                new DomainProblemException(new DummyProblem("test1", "param1"))
            );
            Add(
                new DomainProblemException(
                    new DummyProblem("something went wrong", "really wrong")
                ),
                new DummyProblem("something went wrong 2", "really wrong 2"),
                new DomainProblemException(
                    new DummyProblem("something went wrong 2", "really wrong 2")
                )
            );
            Add(
                new DomainProblemException(new DummyProblem("boom!", "name")),
                new DummyProblem("boom! 3", "name 3"),
                new DomainProblemException(new DummyProblem("boom! 3", "name 3"))
            );
        }
    }

    [Theory]
    [ClassData(typeof(DomainProblemException_Should_HaveMutableDomainProblem_Data))]
    public void DomainProblemException_Should_HaveMutableDomainProblem(
        DomainProblemException subject,
        IDomainProblem input,
        DomainProblemException expected
    )
    {
        subject.Problem = input;
        subject.Should().BeEquivalentTo(expected);
    }
}
