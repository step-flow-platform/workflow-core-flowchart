using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class NestedIfWorkflow2 : IWorkflow<WorkflowData>
{
    public string Id => "NestedIfWorkflow2";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .If(data => data.Param1 > 12).Do(builder1 => builder1 // 2
                .StartWith<StepA>() // 3
                .If(data => data.Param1 > 16).Do(builder2 => builder2 // 4
                    .StartWith<StepA>() // 5
                    .Then<StepB>()) // 6
                .Then<StepA>() // 7
                .Then<StepB>()) // 8
            .Then<StepC>() // 9
            .EndWorkflow(); // 10
    }
}