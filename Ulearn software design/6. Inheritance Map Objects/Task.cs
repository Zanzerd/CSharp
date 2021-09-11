using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface IProtected
    {
        Army Army { get; set; }
    }

    public interface IOwned
    {
        int Owner { get; set; }
    }

    public interface ITreasure
    {
        Treasure Treasure { get; set; }
    }
    public class Dwelling : IOwned
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwned, IProtected, ITreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IProtected, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IProtected
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IProtected protectedObj)
            {
                if (!player.CanBeat(protectedObj.Army))
                    player.Die();
            }
            if (!player.Dead)
            {
                if (mapObject is ITreasure treasureObj)
                    player.Consume(treasureObj.Treasure);
                if (mapObject is IOwned ownedObj)
                    ownedObj.Owner = player.Id;
            }
            return;
        }
    }
}
