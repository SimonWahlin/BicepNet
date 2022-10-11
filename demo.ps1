ipmo .\out\BicepNet.PS
$result = Export-BicepNetChildResource -ParentResourceId "/providers/Microsoft.Management/managementGroups/EsLZ" -ResourceType PolicyDefinitions
Remove-Item .\root\* -Force
$result.Keys | % {Out-File -FilePath .\root\$_.bicep -InputObject $result[$_]}
code .\root\