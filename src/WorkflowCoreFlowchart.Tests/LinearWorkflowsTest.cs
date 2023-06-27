using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class LinearWorkflowsTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForLinearWorkflow()
    {
        LinearWorkflow workflow = new();
        WorkflowBuilder<object> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("LinearWorkflow", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(6, flowchartModel.Nodes.Count);
        Assert.AreEqual(5, flowchartModel.Directions.Count);
    }
}