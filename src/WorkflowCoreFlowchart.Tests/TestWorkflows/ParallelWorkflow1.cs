using WorkflowCore.Interface;

namespace WorkflowCoreFlowchart.Tests.TestWorkflows;

public class ParallelWorkflow1 : IWorkflow<WorkflowData>
{
    public string Id => "ParallelWorkflow1";

    public int Version => 1;

    public void Build(IWorkflowBuilder<WorkflowData> builder)
    {
        builder
            .StartWith<StepA>() // 0
            .Parallel() // 1
            .Do(_ => _
                .StartWith<StepA>() // 2
                .Then<StepB>()) // 3
            .Do(_ => _
                .StartWith<StepB>() // 4
                .Then<StepC>()) // 5
            .Join()
            .Then<StepC>() // 6
            .EndWorkflow(); // 7
    }
}