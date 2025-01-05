using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ballastlane.Integration.Tests.Order;

internal class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        return testCases.OrderBy(tc =>
        {
            var priorityAttribute = tc.TestMethod.Method.GetCustomAttributes(typeof(PriorityAttribute))
                .FirstOrDefault();

            return priorityAttribute?.GetNamedArgument<int>("Priority") ?? int.MaxValue;
        });
    }
}
