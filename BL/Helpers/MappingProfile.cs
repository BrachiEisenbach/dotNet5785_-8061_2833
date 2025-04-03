using AutoMapper;

public static class MappingProfile
{
    private static readonly IMapper _mapper;

    static MappingProfile()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BO.Call, DO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => (DO.TYPEOFCALL)src.TypeOfCall));

            cfg.CreateMap<DO.Call, BO.Call>()
                .ForMember(dest => dest.TypeOfCall, opt => opt.MapFrom(src => (BO.TYPEOFCALL)src.TypeOfCall))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => CalculateStatus(src))); // חישוב סטטוס
        });

        _mapper = config.CreateMapper();
    }

    public static BO.Call ConvertToBO(DO.Call doCall) => _mapper.Map<BO.Call>(doCall);

    public static DO.Call ConvertToDO(BO.Call boCall) => _mapper.Map<DO.Call>(boCall);

    private static BO.STATUS CalculateStatus(DO.Call doCall)
    {
        if (doCall.MaxTimeToFinish.HasValue && doCall.MaxTimeToFinish.Value < DateTime.Now)
            return BO.STATUS.Expired;
        return BO.STATUS.Open;
    }
}
