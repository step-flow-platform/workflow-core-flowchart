using System.Text;

namespace WorkflowCoreFlowchart;

public class FlowchartRenderer
{
    public string Render(FlowchartModel model)
    {
        StringBuilder builder = new();
        AddHeader(builder);
        RenderNodes(builder, model);
        builder.AppendLine();
        RenderDirections(builder, model);
        AddFooter(builder);
        return builder.ToString();
    }

    private void RenderNodes(StringBuilder builder, FlowchartModel model)
    {
        foreach (NodeModel node in model.Nodes)
        {
            switch (node.Type)
            {
                case NodeType.Default:
                    builder.AppendLine($"{node.Id}[{node.Text}]");
                    break;
                case NodeType.Rhombus:
                    builder.AppendLine($"{node.Id}{{{node.Text}}}");
                    break;
                case NodeType.Circle:
                    builder.AppendLine($"{node.Id}(({node.Text}))");
                    break;
                case NodeType.Subroutine:
                    builder.AppendLine($"{node.Id}[[{node.Text}]]");
                    break;
                case NodeType.Hexagon:
                    builder.AppendLine($"{node.Id}{{{{{node.Text}}}}}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void RenderDirections(StringBuilder builder, FlowchartModel model)
    {
        foreach (NodesDirectionModel relation in model.Directions)
        {
            string title = relation.Title != null ? $"|{relation.Title}|" : string.Empty;
            builder.AppendLine($"{relation.FromId} -->{title} {relation.ToId}");
        }
    }

    private void AddHeader(StringBuilder builder)
    {
        builder.AppendLine("```mermaid");
        builder.AppendLine("flowchart TB");
    }

    private void AddFooter(StringBuilder builder)
    {
        builder.AppendLine("```");
    }
}