﻿using PacEngine.board.tiles;
using PacEngine.utils;

namespace PacEngine.board
{
    public class Board
    {
        public AbstractBoardTile[][] Tiles { get; private set; }
        public Vector SpawnRoomPosition { get; private set; }

        public Board(TileInfo[][] boardTilesInfo, Vector spawnRoomPosition)
        {
            SpawnRoomPosition = spawnRoomPosition;

            Tiles = new AbstractBoardTile[boardTilesInfo.Length][];
            for (int x = 0; x < boardTilesInfo.Length; x++)
            {
                Tiles[x] = new AbstractBoardTile[boardTilesInfo[x].Length];
                for (int y = 0; y < boardTilesInfo[x].Length; y++)
                {
                    Tiles[x][y] = TileFactory.GetTile(boardTilesInfo[x][y], new Vector(x, y));
                }
            }

            for (int x = 0; x < boardTilesInfo.Length; x++)
            {
                for (int y = 0; y < boardTilesInfo[x].Length; y++)
                {
                    Tiles[x][y].ResolveNeighbors(this);
                }
            }
        }

        internal Vector ToBounds(Vector vector)
        {
            var x = MathUtils.Clamp(vector.x, 0, Tiles.Length);
            var newVector = new Vector(x, MathUtils.Clamp(vector.y, 0, Tiles[x].Length));

            return newVector;
        }

        public override string ToString() => LogBoard();

        public bool TryGetTileAt(Vector position, out AbstractBoardTile element)
        {
            element = null;
            if (!InBounds(position))
                return false;

            element = Tiles[position.x][position.y];
            return true;
        }

        public AbstractBoardTile GetTileAt(Vector position)
        {
            if (!InBounds(position))
                throw new PacException($"Please make sure that position {position} is in map bounds. If you are not sure, use TryGetTileAt method instead");

            return Tiles[position.x][position.y];
        }

        private bool InBounds(Vector position)
        {
            return position.x >= 0 && position.x < Tiles.Length &&
                    position.y >= 0 && position.y < Tiles[position.x].Length;
        }

        private string LogBoard()
        {
            var str = "";

            for (int x = 0; x < Tiles.Length; x++)
            {
                for (int y = 0; y < Tiles[x].Length; y++)
                {
                    if (PacEngine.Instance.Pacman.Position.Compare(new Vector(x, y)))
                        str += "u";
                    else if (x == SpawnRoomPosition.x && y == SpawnRoomPosition.y)
                        str += "s";
                    else if (PacEngine.Instance.Blinky.Position.Compare(new Vector(x, y)))
                        str += "b";
                    else if (PacEngine.Instance.Clyde.Position.Compare(new Vector(x, y)))
                        str += "c";
                    else if (PacEngine.Instance.Inky.Position.Compare(new Vector(x, y)))
                        str += "i";
                    else if (PacEngine.Instance.Pinky.Position.Compare(new Vector(x, y)))
                        str += "p";
                    else
                        str += Tiles[x][y];
                }
                str += "\n";
            }

            return str;
        }
    }
}
