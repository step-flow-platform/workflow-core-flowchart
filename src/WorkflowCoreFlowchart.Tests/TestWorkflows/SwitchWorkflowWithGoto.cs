using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class SwitchWorkflowWithGoto : IWorkflow<WorkflowData>
{
    public string Id => "SwitchWorkflowWithGoto";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .When(_ => false).Do(_ => _
                .StartWith<StepA>() // 3
                .Attach("Label"))
            .Then<StepA>() // 4
            .Then<StepB>() // 5
            .Then<StepC>().Id("Label") // 6
            .EndWorkflow(); // 7
    }
}