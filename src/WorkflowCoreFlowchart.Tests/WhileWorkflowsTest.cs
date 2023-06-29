using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class WhileWorkflowsTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForWhileWorkflow1()
    {
        WhileWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("WhileWorkflow1", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(7, flowchartModel.Nodes.Count);
        Assert.AreEqual(7, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "4");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "1");
        AssertDirection(flowchartModel.Directions, "4", "5");
    }

    [TestMethod]
    public void GenerateFlowchartForWhileWorkflow2()
    {
        WhileWorkflow2 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("WhileWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(8, flowchartModel.Nodes.Count);
        Assert.AreEqual(9, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "6");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "4", "5");
        AssertDirection(flowchartModel.Directions, "4", "1");
        AssertDirection(flowchartModel.Directions, "5", "1");
    }
}