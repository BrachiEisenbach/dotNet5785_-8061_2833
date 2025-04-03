using AutoMapper;
using Helpers;

public static class MappingProfile
{
    private static readonly IMapper _mapper;

    static MappingProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DO.Call, BO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToBOType(src.TypeOfCall)))
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // נוותר על הסטטוס כאן
            cfg.CreateMap<BO.Call, DO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToDOType(src.TypeOfCall)));
        });

        _mapper = config.CreateMapper();
    }

    // פונקציה להמיר DO ל-BO ולקבל riskRange כפרמטר
    public static BO.Call ConvertToBO(DO.Call call, TimeSpan riskRange)
    {
        var boCall = _mapper.Map<BO.Call>(call); // המפה את יתר השדות

        // חישוב הסטטוס בנפרד
        boCall.Status = CallManager.CalculateStatus(call, riskRange); // חישוב סטטוס מחוץ למיפוי

        return boCall;
    }

    public static DO.Call ConvertToDO(BO.Call call)
    {
        return _mapper.Map<DO.Call>(call);
    }
}
