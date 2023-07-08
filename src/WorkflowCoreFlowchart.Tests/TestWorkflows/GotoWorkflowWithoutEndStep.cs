using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class GotoWorkflowWithoutEndStep : IWorkflow<WorkflowData>
{
    public string Id => "GotoWorkflowWithoutEndStep";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>().Id("label1") // 0
            .Then<StepB>().Id("label2") // 1
            .If(data => data.Param1 > 96).Do(_ => _ // 2
                .StartWith<StepA>() // 3
                .Attach("label1"))
            .Attach("label2");
    }
}