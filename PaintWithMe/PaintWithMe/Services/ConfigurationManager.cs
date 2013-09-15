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
        public string RockBandDrumsGuid;


        public void Initialize()
        {
            ScreenWidth = 800;
            ScreenHeight = 480;
            RockBandDrumsGuid = "95062b40-1b2b-11e3-8001-444553540000";
        }


        public ServiceType GetServiceType()
        {
            return ServiceType.ConfigurationManager;
        }
    }
}
