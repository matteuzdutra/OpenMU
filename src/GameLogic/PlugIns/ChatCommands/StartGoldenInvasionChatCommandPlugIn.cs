// <copyright file="StartGoldenInvasionChatCommandPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns.ChatCommands;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic.PlugIns.InvasionEvents;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Chat command to force-start the Golden Invasion event. GM only.
/// </summary>
[Guid("B1C2D3E4-F5A6-7890-BCDE-F12345678901")]
[PlugIn]
[ChatCommandHelp(Command, CharacterStatus.GameMaster)]
public class StartGoldenInvasionChatCommandPlugIn : IChatCommandPlugIn
{
    private const string Command = "/startgolden";

    /// <inheritdoc />
    public string Key => Command;

    /// <inheritdoc />
    public CharacterStatus MinCharacterStatusRequirement => CharacterStatus.GameMaster;

    private static readonly Guid GoldenInvasionPlugInGuid = new("06D18A9E-2919-4C17-9DBC-6E4F7756495C");

    /// <inheritdoc />
    public async ValueTask HandleCommandAsync(Player player, string command)
    {
        var invasion = player.GameContext.PlugInManager
            .GetActivePlugInsOf<IPeriodicTaskPlugIn>()
            .OfType<GoldenInvasionPlugIn>()
            .FirstOrDefault();

        if (invasion is null)
        {
            await player.ShowBlueMessageAsync("Golden Invasion plugin not found or not active.").ConfigureAwait(false);
            return;
        }

        invasion.ForceStart();
        await player.ShowBlueMessageAsync("Golden Invasion forced to start!").ConfigureAwait(false);
    }
}
