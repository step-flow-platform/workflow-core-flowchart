using System.Linq.Expressions;
using System.Reflection;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace WorkflowCoreFlowchart;

public class FlowchartGenerator
{
    public FlowchartModel Generate(WorkflowDefinition definition)
    {
        Process(definition);
        AddStartNode();
        return new FlowchartModel(_nodes, _directions);
    }

    private void Process(WorkflowDefinition definition)
    {
        foreach (WorkflowStep step in definition.Steps)
        {
            string? outcomeTitle = null;
            switch (step)
            {
                case EndStep:
                    _nodes.Add(new NodeModel(step.Id.ToString(), "End", NodeType.Circle));
                    continue;
                case WorkflowStep<If>:
                    ProcessIfStep(step);
                    outcomeTitle = "false";
                    break;
                default:
                    _nodes.Add(new NodeModel(step.Id.ToString(), step.Name, NodeType.Default));
                    break;
            }

            if (step.Outcomes.Any())
            {
                foreach (IStepOutcome outcome in step.Outcomes)
                {
                    _directions.Add(new(step.Id.ToString(), outcome.NextStep.ToString(), outcomeTitle));
                }
            }
            else if (_lastTargetNodeId is not null)
            {
                _directions.Add(new(step.Id.ToString(), _lastTargetNodeId, outcomeTitle));
            }

            foreach (int childId in step.Children)
            {
                _directions.Add(new(step.Id.ToString(), childId.ToString(), "true"));
            }
        }
    }

    private void ProcessIfStep(WorkflowStep step)
    {
        Type type = typeof(MemberMapParameter);
        FieldInfo? field = type.GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
        LambdaExpression? value = field?.GetValue(step.Inputs[0]) as LambdaExpression;
        string condition = $"\"{value?.Body.ToString() ?? "-"}\"";
        _nodes.Add(new NodeModel(step.Id.ToString(), condition, NodeType.Rhombus));
        _lastTargetNodeId = step.Outcomes[0].NextStep.ToString();
    }

    /*private void ProcessStepNode(WorkflowNode node)
    {
        WorkflowStepDefinition definition = (WorkflowStepDefinition)node.Definition;
        NodeModel model = new(node.Id, definition.StepType.Name, NodeType.Step);
        _nodes.Add(model);

        switch (node.Directions.Count)
        {
            case 1:
                NodesDirectionModel direction = new(node.Id, node.Directions[0], null);
                _directions.Add(direction);
                break;
            case 0:
                AddEndNodeRelation(node.Id);
                break;
            default:
                throw new ApplicationException("Unexpected workflow STEP directions count.");
        }
    }*/

    /*private void ProcessIfNode(WorkflowNode node)
    {
        WorkflowIfDefinition definition = (WorkflowIfDefinition)node.Definition;
        string condition = $"\"{definition.Condition.Body}\"";
        NodeModel model = new(node.Id, condition, NodeType.If);
        _nodes.Add(model);

        switch (node.Directions.Count)
        {
            case 1:
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[0], "true"));
                AddEndNodeRelation(node.Id, "false");
                break;
            case 2:
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[0], "true"));
                _directions.Add(new NodesDirectionModel(node.Id, node.Directions[1], "false"));
                break;
            default:
                throw new ApplicationException("Unexpected workflow IF directions count.");
        }
    }*/

    private void AddStartNode()
    {
        NodeModel? firstNode = _nodes.FirstOrDefault();
        NodeModel startNode = new NodeModel("startNode", "Start", NodeType.Circle);
        _nodes.Insert(0, startNode);
        if (firstNode is not null)
        {
            NodesDirectionModel direction = new(startNode.Id, firstNode.Id, null);
            _directions.Insert(0, direction);
        }
    }

    private readonly List<NodeModel> _nodes = new();
    private readonly List<NodesDirectionModel> _directions = new();
    private string? _lastTargetNodeId;
}