using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class LinearWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "LinearWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .Then<StepA>() // 2
            .Then<StepC>() // 3
            .EndWorkflow(); // 4
    }
}