using System.Net;

using FWO.Basics;

using NetTools;

namespace FWO.Basics.Comparer
{
    public class IPAdressComparer : IComparer<IPAddress>
    {
        public int Compare(IPAddress? x, IPAddress? y)
        {
            if (x is null || y is null)
            {
                return 0;
            }

            int compareIPFamiliesResult = IpOperations.CompareIpFamilies(x, y);

            if (compareIPFamiliesResult != 0)
            {
                return compareIPFamiliesResult;
            }

            return IpOperations.CompareIpValues(x, y);
        }
    }
}
