using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Azure.ResourceManager.ManagementGroups.Models;

namespace BicepNet.Core.Azure;

internal static class ManagementGroupHelper
{
    public static async Task<JsonElement> GetManagementGroupAsync(ResourceIdentifier resourceIdentifier, ArmClient armClient, CancellationToken cancellationToken)
    {
        var mg = armClient.GetManagementGroupResource(resourceIdentifier);
        var mgResponse = await mg.GetAsync(cancellationToken: cancellationToken);
        if (mgResponse is null || 
            mgResponse.GetRawResponse().ContentStream is not { } mgContentStream)
        {
            throw new Exception($"Failed to fetch resource from Id '{resourceIdentifier}'");
        }
        mgContentStream.Position = 0;
        return await JsonSerializer.DeserializeAsync<JsonElement>(mgContentStream, cancellationToken: cancellationToken);
    }

    public static async Task<IDictionary<string, JsonElement>> ListManagementGroupPolicyDefinitionsAsync(ResourceIdentifier resourceIdentifier, ArmClient armClient, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, JsonElement>();
        var mg = armClient.GetManagementGroupResource(resourceIdentifier);

        var collection = mg.GetManagementGroupPolicyDefinitions();
        var list = collection.GetAllAsync(filter: "atExactScope()", cancellationToken: cancellationToken);
        
        JsonElement element;

        var taskList = new Dictionary<string, Task<Response<ManagementGroupPolicyDefinitionResource>>>();
        await foreach (var item in list)
        {
            taskList.Add(item.Id.ToString(), item.GetAsync(cancellationToken: cancellationToken));
        }

        foreach (var id in taskList.Keys)
        {
            var policyItemResponse = await taskList[id];
            var resourceId = AzureHelpers.ValidateResourceId(id);
            if (policyItemResponse is null ||
                policyItemResponse.GetRawResponse().ContentStream is not { } contentStream)
            {
                throw new Exception($"Failed to fetch resource from Id '{resourceId.FullyQualifiedId}'");
            }
            contentStream.Position = 0;
            element = await JsonSerializer.DeserializeAsync<JsonElement>(contentStream, cancellationToken: cancellationToken);
            result.Add(id, element);
        }
        return result;
    }

    public static async Task<IDictionary<string, JsonElement>> ListManagementGroupPolicySetDefinitionsAsync(ResourceIdentifier resourceIdentifier, ArmClient armClient, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, JsonElement>();
        var mg = armClient.GetManagementGroupResource(resourceIdentifier);

        var collection = mg.GetManagementGroupPolicySetDefinitions();
        var list = collection.GetAllAsync(filter: "atExactScope()", cancellationToken: cancellationToken);

        JsonElement element;

        var taskList = new Dictionary<string, Task<Response<ManagementGroupPolicySetDefinitionResource>>>();
        await foreach (var item in list)
        {
            taskList.Add(item.Id.ToString(), item.GetAsync(cancellationToken: cancellationToken));
        }

        foreach (var id in taskList.Keys)
        {
            var policyItemResponse = await taskList[id];
            var resourceId = AzureHelpers.ValidateResourceId(id);
            if (policyItemResponse is null ||
                policyItemResponse.GetRawResponse().ContentStream is not { } contentStream)
            {
                throw new Exception($"Failed to fetch resource from Id '{resourceId.FullyQualifiedId}'");
            }
            contentStream.Position = 0;
            element = await JsonSerializer.DeserializeAsync<JsonElement>(contentStream, cancellationToken: cancellationToken);
            result.Add(id, element);
        }
        return result;
    }

    public static async Task<IDictionary<string, JsonElement>> ListManagementDescendantsAsync(ResourceIdentifier resourceIdentifier, ArmClient armClient, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, JsonElement>();
        var mg = armClient.GetManagementGroupResource(resourceIdentifier);

        var collection = mg.GetDescendantsAsync(cancellationToken: cancellationToken);
        var descendants = new List<DescendantData>();
        await foreach (var item in collection)
        {
            descendants.Add(item);
        }

        foreach (var descendant in descendants)
        {
            // return descendant info for now?
        }

        //JsonElement element;

        //var taskList = new Dictionary<string, Task<Response<ManagementGroupPolicySetDefinitionResource>>>();
        //await foreach (var item in list)
        //{
        //    taskList.Add(item.Id.ToString(), item.GetAsync(cancellationToken: cancellationToken));
        //}

        //foreach (var id in taskList.Keys)
        //{
        //    var policyItemResponse = await taskList[id];
        //    var resourceId = AzureHelpers.ValidateResourceId(id);
        //    if (policyItemResponse is null ||
        //        policyItemResponse.GetRawResponse().ContentStream is not { } contentStream)
        //    {
        //        throw new Exception($"Failed to fetch resource from Id '{resourceId.FullyQualifiedId}'");
        //    }
        //    contentStream.Position = 0;
        //    element = await JsonSerializer.DeserializeAsync<JsonElement>(contentStream, cancellationToken: cancellationToken);
        //    result.Add(id, element);
        //}
        return result;
    }

}