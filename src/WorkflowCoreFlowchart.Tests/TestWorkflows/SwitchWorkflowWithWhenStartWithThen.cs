using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class SwitchWorkflowWithWhenStartWithThen : IWorkflow<WorkflowData>
{
    public string Id => "SwitchWorkflowWithWhenStartWithThen";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .When(_ => false).Do(_ => _
                .Then<StepA>() // 4
                .Attach("Label"))
            .Then<StepA>() // 5
            .Then<StepB>() // 6
            .Then<StepC>().Id("Label") // 7
            .EndWorkflow(); // 8
    }
}