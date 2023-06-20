using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class StepB : IStepBody
{
    public Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}