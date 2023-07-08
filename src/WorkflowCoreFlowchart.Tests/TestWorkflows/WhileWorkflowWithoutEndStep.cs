using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class WhileWorkflowWithoutEndStep : IWorkflow<WorkflowData>
{
    public string Id => "WhileWorkflowWithoutEndStep";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .While(_ => true).Do(_ => _ // 1
                .StartWith<StepA>() // 2
                .Then<StepB>()); // 3
    }
}