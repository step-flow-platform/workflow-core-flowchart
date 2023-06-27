namespace WorkflowCoreFlowchart.Tests;

public abstract class WorkflowTestBase
{
    protected void AssertDirection(List<NodesDirectionModel> directions, string from, string to)
    {
        int count = directions.Count(x => x.FromId == from && x.ToId == to);
        Assert.AreEqual(1, count);
    }
}