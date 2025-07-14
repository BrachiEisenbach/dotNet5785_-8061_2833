

namespace Dal;
using DalApi;
using System;
using System.Runtime.CompilerServices;


/// <summary>
/// Represents an implementation of the <see cref="IConfig"/> interface.
/// Provides access to system-wide settings and configuration.
/// </summary>
/// <param name="Clock">
/// The simulated clock for the system, representing the current date and time in the application.
/// </param>
/// <param name="RiskRange">
/// Defines the range of time considered as a "risk period," used for calculations or validations.
/// </param>
/// <param name="Reset">
/// Resets the system's configuration to its default state, including resetting the clock and risk range.
/// </param>

internal class ConfigImplementation : IConfig
{

    /// <summary>
    /// The simulated clock for the system, representing the current date and time.
    /// </summary>
    public DateTime Clock
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.Clock;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.Clock = value;
    }

    /// <summary>
    /// Defines the range of time considered a "risk period,"
    /// </summary>
    public TimeSpan RiskRange
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get => Config.RiskRange;
        [MethodImpl(MethodImplOptions.Synchronized)]
        set => Config.RiskRange = value;
    }

    /// <summary>
    /// Resets the configuration settings to their default values.
    /// This includes resetting the clock and risk range.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Reset()
    {
        Config.Reset();
    }

}
