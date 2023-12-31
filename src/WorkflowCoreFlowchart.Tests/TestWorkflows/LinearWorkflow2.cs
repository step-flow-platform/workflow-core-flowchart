using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class LinearWorkflow2 : IWorkflow<WorkflowData>
{
    public string Id => "LinearWorkflow2";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .Then(DoSomething) // 2
            .Then<StepC>() // 3
            .Then(context => ((WorkflowData)context.Workflow.Data).Param1 = 21).Id("SomeStep") // 4
            .Then<StepA>().Id("Description for step") // 5
            .EndWorkflow(); // 6
    }

    private ExecutionResult DoSomething(IStepExecutionContext context)
    {
        ((WorkflowData)context.Workflow.Data).Param1 = 42;
        return ExecutionResult.Next();
    }
}