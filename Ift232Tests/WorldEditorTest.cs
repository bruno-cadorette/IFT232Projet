using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameBuilder.Utility;
using System.IO;
using GameBuilder;
namespace Ift232Tests
{
    [TestClass]
    public class WorldEditorTest
    {
        private MapEditorViewModel mapEditor;

        [TestInitialize]
        public void Init()
        {
            mapEditor = new MapEditorViewModel(new TileSetGenerator(@".\test_tileset.png", 32, 32, 1),10,10);
        }
        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void LoadTileSet()
        {
            const int tileCount = 108;
            Assert.AreEqual(tileCount, mapEditor.LandscapeSelector.Count);
            Assert.IsTrue(mapEditor.LandscapeSelector.All(x => x.Tile.PixelHeight == 32 && x.Tile.PixelWidth == 32));
        }
        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void SelectTile()
        {
            mapEditor.SelectLandscape.Execute(mapEditor.LandscapeSelector.ElementAt(3));
            Assert.AreEqual(3, mapEditor.SelectedLandscape.ID);
        }
        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void FillFromStartEditor()
        {
            mapEditor.FillMode = true;
            mapEditor.SelectLandscape.Execute(mapEditor.LandscapeSelector.ElementAt(3));
            mapEditor.ChangeLand.Execute(0);
            Assert.IsTrue(mapEditor.LandscapeTiles.All(x => x.ID == 3), mapEditor.LandscapeTiles.Count(x=>x.ID==3) + " / " + mapEditor.LandscapeTiles.Count);
        }

        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void FillFromMiddleEditor()
        {
            mapEditor.FillMode = true;
            mapEditor.SelectLandscape.Execute(mapEditor.LandscapeSelector.ElementAt(3));
            mapEditor.ChangeLand.Execute(55);
            Assert.IsTrue(mapEditor.LandscapeTiles.All(x => x.ID == 3), mapEditor.LandscapeTiles.Count(x => x.ID == 3) + " / " + mapEditor.LandscapeTiles.Count);
        }

        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void ChangeATile()
        {
            mapEditor.SelectLandscape.Execute(mapEditor.LandscapeSelector.ElementAt(9));
            mapEditor.ChangeLand.Execute(14);
            Assert.AreEqual(9, mapEditor.LandscapeTiles[14].ID);
            Assert.AreEqual(1, mapEditor.LandscapeTiles.Count(x => x.ID == 9));
        }

        [TestMethod]
        [DeploymentItem(@"Data\test_tileset.png")]
        public void EraseTile()
        {
            mapEditor.SelectLandscape.Execute(mapEditor.LandscapeSelector.ElementAt(9));
            mapEditor.ChangeLand.Execute(14);
            mapEditor.EraseMode = true;
            mapEditor.ChangeLand.Execute(14);
            Assert.IsTrue(mapEditor.LandscapeTiles.All(x => x.ID == 0));
        }
    }
}
