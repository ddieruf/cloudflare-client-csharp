using System.Text.Json.Serialization;

namespace CloudflareClient.Models
{
  /// <summary>
  ///
  /// </summary>
  /// <param name="Name">Name of this rule. Only used for human readability.</param>
  /// <param name="Condition">The condition expressions to evaluate. If the condition evaluates to true the overrides or fixed_response in this rule will be applied. An empty condition is always true. For more details on condition expressions please see https://developers.cloudflare.com/load-balancing/understand-basics/load-balancing-rules/expressions</param>
  /// <param name="Overrides">A collection of overrides to apply to the load balancer when this rule's condition is true. All fields are optional.</param>
  /// <param name="Priority">The order in which rules should be executed in relation to each other. Lower values are executed first. Values do not need to be sequential. If no value is provided for any rule the array order of the rules field will be used to assign a priority.</param>
  /// <param name="Disabled">Disable this specific rule. It will no longer be evaluated by this load balancer.</param>
  /// <param name="Terminates">If this rule's condition is true, terminates causes rule evaluation to stop after processing this rule.</param>
  /// <param name="FixedResponse">A collection of fields used to directly respond to the eyeball instead of routing to a pool. If a fixed_response is supplied the rule will be marked as terminates.</param>
  public record Rule (
    string Name,
    string Condition,
    Overrides Overrides,
    long Priority,
    bool Disabled,
    bool Terminates,
    [property: JsonPropertyName("fixed_response")]
    FixedResponse FixedResponse);
}
