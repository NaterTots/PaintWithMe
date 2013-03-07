using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    class RandomGenerator : IService
    {
        Random randGenerator;

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
