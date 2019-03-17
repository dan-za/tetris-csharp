using Tetris.Utility;

namespace Tetris.Types {
    public class Tetronimo {
        
        private Tile[,] _asset;
        private int _offsetX;
        private int _offsetY;

        public Tetronimo(Tile[,] asset, int offsetX, int offsetY) {
            _asset = asset;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        public void MoveLeft() {
            _offsetY--;
        }

        public void MoveRight() {
            _offsetY++;
        }

        public void MoveDown() {
            _offsetX++;
        }

        public void MoveUp() {
            _offsetX--;
        }

        public void RotateClockwise() {
            _asset = MatrixFunctions<Tile>.Rotate90DegreeCw(_asset);
        }

        public void RotateCounterClockwise() {
            _asset = MatrixFunctions<Tile>.Rotate90DegreeCcw(_asset);
        }
        
        public int GetHeight() {
            return _asset.GetLength(0);
        }
        
        public int GetWidth() {
            return _asset.GetLength(1);
        }

        private bool IsValidPosition(int worldPositionX, int worldPositionY) {
            return (worldPositionX >= _offsetX && worldPositionX < _offsetX + GetHeight() &&
                    worldPositionY >= _offsetY && worldPositionY < _offsetY + GetWidth());
        }
        
        public Tile GetRelativeTo(int worldPositionX, int worldPositionY) {

            if (!IsValidPosition(worldPositionX, worldPositionY)) return Tile.Empty;
            
            return _asset[worldPositionX - _offsetX, worldPositionY - _offsetY];
        }
        
        public Tile GetAbsolute(int x, int y) {

            if (x < 0 || x >= GetHeight() || y < 0 || y >= GetWidth()) return Tile.Empty;
            
            return _asset[x, y];
        }
        
        
    }
}