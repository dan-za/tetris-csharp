namespace Tetris.Utility {
    public static class MatrixFunctions<T> {
        
        // + 90 degree (clockwise)
        public static T[,] Rotate90DegreeCw(T[,] matrix) {

            var transpose = Transpose(matrix);

            var height = transpose.GetLength(0);
            var width = transpose.GetLength(1);
            
            // reverse each row
            var result = new T[height, width];

            for (int x = height - 1, i = 0; x >= 0; x--, i++) {
                for (var y = 0; y < width; y++) {
                    result[i, y] = transpose[x, y];
                }
            }
            return result;
        }
        
        // - 90 degree (counter clockwise) 
        public static T[,] Rotate90DegreeCcw(T[,] matrix) {

            var transpose = Transpose(matrix);

            var height = transpose.GetLength(0);
            var width = transpose.GetLength(1);
            
            // reverse each column
            var result = new T[height, width];

            for (var x = 0; x < height; x++) {
                for (int y = width - 1, i = 0; y >= 0; y--, i++) {
                    result[x, i] = transpose[x, y];
                }
            }
            return result;
        }

        private static T[,] Transpose(T[,] matrix) {

            var height = matrix.GetLength(0);
            var width = matrix.GetLength(1);
            
            var transpose = new T[width, height];

            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    transpose[x, y] = matrix[y, x];
                }
            }
            return transpose;
        }
        
    }
}