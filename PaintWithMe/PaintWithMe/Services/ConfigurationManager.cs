using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    public class ConfigurationManager : IService
    {
        public int ScreenWidth;
        public int ScreenHeight;

        public void Initialize()
        {
            ScreenWidth = 800;
            ScreenHeight = 480;
        }


        public ServiceType GetServiceType()
        {
            return ServiceType.ConfigurationManager;
        }
    }
}
