using MermaidCharting.Model;
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
        Assert.AreEqual(8, flowchartModel.Links.Count);
        AssertNode(flowchartModel.Nodes, "2", @"""(data.Param1 > 12)""", NodeType.Rhombus);
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
        Assert.AreEqual(14, flowchartModel.Links.Count);
        AssertNode(flowchartModel.Nodes, "2", @"""(data.Param1 > 12)""", NodeType.Rhombus);
        AssertNode(flowchartModel.Nodes, "4", @"""(data.Param1 > 16)""", NodeType.Rhombus);
        AssertNode(flowchartModel.Nodes, "7", @"""(data.Param1 < 18)""", NodeType.Rhombus);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3", "True");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "4", "5", "True");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
        AssertLink(flowchartModel.Links, "7", "8", "True");
        AssertLink(flowchartModel.Links, "8", "9");
        AssertLink(flowchartModel.Links, "9", "10");
        AssertLink(flowchartModel.Links, "2", "9", "False");
        AssertLink(flowchartModel.Links, "4", "9", "False");
        AssertLink(flowchartModel.Links, "7", "9", "False");
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
        Assert.AreEqual(13, flowchartModel.Links.Count);
        AssertNode(flowchartModel.Nodes, "2", @"""(data.Param1 > 12)""", NodeType.Rhombus);
        AssertNode(flowchartModel.Nodes, "4", @"""(data.Param1 > 16)""", NodeType.Rhombus);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3", "True");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "4", "5", "True");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
        AssertLink(flowchartModel.Links, "7", "8");
        AssertLink(flowchartModel.Links, "8", "9");
        AssertLink(flowchartModel.Links, "9", "10");
        AssertLink(flowchartModel.Links, "2", "9", "False");
        AssertLink(flowchartModel.Links, "4", "7", "False");
    }
}