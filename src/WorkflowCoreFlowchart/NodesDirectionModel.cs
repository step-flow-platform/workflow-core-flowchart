namespace WorkflowCoreFlowchart;

public record NodesDirectionModel(
    string FromId,
    string ToId,
    string? Title)
{
    public string ToId { get; set; } = ToId;
}