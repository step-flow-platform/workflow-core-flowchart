using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreFlowchart.Tests;

public static class WorkflowDefinitionPrinter
{
    public static string Print(WorkflowDefinition definition)
    {
        StringBuilder builder = new StringBuilder();
        foreach (WorkflowStep step in definition.Steps)
        {
            builder.AppendLine($"{step.Id} | {step.Name ?? step.BodyType?.ToString() ?? step.GetType().ToString()}");

            builder.Append("Outcomes: ");
            foreach (IStepOutcome outcome in step.Outcomes)
            {
                builder.Append($"[{outcome.NextStep}]");
            }

            builder.AppendLine();
            builder.Append("Children: ");
            foreach (int child in step.Children)
            {
                builder.Append($"[{child}]");
            }

            builder.AppendLine();
            builder.AppendLine();
        }

        return builder.ToString();
    }
}