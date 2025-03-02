using System.Xml.Linq;
using Lean.CodeGen.Domain.Entities.Workflow;

namespace Lean.CodeGen.Application.Services.Workflow.Parsers;

/// <summary>
/// BPMN解析器
/// </summary>
public class LeanBpmnParser
{
    /// <summary>
    /// 解析BPMN内容
    /// </summary>
    public LeanWorkflowDefinition Parse(string bpmnContent)
    {
        var doc = XDocument.Parse(bpmnContent);
        var definition = new LeanWorkflowDefinition();

        // 解析流程基本信息
        var process = doc.Root?.Element(XName.Get("process", "http://www.omg.org/spec/BPMN/20100524/MODEL"));
        if (process == null)
        {
            throw new Exception("Invalid BPMN: No process element found");
        }

        definition.WorkflowCode = process.Attribute("id")?.Value ?? "";
        definition.WorkflowName = process.Attribute("name")?.Value ?? "";

        // 解析节点
        foreach (var element in process.Elements())
        {
            var activity = ParseActivity(element);
            if (activity != null)
            {
                definition.Activities.Add(activity);
            }
        }

        // 解析连线
        foreach (var element in process.Elements())
        {
            var flow = ParseFlow(element);
            if (flow != null)
            {
                definition.Flows.Add(flow);
            }
        }

        return definition;
    }

    private LeanWorkflowActivity? ParseActivity(XElement element)
    {
        switch (element.Name.LocalName)
        {
            case "startEvent":
                return new LeanWorkflowActivity
                {
                    ActivityId = element.Attribute("id")?.Value ?? "",
                    ActivityName = element.Attribute("name")?.Value ?? "",
                    ActivityType = "StartEvent"
                };

            case "endEvent":
                return new LeanWorkflowActivity
                {
                    ActivityId = element.Attribute("id")?.Value ?? "",
                    ActivityName = element.Attribute("name")?.Value ?? "",
                    ActivityType = "EndEvent"
                };

            case "userTask":
                return new LeanWorkflowActivity
                {
                    ActivityId = element.Attribute("id")?.Value ?? "",
                    ActivityName = element.Attribute("name")?.Value ?? "",
                    ActivityType = "UserTask"
                };

            case "exclusiveGateway":
                return new LeanWorkflowActivity
                {
                    ActivityId = element.Attribute("id")?.Value ?? "",
                    ActivityName = element.Attribute("name")?.Value ?? "",
                    ActivityType = "ExclusiveGateway"
                };

            default:
                return null;
        }
    }

    private LeanWorkflowFlow? ParseFlow(XElement element)
    {
        if (element.Name.LocalName != "sequenceFlow")
        {
            return null;
        }

        return new LeanWorkflowFlow
        {
            FlowId = element.Attribute("id")?.Value ?? "",
            SourceNodeId = element.Attribute("sourceRef")?.Value ?? "",
            TargetNodeId = element.Attribute("targetRef")?.Value ?? "",
            Condition = element.Element(XName.Get("conditionExpression", "http://www.omg.org/spec/BPMN/20100524/MODEL"))?.Value
        };
    }
}