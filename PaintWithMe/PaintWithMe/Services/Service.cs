using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    public interface IService
    {
        void Initialize();

        ServiceType GetServiceType();
    }

    public enum ServiceType
    {
        RandomGenerator,
        Logger,
        ConfigurationManager,
        Game,
        TextureManager
    }
}
