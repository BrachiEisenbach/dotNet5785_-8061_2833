

namespace DalApi;

/// <summary>
/// Represents the configuration interface for managing system-wide settings and time-related operations.
/// </summary>
public interface IConfig
{
    /// <summary>
    /// Gets and sets the current simulated clock time for the system.
    /// </summary>
    DateTime Clock { get; set; }

    /// <summary>
    /// Resets the system configuration to its default state.
    /// </summary>
    void Reset();

    /// <summary>
    /// Gets or sets the range of time considered as a "risk period."
    /// </summary>
    TimeSpan RiskRange { get; set; }
}

