using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class WhileWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "WhileWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .While(data => data.Param1 > 10).Do(_ => _ // 1
                .StartWith<StepA>() // 2
                .Then<StepB>()) // 3
            .Then<StepC>() // 4
            .EndWorkflow(); // 5
    }
}