
/// <summary>
/// This class contains configuration values and settings for the system.
/// It manages identifiers for calls and assignments, as well as system clock and risk range.
/// </summary>
namespace Dal;

internal static class Config
{
    /// <summary>
    /// The starting value for the call identifier. This constant defines the smallest identifier for the call entity.
    /// </summary>
    internal const int StartCallId = 0;
    /// <summary>
    /// The private static field that holds the next call ID and is initialized with the current call ID.
    /// </summary>
    private static int s_nextCallId = StartCallId;
    /// <summary>
    /// Gets the next available call identifier, and increments it automatically.
    /// This property ensures that each call identifier is unique and sequential.
    /// </summary>
    internal static int S_NextCallId { get => s_nextCallId++; }

    /// <summary>
    /// The starting value for the assignment identifier.
    /// </summary>
    internal const int StartAssignmentId = 0;
    /// <summary>
    /// The private static field that holds the next assignment ID and is initialized with the current assignment ID.
    /// </summary>
    private static int s_nextAssignmentId = StartAssignmentId;

    /// <summary>
    /// Gets the next available assignment identifier, and increments it automatically.
    /// This property ensures that each assignment identifier is unique and sequential.
    /// </summary>
    internal static int S_NextAssignmentId { get => s_nextAssignmentId++; }
    /// <summary>
    /// The system clock, representing the current time.
    /// </summary>
    internal static DateTime Clock { get; set; }
    /// <summary>
    /// The risk range time span used for calculations or decision-making in the system.
    /// </summary>
    internal static TimeSpan RiskRange { get; set; }


    /// <summary>
    /// Resets all configuration values to their initial state. This includes resetting
    /// the next available call and assignment identifiers, as well as the system clock and risk range.
    /// </summary>
    internal static void Reset()
    {
        s_nextCallId = StartCallId;
        s_nextAssignmentId = StartAssignmentId;


        Clock = default(DateTime);
        RiskRange = default(TimeSpan);

    }
}