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
        GotoWorkflow4 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("0", 1);
        string printedDefinition = WorkflowDefinitionPrinter.Print(definition);

        FlowchartGenerator generator = new();
        FlowchartRenderer renderer = new();
        FlowchartModel flowchartModel = generator.Generate(definition);
        string flowchart = renderer.Render(flowchartModel);

        Assert.IsFalse(string.IsNullOrEmpty(printedDefinition));
        Assert.IsFalse(string.IsNullOrEmpty(flowchart));
    }
}