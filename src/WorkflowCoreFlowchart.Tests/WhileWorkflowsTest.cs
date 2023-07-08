using MermaidCharting.Model;
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
        Assert.AreEqual(7, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "4");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "1");
        AssertLink(flowchartModel.Links, "4", "5");
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
        Assert.AreEqual(9, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "6");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "4", "5");
        AssertLink(flowchartModel.Links, "4", "1");
        AssertLink(flowchartModel.Links, "5", "1");
    }

    [TestMethod]
    public void GenerateFlowchartForWhileWorkflowWithoutEndStep()
    {
        WhileWorkflowWithoutEndStep workflow = new();
        WorkflowBuilder<WorkflowData> workflowBuilder = new(Array.Empty<WorkflowStep>());
        workflow.Build(workflowBuilder);
        WorkflowDefinition definition = workflowBuilder.Build("WhileWorkflowWithoutEndStep", 1);

        FlowchartGenerator generator = new();
        FlowchartModel flowchartModel = generator.Generate(definition);

        Assert.AreEqual(5, flowchartModel.Nodes.Count);
        Assert.AreEqual(5, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "1");
    }
}