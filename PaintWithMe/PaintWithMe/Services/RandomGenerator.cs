using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace PaintWithMe
{
    class RandomGenerator : IService
    {
        Random randGenerator;

        private Game _gameService;

        protected Game GameService
        {
            get
            {
                if (_gameService == null)
                {
                    _gameService = ServiceManager.Instance.GetService<Game>(ServiceType.Game);
                }
                return _gameService;
            } 
        }

        internal int Next(int max)
        {
            return randGenerator.Next(max);
        }

        internal int Next(int min, int max)
        {
            return randGenerator.Next(min, max);
        }

        internal double NextDouble(double min, double max)
        {
            return randGenerator.NextDouble() * (max - min) + min;
        }

        internal bool NextBool()
        {
            return (randGenerator.Next(2) > 0);
        }

        internal int NextXLocation()
        {
            return Next(GameService.Window.ClientBounds.Width);
        }

        /// <summary>
        /// Returns a new X-location filtered by a specific area of the board
        /// </summary>
        /// <param name="lowRangePercentage">A percentage that indicates the low end (inclusive) of the region.</param>
        /// <param name="highRangePercentage">A percentage that indicates the high end (exclusive) of the region.</param>
        /// <returns></returns>
        internal int NextXLocation(int lowRangePercentage, int highRangePercentage)
        {
            int baseWidth = GameService.Window.ClientBounds.Width;
            return Next((int)(baseWidth * lowRangePercentage * .01), (int)(baseWidth * highRangePercentage * .01));
        }

        internal int NextYLocation()
        {
            return Next(GameService.Window.ClientBounds.Height);
        }

        /// <summary>
        /// Returns a new Y-location filtered by a specific area of the board
        /// </summary>
        /// <param name="lowRangePercentage">A percentage that indicates the low end (inclusive) of the region.</param>
        /// <param name="highRangePercentage">A percentage that indicates the high end (exclusive) of the region.</param>
        /// <returns></returns>
        internal int NextYLocation(int lowRangePercentage, int highRangePercentage)
        {
            int baseHeight = GameService.Window.ClientBounds.Height;
            return Next((int)(baseHeight * lowRangePercentage * .01), (int)(baseHeight * highRangePercentage * .01));
        }

        public void Initialize()
        {
            randGenerator = new Random((int)DateTime.Now.Ticks);
        }

        public ServiceType GetServiceType()
        {
            return ServiceType.RandomGenerator;
        }
    }
}
