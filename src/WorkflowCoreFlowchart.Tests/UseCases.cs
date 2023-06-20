using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class UseCases
{
    [TestMethod]
    public void GenerateAndRenderFlowchart()
    {
        LinearWorkflow workflow = new();
        WorkflowBuilder<object> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("LinearWorkflow", 1);

        FlowchartGenerator generator = new();
        FlowchartRenderer renderer = new();
        FlowchartModel flowchartModel = generator.Generate(definition);
        string flowchart = renderer.Render(flowchartModel);

        Assert.IsFalse(string.IsNullOrEmpty(flowchart));
    }
}