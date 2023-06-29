using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class ParallelWorkflow2 : IWorkflow<WorkflowData>
{
    public string Id => "ParallelWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .If(data => data.Param1 == 42).Do(_ => _ // 1
                .Parallel() // 3
                .Do(__ => __
                    .StartWith<StepA>()) // 4
                .Do(__ => __
                    .StartWith<StepB>()) // 5
                .Join())
            .Then<StepC>() // 6
            .EndWorkflow(); // 7
    }
}