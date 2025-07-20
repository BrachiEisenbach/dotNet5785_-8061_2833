using AutoMapper;
using BO;
using DalApi;
using Helpers;
using System.Diagnostics;

public static class MappingProfile
{
    private static readonly IMapper _mapper;
    /// <summary>
    /// Static constructor that initializes the AutoMapper configuration.
    /// מגדיר את המיפוי בין מחלקות Data Object (DO) ל-Business Object (BO) ולהפך.
    /// </summary>
    static MappingProfile()
    {
        try
        {
            var config = new MapperConfiguration(cfg =>
        {
            // המיפוי בין DO.Call ל-BO.Call
            cfg.CreateMap<DO.Call, BO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToBOType(src.TypeOfCall)))
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // נוותר על הסטטוס כאן
                .ForMember(dest => dest.listOfCallAssign, opt => opt.Ignore());

            // המיפוי בין BO.Call ל-DO.Call
            cfg.CreateMap<BO.Call, DO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => CallManager.ConvertToDOType(src.TypeOfCall)));

            // המיפוי בין DO.Volunteer ל-BO.Volunteer
            cfg.CreateMap<DO.Volunteer, BO.Volunteer>()
                 .ForMember(dest => dest.Role, opt => opt.MapFrom(src => VolunteerManager.ConvertToBORole(src.Role)))
                 .ForMember(dest => dest.TypeOfDistance, opt => opt.MapFrom(src => VolunteerManager.ConvertToBOType(src.TypeOfDistance)))
                 .ForMember(dest => dest.AllCallsThatTreated, opt => opt.Ignore())
                 .ForMember(dest => dest.AllCallsThatCanceled, opt => opt.Ignore())
                 .ForMember(dest => dest.AllCallsThatHaveExpired, opt => opt.Ignore())
                 .ForMember(dest => dest.CallInTreate, opt => opt.Ignore());
                 
            // המיפוי בין BO.Volunteer ל-DO.Volunteer
            cfg.CreateMap<BO.Volunteer, DO.Volunteer>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => VolunteerManager.ConvertToDORole(src.Role)))
                .ForMember(dest => dest.TypeOfDistance, opt => opt.MapFrom(src => VolunteerManager.ConvertToDOType(src.TypeOfDistance)));
        });

            _mapper = config.CreateMapper();
            Debug.WriteLine("Mapper created!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("AutoMapper static ctor error: " + ex);
            throw;
        }
    }

    // פונקציה המקבלת  riskRange  כפרמטר ואת מערך ההקצאות לקריאה וממירה את יישות קריאה DO ל-BO 
    public static BO.Call ConvertToBO(DO.Call call, List<CallAssignInList> callAssignments)
    {
        var boCall = _mapper.Map<BO.Call>(call); // המפר את יתר השדות

        // חישוב הסטטוס בנפרד
        boCall.Status = CallManager.CalculateStatus(call); // חישוב סטטוס מחוץ למיפוי
        boCall.listOfCallAssign= callAssignments;
        return boCall;
    }

    public static DO.Call ConvertToDO(BO.Call call)
    {
        return _mapper.Map<DO.Call>(call);
    }

    // פונקציה להמיר DO ל-BO של מתנדב
    public static BO.Volunteer ConvertToBO(DO.Volunteer volunteer)
    {
        try
        {
            return _mapper.Map<BO.Volunteer>(volunteer);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("❌ Mapping failed: " + ex);
            throw;
        }
    }

    // פונקציה להמיר BO ל-DO של מתנדב
    public static DO.Volunteer ConvertToDO(BO.Volunteer volunteer)
    {
        return _mapper.Map<DO.Volunteer>(volunteer);
    }
}
