using MermaidCharting;
using MermaidCharting.Model;

namespace WorkflowCoreFlowchart;

public static class FlowchartRenderer
{
    public static string Render(FlowchartModel flowchart)
    {
        return MermaidRenderer.RenderFlowchart(flowchart);
    }
}