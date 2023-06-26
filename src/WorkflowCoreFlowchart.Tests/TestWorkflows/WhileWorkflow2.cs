using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class WhileWorkflow2 : IWorkflow<WorkflowData>
{
    public string Id => "WhileWorkflow2";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .While(data => data.Param1 > 10).Do(_ => _ // 1
                .StartWith<StepA>() // 2
                .Then<StepB>() // 3
                .If(data => data.Param1 > 20).Do(__ => __ // 4
                    .StartWith<StepB>())) // 5
            .EndWorkflow(); // 6
    }
}