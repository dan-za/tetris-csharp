using System.Collections.Generic;
using System.Text;
using Xunit;
using Tetris.Types;
using Xunit.Abstractions;

namespace TetrisTest {
    public class BoardTest {
        
        private readonly ITestOutputHelper _output;

        public BoardTest(ITestOutputHelper output) {
            this._output = output;
        }

        [Fact]
        public void ShouldClearOneRow()
        {
                        
            var asset = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ','D','D',' ',' ',' ',' ','#'},
                {'#',' ',' ','D','D',' ',' ',' ',' ','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','B','B','B',' ',' ',' ',' ',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
        
            var board = GetBoard(asset);
            
            board.ClearCompletedLines();

            
            var expected = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ','D','D',' ',' ',' ',' ','#'},
                {'#',' ',' ','D','D',' ',' ',' ',' ','#'},
                {'#','B','B','B',' ',' ',' ',' ',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };


            var result = GetAsCharArray(board);
            
            Print(board);

            Assert.Equal(expected, result);
        }

        

        [Fact]
        public void ShouldClearTwoRows()
        {
                                    
            var asset = new[,]
            {
                {'#',' ',' ',' ','D',' ',' ','E','E','#'},
                {'#',' ',' ','D','D','D',' ','E','E','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','B','B','B',' ',' ',' ',' ',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
        
            var board = GetBoard(asset);
            
            board.ClearCompletedLines();

            
            var expected = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ','D',' ',' ',' ',' ','#'},
                {'#',' ',' ','D','D','D',' ','E','E','#'},
                {'#','B','B','B',' ',' ',' ','E','E','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };


            var result = GetAsCharArray(board);
            
            Print(board);
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldClearThreeRows()
        {
            var asset = new[,]
            {
                {'#','U',' ',' ',' ',' ',' ',' ',' ','#'}, // 0
                {'#','U',' ',' ',' ',' ','F','F',' ','#'}, // 1
                {'#','U','U',' ',' ',' ','F','F',' ','#'}, // 2
                {'#','X','X','X','X','X','X','X','X','#'}, // 3
                {'#','X','X','X','X','X','X','X','X','#'}, // 4        
                {'#',' ',' ','D','D',' ',' ','E',' ','#'}, // 5
                {'#','X','X','X','X','X','X','X','X','#'}, // 6
                {'#','B','B','B',' ',' ',' ',' ',' ','#'}, // 7
                {'#','#','#','#','#','#','#','#','#','#'}, // 8
            };
        
            var board = GetBoard(asset);
            
            board.ClearCompletedLines();

            
            var expected = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#','U',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#','U',' ',' ',' ',' ','F','F',' ','#'},
                {'#','U','U','D','D',' ','F','F',' ','#'},
                {'#','B','B','B',' ',' ',' ','E',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };


            var result = GetAsCharArray(board);
            
            Print(board);
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldClearFourRows()
        {
            var asset = new[,]
            {
                {'#','U',' ',' ',' ',' ','F','F',' ','#'}, 
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','U','U',' ',' ',' ','F','F',' ','#'}, 
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','X','X','X','X','X','X','X','X','#'},        
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#',' ',' ','D','D',' ',' ','E',' ','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','B','B','B',' ',' ',' ',' ',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
        
            var board = GetBoard(asset);
            
            board.ClearCompletedLines();

            
            var expected = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#','U',' ',' ',' ',' ','F','F',' ','#'},
                {'#','U','U','D','D',' ','F','F',' ','#'},
                {'#','B','B','B',' ',' ',' ','E',' ','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };


            var result = GetAsCharArray(board);
            
            Print(board);
            
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void ShouldClearMultipleRows()
        {
            var asset = new[,]
            {
                {'#','U',' ','B',' ',' ','F','F',' ','#'}, 
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','U','U',' ',' ',' ','F','F',' ','#'}, 
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','X','X','X','X','X','X','X','X','#'},        
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#',' ',' ','D','D',' ',' ','E',' ','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','X','X','X','X','X','X','X','X','#'},
                {'#','B','B','B',' ',' ','V',' ','V','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };
        
            var board = GetBoard(asset);
            
            board.ClearCompletedLines();

            
            var expected = new[,]
            {
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'},
                {'#',' ',' ',' ',' ',' ',' ',' ',' ','#'}, 
                {'#','U',' ','B',' ',' ','F','F',' ','#'}, 
                {'#','U','U','D','D',' ','F','F',' ','#'},
                {'#','B','B','B',' ',' ','V','E','V','#'},
                {'#','#','#','#','#','#','#','#','#','#'},
            };


            var result = GetAsCharArray(board);
            
            Print(board);
            
            Assert.Equal(expected, result);
        }

        private static Tile[,] CharsToTiles(char[,] chars) {

            var tiles = new Tile[chars.GetLength(0), chars.GetLength(1)];
            
            for (var x = 0; x < tiles.GetLength(0); x++) {
                for (var y = 0; y < tiles.GetLength(1); y++) {
                    tiles[x, y] = Get(chars[x, y]);
                }
            }

            return tiles;
        }

        private static char Get(Tile tile) {
            var tiles = new Dictionary<Tile, char> {
                {Tile.Empty, ' '},
                {Tile.Wall, '#'},
                {Tile.Blue, 'U'},
                {Tile.Red, 'F'},
                {Tile.Yellow, 'X'},
                {Tile.Green, 'D'},
                {Tile.Orange, 'B'},
                {Tile.Pink, 'E'},
                {Tile.Violet, 'V'}
            };
            return tiles[tile];
        }
        
        private static Tile Get(char c) {
            var chars = new Dictionary<char, Tile> {
                {' ', Tile.Empty},
                {'#', Tile.Wall},
                {'U', Tile.Blue},
                {'F', Tile.Red},
                {'X', Tile.Yellow},
                {'D', Tile.Green},
                {'B', Tile.Orange},
                {'E', Tile.Pink},
                {'V', Tile.Violet}
            };
            return chars[c];
        }
        
        
        private Board GetBoard(char[,] asset) {
            return new Board(CharsToTiles(asset));
        }

        private void Print(Board board)
        {
            for (var x = 0; x < board.GetHeight(); x++) {
                var lineBuilder = new StringBuilder();
                for (var y = 0; y < board.GetWidth(); y++) {
                    lineBuilder.Append(Get(board.Get(x, y)));
                    
                }
                _output.WriteLine(lineBuilder.ToString());
            }
        }
        
        private char[,] GetAsCharArray(Board board) {
            var chars = new char[board.GetHeight(), board.GetWidth()];
            
            for (var x = 0; x < board.GetHeight(); x++)
            {
                for (var y = 0; y < board.GetWidth(); y++) {
                    chars[x, y] = Get(board.Get(x, y));
                }
            }

            return chars;
        }
    }
}