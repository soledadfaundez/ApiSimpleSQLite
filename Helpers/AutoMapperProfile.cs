namespace Customer.Management.Api.Helpers;

using AutoMapper;
using Customer.Management.Api.Entities;
using Customer.Management.Api.Models.Customer;
using Customer.Management.Api.Models.User;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // ESFC: Mapeo CreateRequest -> Customer
        CreateMap<CreateRequest, Customer>();

        // ESFC: Mapeo UpdateRequest -> User
        CreateMap<UpdateRequest, Customer>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Type" && src.Type == null) return false;

                    return true;
                }
            ));

        // ESFC: Mapeo Register -> UserApi
        CreateMap<RegisterModel, UserApi>();
    }
}