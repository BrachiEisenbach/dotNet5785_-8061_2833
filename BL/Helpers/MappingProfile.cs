using AutoMapper;
using DalApi;
using Helpers;

public static class MappingProfile
{
    private static readonly IMapper _mapper;
    private static readonly IDal s_dal = Factory.Get;
    static MappingProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            // המיפוי בין DO.Call ל-BO.Call
            cfg.CreateMap<DO.Call, BO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToBOType(src.TypeOfCall)))
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // נוותר על הסטטוס כאן

            // המיפוי בין BO.Call ל-DO.Call
            cfg.CreateMap<BO.Call, DO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToDOType(src.TypeOfCall)));

            // המיפוי בין DO.Volunteer ל-BO.Volunteer
            cfg.CreateMap<DO.Volunteer, BO.Volunteer>()
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => VolunteerManager.ConvertToBORole(src.Role)))
                 .ForMember(dest => dest.TypeOfDistance, opt => opt.MapFrom(src => VolunteerManager.ConvertToBOType(src.TypeOfDistance)))
                 .ForMember(dest => dest.AllCallsThatTreated, opt => opt.MapFrom(src => VolunteerManager.GetAllCallsThatTreated(src.Id)))
                 .ForMember(dest => dest.AllCallsThatCanceled, opt => opt.MapFrom(src => VolunteerManager.GetAllCallsThatCanceled(src.Id)))
                 .ForMember(dest => dest.AllCallsThatHaveExpired, opt => opt.MapFrom(src => VolunteerManager.GetAllCallsThatHaveExpired(src.Id)))
                 .ForMember(dest => dest.CallInTreate, opt => opt.MapFrom(src => VolunteerManager.GetCallInTreatment(src.Id)));

            // המיפוי בין BO.Volunteer ל-DO.Volunteer
            cfg.CreateMap<BO.Volunteer, DO.Volunteer>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => VolunteerManager.ConvertToDORole(src.Role)))
                .ForMember(dest => dest.TypeOfDistance, opt => opt.MapFrom(src => VolunteerManager.ConvertToDOType(src.TypeOfDistance)));
        });

        _mapper = config.CreateMapper();
    }

    // פונקציה להמיר DO ל-BO ולקבל riskRange כפרמטר
    public static BO.Call ConvertToBO(DO.Call call, TimeSpan riskRange)
    {
        var boCall = _mapper.Map<BO.Call>(call); // המפר את יתר השדות

        // חישוב הסטטוס בנפרד
        boCall.Status = CallManager.CalculateStatus(call, riskRange); // חישוב סטטוס מחוץ למיפוי

        return boCall;
    }

    public static DO.Call ConvertToDO(BO.Call call)
    {
        return _mapper.Map<DO.Call>(call);
    }

    // פונקציה להמיר DO ל-BO של מתנדב
    public static BO.Volunteer ConvertToBO(DO.Volunteer volunteer)
    {
        return _mapper.Map<BO.Volunteer>(volunteer);
    }

    // פונקציה להמיר BO ל-DO של מתנדב
    public static DO.Volunteer ConvertToDO(BO.Volunteer volunteer)
    {
        return _mapper.Map<DO.Volunteer>(volunteer);
    }
}
