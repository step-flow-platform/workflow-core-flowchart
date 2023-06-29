using System.Linq.Expressions;
using System.Reflection;
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
        Dictionary<string, string> replaceDirections = new();
        foreach (WorkflowStep step in definition.Steps)
        {
            switch (step)
            {
                case EndStep:
                    _nodes.Add(new NodeModel(step.Id.ToString(), "End", NodeType.Circle));
                    break;
                case WorkflowStep<If>:
                    ProcessIfStep(step);
                    break;
                case WorkflowStep<While>:
                    ProcessWhileStep(step);
                    break;
                case WorkflowStep<Sequence>:
                    ProcessSequence(step);
                    break;
                case WorkflowStep<InlineStepBody>:
                    replaceDirections[step.Id.ToString()] = step.Outcomes.Single().NextStep.ToString();
                    continue;
                default:
                    ProcessStep(step);
                    break;
            }
        }

        foreach (NodesDirectionModel direction in _directions.Where(x => replaceDirections.ContainsKey(x.ToId)))
        {
            direction.ToId = replaceDirections[direction.ToId];
        }
    }

    private void ProcessStep(WorkflowStep step)
    {
        _nodes.Add(new NodeModel(step.Id.ToString(), step.Name, NodeType.Default));

        if (step.Outcomes.Count == 0)
        {
            _directions.Add(new(step.Id.ToString(), _stack.Pop(), null));
        }
        else
        {
            _directions.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString(), null));
        }
    }

    private void ProcessIfStep(WorkflowStep step)
    {
        string condition = ExtractCondition(step);
        _nodes.Add(new NodeModel(step.Id.ToString(), condition, NodeType.Rhombus));

        switch (step.Outcomes.Count)
        {
            case 0:
                _directions.Add(new(step.Id.ToString(), _stack.Peek(), "false"));
                break;
            case 1:
                _directions.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString(), "false"));
                _stack.Push(step.Outcomes[0].NextStep.ToString());
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _directions.Add(new(step.Id.ToString(), childId.ToString(), "true"));
        }
    }

    private void ProcessWhileStep(WorkflowStep step)
    {
        string condition = ExtractCondition(step);
        _nodes.Add(new NodeModel(step.Id.ToString(), condition, NodeType.Rhombus));

        switch (step.Outcomes.Count)
        {
            case 0:
                _directions.Add(new(step.Id.ToString(), _stack.Peek(), null));
                break;
            case 1:
                _directions.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString(), null));
                _stack.Push(step.Id.ToString());
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _directions.Add(new(step.Id.ToString(), childId.ToString(), null));
        }
    }

    private void ProcessSequence(WorkflowStep step)
    {
        _nodes.Add(new NodeModel(step.Id.ToString(), "Parallel", NodeType.Hexagon));

        string nextStep;
        switch (step.Outcomes.Count)
        {
            case 0:
                nextStep = _stack.Peek();
                break;
            case 1:
                nextStep = step.Outcomes[0].NextStep.ToString();
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _stack.Push(nextStep);
            _directions.Add(new(step.Id.ToString(), childId.ToString(), null));
        }
    }

    private string ExtractCondition(WorkflowStep step)
    {
        Type type = typeof(MemberMapParameter);
        FieldInfo? field = type.GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
        LambdaExpression? value = field?.GetValue(step.Inputs[0]) as LambdaExpression;
        return $"\"{value?.Body.ToString() ?? "-"}\"";
    }

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
    private Stack<string> _stack = new();
}