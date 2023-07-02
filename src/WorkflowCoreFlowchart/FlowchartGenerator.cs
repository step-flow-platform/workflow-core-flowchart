using System.Linq.Expressions;
using System.Reflection;
using MermaidCharting.Model;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace WorkflowCoreFlowchart;

public class FlowchartGenerator
{
    public FlowchartModel Generate(WorkflowDefinition definition)
    {
        Process(definition);
        AddStartNode();
        return _flowchart;
    }

    private void Process(WorkflowDefinition definition)
    {
        Dictionary<string, string> replaceLinks = new();
        foreach (WorkflowStep step in definition.Steps)
        {
            switch (step)
            {
                case EndStep:
                    _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), "End", NodeType.Circle));
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
                    replaceLinks[step.Id.ToString()] = step.Outcomes.Single().NextStep.ToString();
                    continue;
                default:
                    ProcessStep(step);
                    break;
            }
        }

        foreach (LinkModel link in _flowchart.Links.Where(x => replaceLinks.ContainsKey(x.ToId)))
        {
            link.ToId = replaceLinks[link.ToId];
        }
    }

    private void ProcessStep(WorkflowStep step)
    {
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), step.Name));

        if (step.Outcomes.Count == 0)
        {
            _flowchart.Links.Add(new(step.Id.ToString(), _stack.Pop()));
        }
        else
        {
            _flowchart.Links.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString()));
        }
    }

    private void ProcessIfStep(WorkflowStep step)
    {
        string condition = ExtractCondition(step);
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), condition, NodeType.Rhombus));

        switch (step.Outcomes.Count)
        {
            case 0:
                _flowchart.Links.Add(new(step.Id.ToString(), _stack.Peek(), "false"));
                break;
            case 1:
                _flowchart.Links.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString(), "false"));
                _stack.Push(step.Outcomes[0].NextStep.ToString());
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _flowchart.Links.Add(new(step.Id.ToString(), childId.ToString(), "true"));
        }
    }

    private void ProcessWhileStep(WorkflowStep step)
    {
        string condition = ExtractCondition(step);
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), condition, NodeType.Rhombus));

        switch (step.Outcomes.Count)
        {
            case 0:
                _flowchart.Links.Add(new(step.Id.ToString(), _stack.Peek()));
                break;
            case 1:
                _flowchart.Links.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString()));
                _stack.Push(step.Id.ToString());
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _flowchart.Links.Add(new(step.Id.ToString(), childId.ToString()));
        }
    }

    private void ProcessSequence(WorkflowStep step)
    {
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), "Parallel", NodeType.Hexagon));

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
            _flowchart.Links.Add(new(step.Id.ToString(), childId.ToString()));
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
        NodeModel? firstNode = _flowchart.Nodes.FirstOrDefault();
        NodeModel startNode = new NodeModel("startNode", "Start", NodeType.Circle);
        _flowchart.Nodes.Insert(0, startNode);
        if (firstNode is not null)
        {
            LinkModel link = new(startNode.Id, firstNode.Id);
            _flowchart.Links.Insert(0, link);
        }
    }

    private readonly FlowchartModel _flowchart = new();
    private readonly Stack<string> _stack = new();
}