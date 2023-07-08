using MermaidCharting.Model;
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
        SwitchWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("0", 1);
        string printedDefinition = WorkflowDefinitionPrinter.Print(definition);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);
        string flowchart = FlowchartRenderer.Render(flowchartModel);

        Assert.IsFalse(string.IsNullOrEmpty(printedDefinition));
        Assert.IsFalse(string.IsNullOrEmpty(flowchart));
    }
}