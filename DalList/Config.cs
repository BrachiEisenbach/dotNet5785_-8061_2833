
namespace Dal;

internal static class Config
{
    internal const int StartCallId = 0;
    private static int s_nextCallId = StartCallId;
    internal static int S_NextCallId { get => s_nextCallId++; }

    internal const int StartAssignmentId = 0;
    private static int s_nextAssignmentId = StartAssignmentId;

    internal static int S_NextAssignmentId { get => s_nextAssignmentId++; }
    internal static DateTime Clock { get; set; }
    internal static TimeSpan RiskRange { get; set; }


    internal static void Reset()
    {
        s_nextCallId = StartCallId;
        s_nextAssignmentId = StartAssignmentId;


        Clock = default(DateTime);
        RiskRange = default(TimeSpan);

    }
}
