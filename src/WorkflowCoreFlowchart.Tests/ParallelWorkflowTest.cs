using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class ParallelWorkflowTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForNestedIfWorkflow1()
    {
        ParallelWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("ParallelWorkflow1", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(9, flowchartModel.Nodes.Count);
        Assert.AreEqual(9, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "2");
        AssertDirection(flowchartModel.Directions, "1", "4");
        AssertDirection(flowchartModel.Directions, "2", "3");
        AssertDirection(flowchartModel.Directions, "3", "6");
        AssertDirection(flowchartModel.Directions, "4", "5");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
    }

    [TestMethod]
    public void GenerateFlowchartForNestedIfWorkflow2()
    {
        ParallelWorkflow2 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("ParallelWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(8, flowchartModel.Nodes.Count);
        Assert.AreEqual(9, flowchartModel.Directions.Count);
        AssertDirection(flowchartModel.Directions, "startNode", "0");
        AssertDirection(flowchartModel.Directions, "0", "1");
        AssertDirection(flowchartModel.Directions, "1", "3");
        AssertDirection(flowchartModel.Directions, "1", "6");
        AssertDirection(flowchartModel.Directions, "3", "4");
        AssertDirection(flowchartModel.Directions, "3", "5");
        AssertDirection(flowchartModel.Directions, "4", "6");
        AssertDirection(flowchartModel.Directions, "5", "6");
        AssertDirection(flowchartModel.Directions, "6", "7");
    }
}