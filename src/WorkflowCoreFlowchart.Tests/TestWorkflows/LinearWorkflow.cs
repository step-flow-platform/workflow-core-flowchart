using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class LinearWorkflow : IWorkflow
{
    public string Id => "LinearWorkflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .StartWith<StepA>()
            .Then<StepB>()
            .Then<StepA>()
            .Then<StepC>()
            .EndWorkflow();
    }
}