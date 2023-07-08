using MermaidCharting.Model;
using WorkflowCore.Models;
using WorkflowCore.Services;
using WorkflowCoreFlowchart.Tests.TestWorkflows;

namespace WorkflowCoreFlowchart.Tests;

[TestClass]
public class SwitchWorkflowTest : WorkflowTestBase
{
    [TestMethod]
    public void GenerateFlowchartForSwitchWorkflow1()
    {
        SwitchWorkflow1 workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("LinearWorkflow2", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(10, flowchartModel.Nodes.Count);
        Assert.AreEqual(11, flowchartModel.Links.Count);

        AssertNode(flowchartModel.Nodes, "startNode", "Start", NodeType.Circle);
        AssertNode(flowchartModel.Nodes, "0", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "1", "Switch", NodeType.Rhombus);
        AssertNode(flowchartModel.Nodes, "3", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "5", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "6", "StepB", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "8", "StepA", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "9", "StepB", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "10", "StepC", NodeType.Default);
        AssertNode(flowchartModel.Nodes, "11", "End", NodeType.Circle);

        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "3", "42");
        AssertLink(flowchartModel.Links, "1", "5", "21");
        AssertLink(flowchartModel.Links, "1", "8", "101");
        AssertLink(flowchartModel.Links, "3", "11");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "11");
        AssertLink(flowchartModel.Links, "8", "9");
        AssertLink(flowchartModel.Links, "9", "10");
        AssertLink(flowchartModel.Links, "10", "11");
    }
}