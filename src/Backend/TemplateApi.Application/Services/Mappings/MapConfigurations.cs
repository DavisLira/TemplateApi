using Mapster;
using TemplateApi.Communication.Requests;

namespace TemplateApi.Application.Services.Mappings;

public class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, Domain.Entities.User>
            .NewConfig()
            .Ignore(dest => dest.Password);
    }
}