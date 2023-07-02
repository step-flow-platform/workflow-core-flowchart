using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class NestedIfWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "NestedIfWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Then<StepB>() // 1
            .If(data => data.Param1 > 12).Do(builder1 => builder1 // 2
                .StartWith<StepA>() // 3
                .If(data => data.Param1 > 16).Do(builder2 => builder2 // 4
                    .StartWith<StepB>() // 5
                    .Then<StepC>() // 6
                    .If(data => data.Param1 < 18).Do(builder3 => builder3 // 7
                        .StartWith<StepA>()))) // 8
            .Then<StepB>() // 9
            .EndWorkflow(); // 10
    }
}