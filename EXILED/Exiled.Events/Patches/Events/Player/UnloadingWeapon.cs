// -----------------------------------------------------------------------
// <copyright file="UnloadingWeapon.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.Events.Patches.Events.Player
{
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using API.Features.Pools;

    using Exiled.API.Extensions;
    using Exiled.Events.Attributes;
    using Exiled.Events.EventArgs.Player;
    using HarmonyLib;
    using InventorySystem.Items.Firearms.Modules;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="AnimatorReloaderModuleBase.ServerProcessCmd" />.
    /// Adds the <see cref="Handlers.Player.UnloadingWeapon" /> event.
    /// </summary>
    [EventPatch(typeof(Handlers.Player), nameof(Handlers.Player.UnloadingWeapon))]
    [HarmonyPatch(typeof(AnimatorReloaderModuleBase), nameof(AnimatorReloaderModuleBase.ServerProcessCmd))]
    internal static class UnloadingWeapon
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Pool.Get(instructions);

            int offset = 2;
            int index = newInstructions.FindIndex(x => x.Calls(Method(typeof(IReloadUnloadValidatorModule), nameof(IReloadUnloadValidatorModule.ValidateUnload)))) + offset;

            Label skip = (Label)newInstructions[index - 1].operand;
            newInstructions.InsertRange(
                index,
                new[]
                {
                    // player
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new(OpCodes.Callvirt, PropertyGetter(typeof(AnimatorReloaderModuleBase), nameof(AnimatorReloaderModuleBase.Firearm))),

                    // UnloadingWeaponEventArgs ev = new(firearm)
                    new(OpCodes.Newobj, GetDeclaredConstructors(typeof(UnloadingWeaponEventArgs))[0]),
                    new(OpCodes.Dup),

                    // Player.OnUnloadingWeapon(ev)
                    new(OpCodes.Call, Method(typeof(Handlers.Player), nameof(Handlers.Player.OnUnloadingWeapon))),

                    // if (!ev.IsAllowed)
                    //    goto skip;
                    new(OpCodes.Callvirt, PropertyGetter(typeof(UnloadingWeaponEventArgs), nameof(UnloadingWeaponEventArgs.IsAllowed))),
                    new(OpCodes.Brfalse, skip),
                });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Pool.Return(newInstructions);
        }
    }
}