using MermaidCharting.Model;

namespace WorkflowCoreFlowchart.Tests;

public abstract class WorkflowTestBase
{
    protected void AssertNode(List<NodeModel> nodes, string nodeId, string expectedText, NodeType expectedType)
    {
        NodeModel node = nodes.Single(x => x.Id == nodeId);
        Assert.AreEqual(expectedText, node.Text);
        Assert.AreEqual(expectedType, node.Type);
    }

    protected void AssertLink(List<LinkModel> links, string from, string to, string? title = null)
    {
        LinkModel? link = links.SingleOrDefault(x => x.FromId == from && x.ToId == to);
        Assert.IsNotNull(link);
        Assert.AreEqual(title, link.Title);
    }
}