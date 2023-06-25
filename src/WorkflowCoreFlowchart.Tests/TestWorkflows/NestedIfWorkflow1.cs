using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class NestedIfWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "NestedIfWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>()
            .Then<StepB>()
            .If(data => data.Param1 > 12).Do(builder1 => builder1
                .StartWith<StepA>()
                .If(data => data.Param1 > 16).Do(builder2 => builder2
                    .StartWith<StepB>()
                    .Then<StepC>()
                    .If(data => data.Param1 < 18).Do(builder3 => builder3
                        .StartWith<StepA>())))
            .Then<StepB>()
            .EndWorkflow();
    }
}