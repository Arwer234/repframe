## Feature Folder Naming Convention

All files in `Features/<FeatureName>/` start with `<FeatureName>` (e.g., `SetHandler.cs`, not `Handler.cs`).

## REPR Pattern (when features grow)

Split into operation-level subfolders:

```
Features/Sets/
  CreateSet/
    CreateSetCommand.cs
    CreateSetHandler.cs
    Validation/CreateSetRequestValidator.cs
  GetSets/
    GetSetsQuery.cs
    GetSetsResponse.cs
```
