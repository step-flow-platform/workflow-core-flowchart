using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class GotoWorkflow1 : IWorkflow
{
    public string Id => "GotoWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<object> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .Attach("label1")
            .Then<StepA>() // 2
            .Then<StepC>().Id("label1") // 3
            .EndWorkflow(); // 4
    }
}