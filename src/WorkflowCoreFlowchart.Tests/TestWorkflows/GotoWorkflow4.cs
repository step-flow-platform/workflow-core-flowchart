using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class GotoWorkflow4 : IWorkflow<WorkflowData>
{
    public string Id => "GotoWorkflow3";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .If(data => data.Param1 > 96).Do(_ => _ // 1
                .StartWith<StepB>().Id("label1") // 2
                .If(data => data.Param1 > 144).Do(__ => __ // 3
                    .StartWith<StepC>())) // 4
            .Then<StepA>() // 5
            .If(data => data.Param1 > 962).Do(_ => _ // 6
                .StartWith<StepB>() // 7
                .Attach("label1"))
            .EndWorkflow(); // 8
    }
}