using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class SwitchWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "SwitchWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .When(_ => 42).Do(_ => _
                .StartWith<StepA>()) // 3
            .When(_ => 21).Do(_ => _
                .StartWith<StepA>() // 5
                .Then<StepB>()) // 6
            .When(_ => 101).Do(_ => _
                .StartWith<StepA>() // 8
                .Then<StepB>() // 9
                .Then<StepC>()) // 10
            .EndWorkflow(); // 11
    }
}