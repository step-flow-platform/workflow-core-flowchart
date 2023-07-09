using System.Linq.Expressions;
using System.Reflection;
using MermaidCharting.Model;
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
                case WorkflowStep<OutcomeSwitch>:
                    ProcessSwitchStep(step);
                    break;
                case WorkflowStep<When>:
                    ProcessWhenStep(step);
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
            Tuple<string, string?> values = _replaceLinks[link.ToId];
            link.ToId = values.Item1;
            if (values.Item2 is not null)
            {
                link.Title = values.Item2;
            }
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
                _flowchart.Links.Add(new(step.Id.ToString(), _stack.Peek(), "False"));
                break;
            case 1:
                _flowchart.Links.Add(new(step.Id.ToString(), step.Outcomes[0].NextStep.ToString(), "False"));
                _stack.Push(step.Outcomes[0].NextStep.ToString());
                break;
            default:
                throw new ApplicationException("Unexpected step outcomes");
        }

        foreach (int childId in step.Children)
        {
            _flowchart.Links.Add(new(step.Id.ToString(), childId.ToString(), "True"));
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

    private void ProcessSwitchStep(WorkflowStep step)
    {
        _flowchart.Nodes.Add(new NodeModel(step.Id.ToString(), "Switch", NodeType.Rhombus));

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

        _flowchart.Links.Add(new(step.Id.ToString(), nextStep, "Default"));

        foreach (int childId in step.Children)
        {
            _stack.Push(nextStep);
            _flowchart.Links.Add(new(step.Id.ToString(), childId.ToString()));
        }
    }

    private void ProcessWhenStep(WorkflowStep step)
    {
        string linkText = ExtractInputText(step.Inputs[0]);
        _replaceLinks[step.Id.ToString()] = new(step.Children.Single().ToString(), linkText);
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
            _replaceLinks[step.Id.ToString()] = new(step.Outcomes.Single().NextStep.ToString(), null);
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
            return ExtractInputText(step.Inputs[0]);
        }

        return "---";
    }

    private string ExtractInputText(IStepParameter input)
    {
        Type type = typeof(MemberMapParameter);
        FieldInfo? field = type.GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
        LambdaExpression? value = field?.GetValue(input) as LambdaExpression;
        if (value?.Body is UnaryExpression unaryExpression)
        {
            return unaryExpression.Operand.ToString();
        }

        return $"\"{value?.Body?.ToString() ?? "-/-"}\"";
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
    private readonly Dictionary<string, Tuple<string, string?>> _replaceLinks = new();
}