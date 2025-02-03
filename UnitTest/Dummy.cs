using System.ComponentModel;
using System.Text.Json.Serialization;
using CarboxylicBoron;
using NJsonSchema.Annotations;

namespace UnitTest;

public class DummyProblem() : IDomainProblem
{
    public DummyProblem(string detail, string param)
        : this()
    {
        Detail = detail;
        Param = param;
    }

    [JsonIgnore, JsonSchemaIgnore]
    public string Id { get; } = "dummy_problem";

    [JsonIgnore, JsonSchemaIgnore]
    public string Title { get; } = "Dummy Problem";

    [JsonIgnore, JsonSchemaIgnore]
    public string Detail { get; } = string.Empty;

    [JsonIgnore, JsonSchemaIgnore]
    public string Namespace { get; } = "v1";

    [Description("The parameters that caused the problem")]
    public string Param { get; set; } = string.Empty;
}
