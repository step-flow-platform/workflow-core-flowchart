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
                case WorkflowStepInline inlineStep:
                    ProcessInlineStepBody(inlineStep);
                    continue;
                default:
                    ProcessStep(step);
                    break;
            }
        }

        foreach (LinkModel link in _flowchart.Links.Where(x => _replaceLinks.ContainsKey(x.ToId)))
        {
            link.ToId = _replaceLinks[link.ToId];
        }
    }

    private void ProcessStep(WorkflowStep step, string? stepText = null)
    {
        string text = stepText ?? GetNodeText(step);
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), text));

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
        string text = GetNodeText(step);
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), text, NodeType.Rhombus));

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
        string text = GetNodeText(step);
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), text, NodeType.Rhombus));

        if (step.Outcomes.Count == 1)
        {
            _flowchart.Links.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString()));
        }
        else
        {
            if (_stack.TryPeek(out string? toId))
            {
                _flowchart.Links.Add(new(step.Id.ToString(), toId));
            }
        }

        _stack.Push(step.Id.ToString());

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

    private void ProcessInlineStepBody(WorkflowStepInline step)
    {
        if (step.Body.Method.Module.Name is "WorkflowCore.dll")
        {
            _replaceLinks[step.Id.ToString()] = step.Outcomes.Single().NextStep.ToString();
        }
        else
        {
            string text = step.Body.Method.Name;
            ProcessStep(step, text);
        }
    }

    private string GetNodeText(WorkflowStep step)
    {
        if (!string.IsNullOrEmpty(step.ExternalId))
        {
            return step.ExternalId;
        }

        if (!string.IsNullOrEmpty(step.Name))
        {
            return step.Name;
        }

        if (step.Inputs.Count > 0)
        {
            Type type = typeof(MemberMapParameter);
            FieldInfo? field = type.GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
            LambdaExpression? value = field?.GetValue(step.Inputs[0]) as LambdaExpression;
            return $"\"{value?.Body.ToString() ?? "-/-"}\"";
        }

        return "---";
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
    private readonly Dictionary<string, string> _replaceLinks = new();
}