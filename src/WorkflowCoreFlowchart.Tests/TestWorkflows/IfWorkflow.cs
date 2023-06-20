using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class IfWorkflow : IWorkflow<WorkflowData>
{
    public string Id => "IfWorkflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>()
            .Then<StepB>()
            .If(data => data.Param1 > 12).Do(_ => _
                .StartWith<StepA>()
                .Then<StepC>())
            .Then<StepB>()
            .EndWorkflow();
    }
}