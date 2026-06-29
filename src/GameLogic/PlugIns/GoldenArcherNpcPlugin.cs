// <copyright file="GoldenArcherNpcPlugin.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.GameLogic.PlugIns;

using System.Runtime.InteropServices;
using MUnique.OpenMU.DataModel.Configuration.Items;
using MUnique.OpenMU.GameLogic.NPC;
using MUnique.OpenMU.GameLogic.Views.Inventory;
using MUnique.OpenMU.GameLogic.Views.NPC;
using MUnique.OpenMU.PlugIns;

/// <summary>
/// Plugin that handles interaction with the Golden Archer NPC (236).
/// Players deliver Gemstones and receive Jewels of Bless as reward.
/// Every 5 Gemstones = 1 Jewel of Bless.
/// </summary>
[Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890")]
[PlugIn]
public class GoldenArcherNpcPlugin : IPlayerTalkToNpcPlugIn
{
    private const short NpcNumber = 236;
    private const byte GemstoneGroup = 14;
    private const byte GemstoneNumber = 41;
    private const byte JewelOfBlessGroup = 14;
    private const byte JewelOfBlessNumber = 13;
    private const int GemstonesPerJewel = 5;

    /// <inheritdoc />
    public async ValueTask PlayerTalksToNpcAsync(Player player, NonPlayerCharacter npc, NpcTalkEventArgs eventArgs)
    {
        if (npc.Definition.Number != NpcNumber)
        {
            return;
        }

        eventArgs.HasBeenHandled = true;

        // Contar Gemstones no inventário
        var inventory = player.Inventory;
        if (inventory is null)
        {
            await player.InvokeViewPlugInAsync<IShowMessageOfObjectPlugIn>(p =>
                p.ShowMessageOfObjectAsync("You have no inventory.", npc)).ConfigureAwait(false);
            await player.PlayerState.TryAdvanceToAsync(PlayerState.EnteredWorld).ConfigureAwait(false);
            return;
        }

        var gemstones = inventory.Items
            .Where(item => item.Definition?.Group == GemstoneGroup && item.Definition?.Number == GemstoneNumber)
            .ToList();

        if (gemstones.Count == 0)
        {
            await player.InvokeViewPlugInAsync<IShowMessageOfObjectPlugIn>(p =>
                p.ShowMessageOfObjectAsync("You don't have any Gemstones. Bring me Gemstones from Golden monsters!", npc)).ConfigureAwait(false);
            await player.PlayerState.TryAdvanceToAsync(PlayerState.EnteredWorld).ConfigureAwait(false);
            return;
        }

        int jewelsToGive = gemstones.Count / GemstonesPerJewel;
        int gemstonesUsed = jewelsToGive * GemstonesPerJewel;
        int gemstonesLeft = gemstones.Count - gemstonesUsed;

        if (jewelsToGive == 0)
        {
            await player.InvokeViewPlugInAsync<IShowMessageOfObjectPlugIn>(p =>
                p.ShowMessageOfObjectAsync($"You need at least {GemstonesPerJewel} Gemstones. You have {gemstones.Count}.", npc)).ConfigureAwait(false);
            await player.PlayerState.TryAdvanceToAsync(PlayerState.EnteredWorld).ConfigureAwait(false);
            return;
        }

        // Encontrar definição do Jewel of Bless
        var jewelDefinition = player.GameContext.Configuration.Items
            .FirstOrDefault(i => i.Group == JewelOfBlessGroup && i.Number == JewelOfBlessNumber);

        if (jewelDefinition is null)
        {
            await player.InvokeViewPlugInAsync<IShowMessageOfObjectPlugIn>(p =>
                p.ShowMessageOfObjectAsync("Reward item not found. Contact admin.", npc)).ConfigureAwait(false);
            await player.PlayerState.TryAdvanceToAsync(PlayerState.EnteredWorld).ConfigureAwait(false);
            return;
        }

        // Remover gemstones usadas
        var gemstonesToRemove = gemstones.Take(gemstonesUsed).ToList();
        foreach (var gemstone in gemstonesToRemove)
        {
            await inventory.RemoveItemAsync(gemstone).ConfigureAwait(false);
            await player.InvokeViewPlugInAsync<IItemRemovedPlugIn>(p => p.RemoveItemAsync(gemstone.ItemSlot)).ConfigureAwait(false);
        }

        // Dar Jewels of Bless
        for (int i = 0; i < jewelsToGive; i++)
        {
            var jewel = player.PersistenceContext.CreateNew<DataModel.Entities.Item>();
            jewel.Definition = jewelDefinition;
            jewel.Level = 0;
            jewel.Durability = 1;
            await inventory.AddItemAsync(jewel).ConfigureAwait(false);
            await player.InvokeViewPlugInAsync<IItemAppearPlugIn>(p => p.ItemAppearAsync(jewel)).ConfigureAwait(false);
        }

        string message = gemstonesLeft > 0
            ? $"Thank you! You received {jewelsToGive} Jewel(s) of Bless. You have {gemstonesLeft} Gemstone(s) remaining."
            : $"Thank you! You received {jewelsToGive} Jewel(s) of Bless.";

        await player.InvokeViewPlugInAsync<IShowMessageOfObjectPlugIn>(p =>
            p.ShowMessageOfObjectAsync(message, npc)).ConfigureAwait(false);

        await player.PlayerState.TryAdvanceToAsync(PlayerState.EnteredWorld).ConfigureAwait(false);
    }
}
