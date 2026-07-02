// <copyright file="FindGoldenChatCommandPlugIn.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns.ChatCommands;

using System.Runtime.InteropServices;
using MUnique.OpenMU.GameLogic.NPC;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// GM chat command to locate Golden invasion monsters on all maps.
/// </summary>
[Guid("C2D3E4F5-A6B7-8901-CDEF-123456789012")]
[PlugIn]
[ChatCommandHelp(Command, CharacterStatus.GameMaster)]
public class FindGoldenChatCommandPlugIn : IChatCommandPlugIn
{
    private const string Command = "/findgolden";

    private static readonly HashSet<short> GoldenMonsterIds = [43, 53, 54, 78, 79, 80, 81, 82, 83];

    // Todos os mapas onde monstros dourados podem spawnar
    private static readonly ushort[] GoldenMapIds = [0, 2, 3, 7, 8]; // Lorencia, Devias, Noria, Atlans, Tarkan

    /// <inheritdoc />
    public string Key => Command;

    /// <inheritdoc />
    public CharacterStatus MinCharacterStatusRequirement => CharacterStatus.GameMaster;

    /// <inheritdoc />
    public async ValueTask HandleCommandAsync(Player player, string command)
    {
        var found = false;
        var center = new Pathfinding.Point(128, 128);

        foreach (var mapId in GoldenMapIds)
        {
            var map = await player.GameContext.GetMapAsync(mapId).ConfigureAwait(false);
            if (map is null)
            {
                continue;
            }

            var goldenOnMap = map.GetAttackablesInRange(center, 255)
                .OfType<Monster>()
                .Where(m => GoldenMonsterIds.Contains((short)m.Definition.Number))
                .GroupBy(m => m.Definition.Number)
                .ToList();

            foreach (var group in goldenOnMap)
            {
                var first = group.First();
                await player.ShowBlueMessageAsync(
                    $"{first.Definition.Designation} x{group.Count()} @ {map.Definition.Name} ({first.Position.X},{first.Position.Y})")
                    .ConfigureAwait(false);
                found = true;
            }
        }

        if (!found)
        {
            await player.ShowBlueMessageAsync("No Golden monsters found.").ConfigureAwait(false);
        }
    }
}
