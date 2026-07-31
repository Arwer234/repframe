using Riok.Mapperly.Abstractions;
using RepFrame.Api.Features.Sets;
using RepFrame.Api.Models;

namespace RepFrame.Api.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class SetMapper
{
    public static partial SetDto ToDto(Set set);

    public static partial Set ToEntity(CreateSetRequest request);
}
