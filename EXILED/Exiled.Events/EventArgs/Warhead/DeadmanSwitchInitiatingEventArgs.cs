// -----------------------------------------------------------------------
// <copyright file="DeadmanSwitchInitiatingEventArgs.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.EventArgs.Warhead
{
    using Exiled.Events.EventArgs.Interfaces;

    /// <summary>
    /// Contains all information before detonating the warhead.
    /// </summary>
    public class DeadmanSwitchInitiatingEventArgs : IDeniableEvent
    {
        /// <inheritdoc/>
        public bool IsAllowed { get; set; } = true;
    }
}
