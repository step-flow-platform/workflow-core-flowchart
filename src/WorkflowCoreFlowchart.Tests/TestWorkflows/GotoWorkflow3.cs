using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class GotoWorkflow3 : IWorkflow<WorkflowData>
{
    public string Id => "GotoWorkflow3";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .If(data => data.Param1 > 96).Do(_ => _ // 1
                .StartWith<StepB>() // 2
                .If(data => data.Param1 > 144).Do(__ => __ // 3
                    .StartWith<StepC>() // 4
                    .Attach("label1")))
            .Then<StepA>() // 5
            .Then<StepB>() // 6
            .Then<StepC>().Id("label1") // 7
            .EndWorkflow(); // 8
    }
}