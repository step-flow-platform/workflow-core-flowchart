using MermaidCharting.Model;
using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class LinearWorkflowsTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForLinearWorkflow1()
    {
        LinearWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("LinearWorkflow1", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(6, flowchartModel.Nodes.Count);
        Assert.AreEqual(5, flowchartModel.Links.Count);
        AssertNode(flowchartModel.Nodes, "startNode", "Start", NodeType.Circle);
        AssertNode(flowchartModel.Nodes, "0", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "1", "StepB", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "2", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "3", "StepC", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "4", "End", NodeType.Circle);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
    }

    [TestMethod]
    public void GenerateFlowchartForLinearWorkflow2()
    {
        LinearWorkflow2 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("LinearWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(8, flowchartModel.Nodes.Count);
        Assert.AreEqual(7, flowchartModel.Links.Count);
        AssertNode(flowchartModel.Nodes, "startNode", "Start", NodeType.Circle);
        AssertNode(flowchartModel.Nodes, "0", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "1", "StepB", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "2", "DoSomething", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "3", "StepC", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "4", "SomeStep", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "5", "Description for step", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "6", "End", NodeType.Circle);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "4", "5");
        AssertLink(flowchartModel.Links, "5", "6");
    }
}