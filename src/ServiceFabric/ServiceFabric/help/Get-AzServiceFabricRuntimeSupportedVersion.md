---
external help file: Microsoft.Azure.PowerShell.Cmdlets.ServiceFabric.dll-Help.xml
Module Name: Az.ServiceFabric
online version: https://docs.microsoft.com/powershell/module/az.servicefabric/Get-AzServiceFabricRuntimeSupportedVersion
schema: 2.0.0
---

# Get-AzServiceFabricRuntimeSupportedVersion

## SYNOPSIS
Get Service Fabric supported runtime versions. Only supports ARM deployed service fabric clusters.

## SYNTAX

### ByRegion (Default)
```
Get-AzServiceFabricRuntimeSupportedVersion -Region <string> [-Environment {Windows | Linux}] [-DefaultProfile <IAzureContextContainer>] [<CommonParameters>]
```

### ByLatest
```
Get-AzServiceFabricRuntimeSupportedVersion [-Environment {Windows | Linux}] [-Latest] [-Region <string>] [-DefaultProfile <IAzureContextContainer>] [<CommonParameters>]
```

### ByVersion
```
Get-AzServiceFabricRuntimeSupportedVersion [-Environment {Windows | Linux}] [-Region <string>] [-Version <string>] [-DefaultProfile <IAzureContextContainer>] [<CommonParameters>]
```

## DESCRIPTION
Use this cmdlet to get the current service fabric runtime supported versions for ARM deployed clusters.

## EXAMPLES

### Example 1
```powershell
PS C:\> $region = "westus"
PS C:\> Get-AzServiceFabricRuntimeSupportedVersion -Region $region

Environment IsGoalPackage SupportExpiryDate   Version
----------- ------------- -----------------   -------
Windows             False 7/31/2021 00:00:00  7.1.409.9590
Windows             False 7/31/2021 00:00:00  7.1.417.9590
Windows             False 7/31/2021 00:00:00  7.1.428.9590
Windows             False 7/31/2021 00:00:00  7.1.456.9590
Windows             False 7/31/2021 00:00:00  7.1.458.9590
Windows             False 7/31/2021 00:00:00  7.1.459.9590
Windows             False 7/31/2021 00:00:00  7.1.503.9590
Windows             False 7/31/2021 00:00:00  7.1.510.9590
Windows             False 11/30/2021 00:00:00 7.2.413.9590
Windows             False 11/30/2021 00:00:00 7.2.432.9590
Windows             False 11/30/2021 00:00:00 7.2.433.9590
Windows             False 11/30/2021 00:00:00 7.2.445.9590
Windows             False 11/30/2021 00:00:00 7.2.457.9590
Windows             False 11/30/2021 00:00:00 7.2.477.9590
Windows             False 12/31/9999 23:59:59 8.0.514.9590
Windows              True 12/31/9999 23:59:59 8.0.516.9590
```

This example will get the current service fabric runtime supported versions for westus region.

### Example 2
```powershell
PS C:\> Get-AzServiceFabricRuntimeSupportedVersion -Region eastus -Latest

Environment IsGoalPackage SupportExpiryDate   Version
----------- ------------- -----------------   -------
Windows              True 12/31/9999 23:59:59 8.0.516.9590
```

This example will get the latest service fabric runtime supported version for eastus region.

### Example 3
```powershell
PS C:\> Get-AzServiceFabricRuntimeSupportedVersion -Region eastus -Version 8.0.514.9590

Environment IsGoalPackage SupportExpiryDate   Version
----------- ------------- -----------------   -------
Windows             False 12/31/9999 23:59:59 8.0.514.9590
```

This example will get service fabric runtime supported version 8.0.516.9590 for eastus region.

### Example 4
```powershell
PS C:\> Get-AzServiceFabricRuntimeSupportedVersion -Region westeurope -Environment Linux

Environment IsGoalPackage SupportExpiryDate   Version
----------- ------------- -----------------   -------
Ubuntu              False 7/31/2021 00:00:00  7.1.410.1
Ubuntu              False 7/31/2021 00:00:00  7.1.418.1
Ubuntu              False 7/31/2021 00:00:00  7.1.428.1
Ubuntu              False 7/31/2021 00:00:00  7.1.452.1
Ubuntu              False 7/31/2021 00:00:00  7.1.454.1
Ubuntu              False 7/31/2021 00:00:00  7.1.455.1
Ubuntu              False 7/31/2021 00:00:00  7.1.508.1
Ubuntu              False 11/30/2021 00:00:00 7.2.431.1
Ubuntu              False 11/30/2021 00:00:00 7.2.447.1
Ubuntu              False 11/30/2021 00:00:00 7.2.456.1
Ubuntu              False 11/30/2021 00:00:00 7.2.476.1
Ubuntu              False 12/31/9999 23:59:59 8.0.513.1
Ubuntu              False 12/31/9999 23:59:59 8.0.515.1
RedHat              False 12/31/9999 23:59:59 6.1.189.1
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.410.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.418.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.428.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.452.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.454.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.455.1804
Ubuntu18_04         False 7/31/2021 00:00:00  7.1.508.1804
Ubuntu18_04         False 11/30/2021 00:00:00 7.2.431.1804
Ubuntu18_04         False 11/30/2021 00:00:00 7.2.447.1804
Ubuntu18_04         False 11/30/2021 00:00:00 7.2.456.1804
Ubuntu18_04         False 11/30/2021 00:00:00 7.2.476.1804
Ubuntu18_04         False 12/31/9999 23:59:59 8.0.513.1804
Ubuntu18_04          True 12/31/9999 23:59:59 8.0.515.1804
```

This example will get the current service fabric runtime supported Linux versions for westeurope region.

## PARAMETERS

### -Environment
Specify the environment of the cluster.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Latest
Display only the latest supported cluster version.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: ByLatest
Aliases: 

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Region
Specify the name of the Azure region.

```yaml
Type: System.String
Parameter Sets: ByRegion
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Version
Specify supported version of service fabric to enumerate.

```yaml
Type: System.String
Parameter Sets: ByVersion
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### Microsoft.Azure.Commands.ServiceFabric.Models.RuntimePackageDetails

## NOTES

## RELATED LINKS
