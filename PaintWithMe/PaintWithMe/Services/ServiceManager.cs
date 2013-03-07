using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintWithMe
{
    public class ServiceManager : List<IService>
    {
        private static ServiceManager singleton = new ServiceManager();

        public static ServiceManager Instance
        {
            get
            {
                return singleton;
            }
        }

        public IService GetService(ServiceType type)
        {
            IService retVal = null;

            foreach (IService service in this)
            {
                if (service.GetServiceType() == type)
                {
                    retVal = service;
                    break;
                }
            }

            if (retVal == null)
            {
                throw new Exception("Unable to obtain Service of type: " + type.ToString());
            }

            return retVal;
        }

        public T GetService<T>(ServiceType type)
        {
            return (T)GetService(type);
        }
    }
}
