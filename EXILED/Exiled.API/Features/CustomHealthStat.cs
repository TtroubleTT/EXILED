// -----------------------------------------------------------------------
// <copyright file="CustomHealthStat.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features
{
    using PlayerStatsSystem;

    /// <summary>
    /// A custom version of <see cref="HealthStat"/> which allows the player's max amount of health to be changed.
    /// TODO: Move to Features.CustomStats.
    /// </summary>
    public class CustomHealthStat : HealthStat
    {
        /// <inheritdoc/>
        public override float MaxValue => CustomMaxValue == default ? base.MaxValue : CustomMaxValue;

        /// <summary>
        /// Gets or sets the maximum amount of health the player will have.
        /// </summary>
        public float CustomMaxValue { get; set; }
    }
}