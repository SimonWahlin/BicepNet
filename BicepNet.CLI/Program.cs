using BicepNet.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();
BicepWrapper.Initialize(logger);


// See https://aka.ms/new-console-template for more information
var resourceIds = new string[]
{
    "/providers/Microsoft.Management/managementGroups/EsLZ/providers/Microsoft.Authorization/policyDefinitions/Deploy-Windows-DomainJoin",
    //"/providers/Microsoft.Management/managementGroups/EsLZ",
    //"/providers/Microsoft.Management/managementGroups/Development/providers/Microsoft.Authorization/policyDefinitions/606182a1-cd9c-4640-8170-9a7ae58684be",
    //"/subscriptions/b6bd9f9a-a652-456f-9d54-ed3c35d7e492/providers/Microsoft.Authorization/policyDefinitions/4844dd68-5edd-480e-aa27-3c3e664c2538",
    //"/providers/Microsoft.Authorization/policyDefinitions/0015ea4d-51ff-4ce3-8d8c-f3f8f0179a56",
};
//var result = BicepWrapper.ExportResources(resourceIds);
//foreach ((string name, string template) in result.TakeLast(1))
//{
//    Console.WriteLine(name);
//    Console.WriteLine(template);
//}


//var mgId = "/providers/Microsoft.Management/managementGroups/EsLZ";
//var policies = await BicepWrapper.ExportChildResouresAsync(mgId, BicepNet.Core.Azure.ChildResourceType.PolicyDefinitions);
//foreach ((string name, string template) in policies.TakeLast(1))
//{
//    Console.WriteLine(name);
//    Console.WriteLine(template);
//}


var convertId = "/providers/Microsoft.Management/managementGroups/EsLZ/providers/Microsoft.Authorization/policyDefinitions/Append-AppService-httpsonly";
var convertBody = @"{
  ""Name"": ""Append-AppService-httpsonly"",
  ""ResourceId"": ""/providers/Microsoft.Management/managementGroups/EsLZ/providers/Microsoft.Authorization/policyDefinitions/Append-AppService-httpsonly"",
  ""ResourceName"": ""Append-AppService-httpsonly"",
  ""ResourceType"": ""Microsoft.Authorization/policyDefinitions"",
  ""SubscriptionId"": null,
  ""Properties"": {
    ""Description"": ""Appends the AppService sites object to ensure that  HTTPS only is enabled for  server/service authentication and protects data in transit from network layer eavesdropping attacks. Please note Append does not enforce compliance use then deny."",
    ""DisplayName"": ""AppService append enable https only setting to enforce https setting."",
    ""Metadata"": {
      ""version"": ""1.0.0"",
      ""category"": ""App Service"",
      ""createdBy"": ""69615b5e-8b26-430c-ae89-4e626f5ba240"",
      ""createdOn"": ""2022-04-18T14:22:14.4991793Z"",
      ""updatedBy"": null,
      ""updatedOn"": null
    },
    ""Mode"": ""All"",
    ""Parameters"": {
      ""effect"": {
        ""type"": ""String"",
        ""metadata"": {
          ""displayName"": ""Effect"",
          ""description"": ""Enable or disable the execution of the policy""
        },
        ""allowedValues"": [
          ""Append"",
          ""Disabled""
        ],
        ""defaultValue"": ""Append""
      }
    },
    ""PolicyRule"": {
      ""if"": {
        ""allOf"": [
          {
            ""field"": ""type"",
            ""equals"": ""Microsoft.Web/sites""
          },
          {
            ""field"": ""Microsoft.Web/sites/httpsOnly"",
            ""notequals"": true
          }
        ]
      },
      ""then"": {
        ""effect"": ""[parameters('effect')]"",
        ""details"": [
          {
            ""field"": ""Microsoft.Web/sites/httpsOnly"",
            ""value"": true
          }
        ]
      }
    },
    ""PolicyType"": 1
  },
  ""PolicyDefinitionId"": ""/providers/Microsoft.Management/managementGroups/EsLZ/providers/Microsoft.Authorization/policyDefinitions/Append-AppService-httpsonly""
}
";

var result = BicepWrapper.ConvertResourceToBicep(convertId, convertBody);
Console.WriteLine(result);

return 0;
