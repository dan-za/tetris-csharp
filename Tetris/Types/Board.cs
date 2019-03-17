using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tetris.Types {
    
    public class Board {
        
        private Tile[,] _asset;

        public Board(int width, int height) {
            if (width < 1) throw new ArgumentException("Must be greater than 0.", nameof(width));
            if (height < 1) throw new ArgumentException("Must be greater than 0.", nameof(height));

            CreateBoard(width, height);
        }
        
        public Board(Tile[,] board) {
            _asset = board;
        }

        private void CreateBoard(int width, int height) {
            _asset = new Tile[height, width];

            for (var x = 0; x < height; x++) {
                for (var y = 0; y < width; y++) {
                    if (y == 0 || y == width - 1)
                        _asset[x, y] = Tile.Wall;
                    else if (x == height - 1)
                        _asset[x, y] = Tile.Wall;
                    else
                        _asset[x, y] = Tile.Empty;
                }
            }
        }

        public int GetWidth() {
            return _asset.GetLength(1);
        }

        public int GetHeight() {
            return _asset.GetLength(0);
        }

        public int ClearCompletedLines() {
            var completedLines = GetCompletedLines();

            if (!completedLines.Any()) return 0;

            ClearCompletedLines(completedLines);

            GetRanges(completedLines).Select(GetFloatingArea).ToList().ForEach(ExtractAndPlaceShapes);

            return completedLines.Count;
        }

        private Tile[,] GetFloatingArea(Tuple<int, int> range) {

            var floatingArea = new Tile[GetHeight(), GetWidth()];

            for (var x = 0; x < GetHeight(); x++) {
                for (var y = 0; y < GetWidth(); y++) {

                    if (x >= range.Item1 && x <= range.Item2) {
                        floatingArea[x, y] = _asset[x, y];    
                    }
                    else {
                        floatingArea[x, y] = Tile.Empty;
                    }
                }
            }

            return floatingArea;
        }

        private void ClearCompletedLines(List<int> completedLines) {
            foreach (var completedLine in completedLines) {
                for (var y = 0; y < GetWidth(); y++) {
                    if (_asset[completedLine, y] != Tile.Wall) _asset[completedLine, y] = Tile.Empty;
                }
            }
        }

        private List<int> GetCompletedLines() {
            var completedLines = new List<int>();

            // ignore floor
            for (var x = 0; x < GetHeight() - 1; x++) {
                if (IsLineComplete(x)) completedLines.Add(x);
            }
            return completedLines;
        }

        private bool IsLineComplete(int x) {
            
            for (var y = 0; y < GetWidth(); y++) {
                if (_asset[x, y] == Tile.Empty) { return false; }
            }
            return true;
        }
        
        private void ExtractAndPlaceShapes(Tile[,] floatingArea) {

            var point = Find(floatingArea);

            while (point.HasValue) {
                
                var shape = new List<Tuple<Point, Tile>>();
                ExtractShape(point.Value, floatingArea, shape);

                var tetronimo = CreateTetronimo(shape);
                
                Place(tetronimo);
                
                point = Find(floatingArea);
            }
        }
        
        private static Point? Find(Tile[,] floatingArea) {
            
            for (var x = 0; x < floatingArea.GetLength(0); x++) {
                for (var y = 0; y < floatingArea.GetLength(1); y++) {
                    if (floatingArea[x, y] != Tile.Empty && floatingArea[x, y] != Tile.Wall) return new Point(x, y);
                }
            }
            return null;
        }
        
        private void ExtractShape(Point point, Tile[,] floatingArea, List<Tuple<Point, Tile>> shape) {
            var x = point.X;
            var y = point.Y;

            if (x < 0 || x >= floatingArea.GetLength(0) || 
                y < 0 || y >= floatingArea.GetLength(1) || 
                floatingArea[x, y] == Tile.Empty ||
                floatingArea[x, y] == Tile.Wall)
                return;

            shape.Add(Tuple.Create(point, floatingArea[x, y]));

            // Clear
            _asset[x, y] = Tile.Empty;
            floatingArea[x, y] = Tile.Empty;

            ExtractShape(new Point(x + 1, y), floatingArea, shape);
            ExtractShape(new Point(x - 1, y), floatingArea, shape);
            ExtractShape(new Point(x, y + 1), floatingArea, shape);
            ExtractShape(new Point(x, y - 1), floatingArea, shape);
        }

        private Tetronimo CreateTetronimo(List<Tuple<Point, Tile>> shape) {
            
            // create Tile array
            var maxX = shape.Max(s => s.Item1.X) + 1;
            var maxY = shape.Max(s => s.Item1.Y) + 1;

            var asset = new Tile[maxX, maxY];
                
            // init with blanks
            for (var x = 0; x < maxX; x++) {
                for (var y = 0; y < maxY; y++) {
                    asset[x, y] = Tile.Empty;
                }
            }

            foreach (var tuple in shape) {
                asset[tuple.Item1.X, tuple.Item1.Y] = tuple.Item2;
            }
                
            var tetronimo = new Tetronimo(asset, 0,0);
            DropDown(tetronimo);

            return tetronimo;
        }

        private void DropDown(Tetronimo tetronimo) {

            while (Fits(tetronimo)) {
                tetronimo.MoveDown();
            }
            tetronimo.MoveUp();
        }

        private static List<Tuple<int, int>> GetRanges(List<int> completedLines) {

            completedLines.Sort();
            
            var ranges = new List<Tuple<int, int>>();

            ranges.Add(Tuple.Create(0, completedLines[0] - 1));

            for (var i = 0; i < completedLines.Count - 1; i++) {
                // ignore completed lines which 'stick' together
                if (completedLines[i] + 1 != completedLines[i + 1]) {
                    ranges.Add(Tuple.Create(completedLines[i] + 1, completedLines[i + 1] - 1));
                }
            }
            // Go from bottom range to top
            ranges.Reverse();

            return ranges;
        }

        public bool Fits(Tetronimo tetronimo) {

            for (var x = 0; x < GetHeight(); x++) {
                for (var y = 0; y < GetWidth(); y++) {
                    if (_asset[x, y] != Tile.Empty && tetronimo.GetRelativeTo(x,y) != Tile.Empty) return false;
                }
            }
            return true;
        }

        public void Place(Tetronimo tetronimo) {
            
            for (var x = 0; x < GetHeight(); x++) {
                for (var y = 0; y < GetWidth(); y++) {
                    var c = tetronimo.GetRelativeTo(x, y);
                    if (c != Tile.Empty) _asset[x, y] = c;
                }
            }
        }

        public Tile Get(int x, int y) {
            return _asset[x, y];
        }
    }
}