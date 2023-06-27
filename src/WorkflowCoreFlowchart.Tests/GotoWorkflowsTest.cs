using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class GotoWorkflowsTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForGotoWorkflow1()
    {
        GotoWorkflow1 workflow = new();
        WorkflowBuilder<object> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("GotoWorkflow1", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(6, flowchartModel.Nodes.Count);
        Assert.AreEqual(5, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "3");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
    }

    [TestMethod]
    public void GenerateFlowchartForGotoWorkflow2()
    {
        GotoWorkflow2 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("GotoWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(7, flowchartModel.Nodes.Count);
        Assert.AreEqual(7, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "3");
        AssertDirection(flowchartModel.Directions, "2", "4");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "4", "5");
    }

    [TestMethod]
    public void GenerateFlowchartForGotoWorkflow3()
    {
        GotoWorkflow3 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("GotoWorkflow3", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(10, flowchartModel.Nodes.Count);
        Assert.AreEqual(11, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "5");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "3", "5");
        AssertDirection(flowchartModel.Directions, "4", "7");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
        AssertDirection(flowchartModel.Directions, "7", "8");
    }

    [TestMethod]
    public void GenerateFlowchartForGotoWorkflow4()
    {
        GotoWorkflow4 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("GotoWorkflow4", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(10, flowchartModel.Nodes.Count);
        Assert.AreEqual(12, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "5");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "3", "5");
        AssertDirection(flowchartModel.Directions, "4", "5");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
        AssertDirection(flowchartModel.Directions, "6", "8");
        AssertDirection(flowchartModel.Directions, "7", "2");
    }
}