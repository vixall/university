using System.Collections.Generic;
using Xunit;

public class GameTests
{
    public static IEnumerable<object[]> Scenarios()
    {
        yield return new object[]
        {
            new TestCase(
                name: "Scenario 1 - mouse caught at cell 11",
                boardSize: 16,
                inputLines: new[]
                {
                    "16",
                    "M         7",
                    "M        -5",
                    "P",
                    "C         6",
                    "M        -7",
                    "P",
                    "C         6",
                    "P",
                    "M         4",
                    "M         6",
                    "C         0",
                    "P",
                    "M         0",
                    "M         6",
                    "P",
                    "C        -1",
                    "P",
                    "C         1",
                    "C         4",
                    "P",
                    "M        -4",
                    "P"
                },
                expectedMarkers: new[]
                {
                    "Cat and Mouse",
                    "Mouse caught at: 11"
                })
        };

        yield return new object[]
        {
            new TestCase(
                name: "Scenario 2 - mouse evaded",
                boardSize: 20,
                inputLines: new[]
                {
                    "20",
                    "M        19",
                    "M        -4",
                    "P",
                    "M        -1",
                    "P",
                    "M         3",
                    "P",
                    "M         0",
                    "P",
                    "C         3",
                    "M        -6",
                    "P",
                    "M         2",
                    "P",
                    "C         1",
                    "P",
                    "C         6",
                    "M         0",
                    "P",
                    "M         9",
                    "P",
                    "M        -1",
                    "P",
                    "M        -3",
                    "P"
                },
                expectedMarkers: new[]
                {
                    "Cat and Mouse",
                    "Mouse evaded Cat"
                })
        };

        yield return new object[]
        {
            new TestCase(
                name: "Scenario 3 - mouse caught at cell 8",
                boardSize: 27,
                inputLines: new[]
                {
                    "27",
                    "M         6",
                    "M         8",
                    "P",
                    "M         1",
                    "C        22",
                    "C         7",
                    "P",
                    "C       -10",
                    "M         1",
                    "P",
                    "C        11",
                    "P",
                    "C         5",
                    "P",
                    "C        -4",
                    "P",
                    "M         3",
                    "P",
                    "C        -8",
                    "P",
                    "M       -11",
                    "P",
                    "C       -15",
                    "P"
                },
                expectedMarkers: new[]
                {
                    "Cat and Mouse",
                    "Mouse caught at:  8"
                })
        };

        yield return new object[]
        {
            new TestCase(
                name: "Cat steps onto mouse immediately",
                boardSize: 10,
                inputLines: new[]
                {
                    "10",
                    "M         5",
                    "C         5"
                },
                expectedMarkers: new[]
                {
                    "Mouse caught at:  5"
                })
        };

        yield return new object[]
        {
            new TestCase(
                name: "Initial state - players not in game",
                boardSize: 10,
                inputLines: new[]
                {
                    "10",
                    "P"
                },
                expectedMarkers: new[]
                {
                    "Cat and Mouse",
                    "??"
                })
        };
    }

    [Theory]
    [MemberData(nameof(Scenarios))]
    public void Scenario_ProducesExpectedOutput(TestCase tc)
    {
        string output = TestRunner.Run(tc);

        foreach (var marker in tc.ExpectedMarkers)
        {
            Assert.Contains(marker, output);
        }
    }
}
