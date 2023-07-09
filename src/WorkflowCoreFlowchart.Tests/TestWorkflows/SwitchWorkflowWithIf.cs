using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class SwitchWorkflowWithIf : IWorkflow<WorkflowData>
{
    public string Id => "SwitchWorkflowWithIf";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .If(_ => true).Do(_ => _ // 1
                .StartWith<StepB>() // 2
                .When(_ => 42).Do(__ => __
                    .StartWith<StepC>())) // 5
            .EndWorkflow(); // 6
    }
}