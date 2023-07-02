using WorkflowCore.Interface;

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
            .Then(context => ((WorkflowData)context.Workflow.Data).Param1 = 42) // 2
            .Then<StepC>() // 3
            .EndWorkflow(); // 4
    }
}