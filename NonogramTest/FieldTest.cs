using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramCore.Core;


namespace NonogramTest
{
    [TestClass]
    public class FieldTest
    {
        [TestMethod]
        public void InitializeTest()
        {
            var field = CreateField();
            field.Initialize();

            Assert.AreEqual(3, field.ColumnCount);
            Assert.AreEqual(3, field.RowCount);

            for (var row = 0; row < field.RowCount; row++)
            {
                for (var column = 0; column < field.ColumnCount; column++)
                {
                    Assert.AreEqual(TileState.Plain, field.GetTile(row, column).State);
                }
            }
        }

        [TestMethod]
        public void ColorTest()
        {
            var field = CreateField();
            field.Initialize();

            field.ColorTile(0, 0);
            field.ColorTile(0, 1);
            field.ColorTile(0, 2);
            field.ColorTile(1, 2);

            Assert.AreEqual(TileState.Colored, field.GetTile(0,0).State);
            Assert.AreEqual(TileState.Colored, field.GetTile(0,1).State);
            Assert.AreEqual(TileState.Colored, field.GetTile(0,2).State);
            Assert.AreEqual(TileState.Colored, field.GetTile(1,2).State);

            field.ColorTile(0, 0);
            field.ColorTile(0, 1);

            Assert.AreEqual(TileState.Plain, field.GetTile(0, 0).State);
            Assert.AreEqual(TileState.Plain, field.GetTile(0, 1).State);
        }

        [TestMethod]
        public void ExcludeTest()
        {
            var field = CreateField();
            field.Initialize();

            field.ExcludeTile(0, 0);
            field.ExcludeTile(0, 1);
            field.ExcludeTile(0, 2);
            field.ExcludeTile(1, 2);

            Assert.AreEqual(TileState.Excluded, field.GetTile(0, 0).State);
            Assert.AreEqual(TileState.Excluded, field.GetTile(0, 1).State);
            Assert.AreEqual(TileState.Excluded, field.GetTile(0, 2).State);
            Assert.AreEqual(TileState.Excluded, field.GetTile(1, 2).State);

            field.ExcludeTile(0, 0);
            field.ExcludeTile(0, 1);
            field.ColorTile(0, 2);
            field.ColorTile(1, 2);

            Assert.AreEqual(TileState.Plain, field.GetTile(0, 0).State);
            Assert.AreEqual(TileState.Plain, field.GetTile(0, 1).State);
            Assert.AreEqual(TileState.Colored, field.GetTile(0, 2).State);
            Assert.AreEqual(TileState.Colored, field.GetTile(1, 2).State);
        }

        [TestMethod]
        public void IsSolvedTest()
        {
            var field = CreateField();
            field.Initialize();

            Assert.AreEqual(false, field.IsSolved());

            field.ColorTile(0,0);
            field.ColorTile(0,1);
            field.ColorTile(0,2);
            field.ColorTile(2,0);
            field.ColorTile(2,1);
            field.ColorTile(2,2);

            Assert.AreEqual(false, field.IsSolved());

            field.ExcludeTile(1,0);
            field.ExcludeTile(1,1);
            field.ExcludeTile(1,2);

            Assert.AreEqual(true, field.IsSolved());
        }


        private Field CreateField()
        {
            return new Field(Difficulty.Easy);
        }
    }
}
