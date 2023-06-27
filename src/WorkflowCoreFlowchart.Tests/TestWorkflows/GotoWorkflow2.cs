using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class GotoWorkflow2 : IWorkflow<WorkflowData>
{
    public string Id => "GotoWorkflow2";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .If(data => data.Param1 > 96).Do(_ => _ // 1
                .StartWith<StepA>() // 2
                .Attach("label1"))
            .Then<StepB>() // 3
            .Then<StepC>().Id("label1") // 4
            .EndWorkflow(); // 5
    }
}