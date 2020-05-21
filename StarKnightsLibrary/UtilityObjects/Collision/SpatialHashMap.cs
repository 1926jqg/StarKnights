using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.UtilityObjects.Collision
{
    public class SpatialHashMap<T>
        where T : class, ICollidable
    {
        private readonly List<T>[] _buckets;
        private readonly int _cols;
        private readonly int _rows;
        private readonly int _xOffset;
        private readonly int _yOffset;
        private readonly int _cellSize;
        private readonly int _bucketsCount;


        public SpatialHashMap(int width, int height, int cellSize)
        {
            _cols = width / cellSize;
            _rows = height / cellSize;
            _xOffset = width / 2;
            _yOffset = height / 2;
            _bucketsCount = _cols * _rows;
            _buckets = new List<T>[_cols * _rows];
            _cellSize = cellSize;
            Clear();
        }

        public void Clear()
        {
            for (int index = 0; index < _bucketsCount; index++)
            {
                _buckets[index] = new List<T>();
            }
        }

        public void RegisterCollidable(T collidable)
        {
            foreach (var bucket in GetBucketsForCollidable(collidable))
            {
                _buckets[bucket].Add(collidable);
            }
        }

        private IEnumerable<int> GetBucketsForCollidable(T collidable)
        {
            return collidable?.GetBucketPoints(_xOffset, _yOffset)
                .Select(p => GetBucketForPoint(p))
                .Distinct() ?? new List<int>();
        }

        private int GetBucketForPoint(Vector2 point)
        {
            int x = (int)Math.Floor(point.X / _cellSize);
            int y = (int)Math.Floor(point.Y / _cellSize);

            if (x < 0)
                x = 0;
            else if (x >= _cols)
                x = _cols - 1;
            if (y < 0)
                y = 0;
            else if (y >= _rows)
                y = _rows - 1;

            return x + y * _cols;
        }

        public List<T> GetNearby(T collidable)
        {
            return GetBucketsForCollidable(collidable)
                .SelectMany(b => _buckets[b])
                .Where(b => b != collidable)
                .Distinct()
                .ToList();
        }
    }
}
