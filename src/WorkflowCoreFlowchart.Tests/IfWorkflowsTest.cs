using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class IfWorkflowsTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForIfWorkflow()
    {
        IfWorkflow workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("IfWorkflow", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(8, flowchartModel.Nodes.Count);
        Assert.AreEqual(8, flowchartModel.Directions.Count);
    }

    [TestMethod]
    public void GenerateFlowchartForNestedIfWorkflow1()
    {
        NestedIfWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("NestedIfWorkflow1", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(12, flowchartModel.Nodes.Count);
        Assert.AreEqual(14, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "4", "5");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
        AssertDirection(flowchartModel.Directions, "7", "8");
        AssertDirection(flowchartModel.Directions, "8", "9");
        AssertDirection(flowchartModel.Directions, "9", "10");
        AssertDirection(flowchartModel.Directions, "2", "9");
        AssertDirection(flowchartModel.Directions, "4", "9");
        AssertDirection(flowchartModel.Directions, "7", "9");
    }

    [TestMethod]
    public void GenerateFlowchartForNestedIfWorkflow2()
    {
        NestedIfWorkflow2 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("NestedIfWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(12, flowchartModel.Nodes.Count);
        Assert.AreEqual(13, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "4", "5");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
        AssertDirection(flowchartModel.Directions, "7", "8");
        AssertDirection(flowchartModel.Directions, "8", "9");
        AssertDirection(flowchartModel.Directions, "9", "10");
        AssertDirection(flowchartModel.Directions, "2", "9");
        AssertDirection(flowchartModel.Directions, "4", "7");
    }
}