using MermaidCharting.Model;
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
        Assert.AreEqual(9, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "2");
        AssertLink(flowchartModel.Links, "1", "4");
        AssertLink(flowchartModel.Links, "2", "3");
        AssertLink(flowchartModel.Links, "3", "6");
        AssertLink(flowchartModel.Links, "4", "5");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
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
        Assert.AreEqual(9, flowchartModel.Links.Count);
        AssertLink(flowchartModel.Links, "startNode", "0");
        AssertLink(flowchartModel.Links, "0", "1");
        AssertLink(flowchartModel.Links, "1", "3", "True");
        AssertLink(flowchartModel.Links, "1", "6", "False");
        AssertLink(flowchartModel.Links, "3", "4");
        AssertLink(flowchartModel.Links, "3", "5");
        AssertLink(flowchartModel.Links, "4", "6");
        AssertLink(flowchartModel.Links, "5", "6");
        AssertLink(flowchartModel.Links, "6", "7");
    }
}