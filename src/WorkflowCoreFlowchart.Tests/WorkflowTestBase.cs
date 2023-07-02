using MermaidCharting.Model;

namespace WorkflowCoreFlowchart.Tests;

public abstract class WorkflowTestBase
{
    protected void AssertLink(List<LinkModel> links, string from, string to)
    {
        int count = links.Count(x => x.FromId == from && x.ToId == to);
        Assert.AreEqual(1, count);
    }
}