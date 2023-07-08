using MermaidCharting.Model;
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
        Assert.AreEqual(5, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "3");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
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
        Assert.AreEqual(7, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "3");
        AssertLink(flowchartModel.Links, "2", "4");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "4", "5");
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
        Assert.AreEqual(11, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "5");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "3", "5");
        AssertLink(flowchartModel.Links, "4", "7");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
        AssertLink(flowchartModel.Links, "7", "8");
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
        Assert.AreEqual(12, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "5");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "3", "5");
        AssertLink(flowchartModel.Links, "4", "5");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
        AssertLink(flowchartModel.Links, "6", "8");
        AssertLink(flowchartModel.Links, "7", "2");
    }

    [TestMethod]
    public void GenerateFlowchartForGotoWorkflowWithoutEndStep()
    {
        GotoWorkflowWithoutEndStep workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("GotoWorkflowWithoutEndStep", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(5, flowchartModel.Nodes.Count);
        Assert.AreEqual(6, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "2", "1");
        AssertLink(flowchartModel.Links, "3", "0");
    }
}