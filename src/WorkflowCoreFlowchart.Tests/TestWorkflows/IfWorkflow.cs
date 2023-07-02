using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class IfWorkflow : IWorkflow<WorkflowData>
{
    public string Id => "IfWorkflow";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .If(data => data.Param1 > 12).Do(_ => _ // 2
                .StartWith<StepA>() // 3
                .Then<StepC>()) // 4
            .Then<StepB>() // 5
            .EndWorkflow(); // 6
    }
}