using Core.Buildings;
using Core.Military;
using Core.Technologies;
using Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Trouver un meilleur nom de namespace
namespace Core.Configuration
{
    public class GameConfig : Singleton<GameConfig>
    {
        private GameConfig()
        {
            var data = GameConfigData.Load("config.xml");
            BuildingFactory = new BuildableEntityFactory<Building>(data.Entities.OfType<Building>());
            SoldierFactory = new BuildableEntityFactory<Soldier>(data.Entities.OfType<Soldier>());
            TechnologyFactory = new BuildableEntityFactory<Technology>(data.Entities.OfType<Technology>());
            WorldMapLandscape = data.Landscape;
        }
        public BuildableEntityFactory<Building> BuildingFactory { get; private set; }
        public BuildableEntityFactory<Soldier> SoldierFactory { get; private set; }
        public BuildableEntityFactory<Technology> TechnologyFactory { get; private set; }
        public Landscape WorldMapLandscape { get; private set; }
    }
}
