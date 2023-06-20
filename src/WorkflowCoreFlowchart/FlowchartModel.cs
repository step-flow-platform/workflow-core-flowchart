namespace WorkflowCoreFlowchart;

public record FlowchartModel(
    List<NodeModel> Nodes,
    List<NodesDirectionModel> Directions);