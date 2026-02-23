namespace FWO.Data.Modelling
{
    public class ModellingManagedIdString
    {
        private const string separator = "-";

        public ModellingNamingConvention NamingConvention { get; set; } = new();


        public ModellingManagedIdString()
        { }

        public ModellingManagedIdString(string idstring)
        {
            Whole = idstring;
            NamingConvention = new();
        }

        public ModellingManagedIdString(ModellingManagedIdString managedIdstring)
        {
            Whole = managedIdstring.Whole;
            NamingConvention = managedIdstring.NamingConvention;
        }

        public string Whole { get; set; } = "";

        public string FixedPart
        {
            get
            {
                return Whole.Length >= NamingConvention.FixedPartLength ? Whole.Substring(0, NamingConvention.FixedPartLength) : Whole;
            }
            set
            {
                string valueToInsert = value.Length > NamingConvention.FixedPartLength ? value.Substring(0, NamingConvention.FixedPartLength) : value;
                valueToInsert = FillFixedIfNecessary(valueToInsert, "?");
                if (Whole.Length >= NamingConvention.FixedPartLength)
                {
                    Whole = valueToInsert + Whole.Substring(NamingConvention.FixedPartLength);
                }
                else
                {
                    Whole = valueToInsert;
                }
            }
        }

        public string AppPart
        {
            get
            {
                return NamingConvention.UseAppPart ? (AppPartExisting() ? Whole.Substring(NamingConvention.FixedPartLength, AppPartEnd() - NamingConvention.FixedPartLength + 1) : "") : "";
            }
            set
            {
                if (NamingConvention.UseAppPart)
                {
                    Whole = FillFixedIfNecessary(Whole);
                    Whole = Whole.Substring(0, NamingConvention.FixedPartLength) + value + FreePart;
                }
            }
        }

        public string CombinedFixPart
        {
            get
            {
                return FixedPart + (AppPart.EndsWith(separator) ? AppPart.Substring(0, AppPart.Length - 1) : AppPart);
            }
            set
            {
                Whole = value + FreePart;
            }
        }

        public string Separator
        {
            get
            {
                return NamingConvention.UseAppPart && AppPart.EndsWith(separator) ? separator : "";
            }
            set
            {
                if (NamingConvention.UseAppPart)
                {
                    AppPart += value;
                }
            }
        }

        public string FreePart
        {
            get
            {
                if (NamingConvention.UseAppPart && AppPartExisting())
                {
                    int appPartEnd = AppPartEnd();
                    int startIndex = appPartEnd + 1;
                    if (startIndex >= 0 && startIndex < Whole.Length)
                    {
                        return Whole.Substring(startIndex);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    int startIndex = NamingConvention.FixedPartLength;
                    if (startIndex >= 0 && startIndex < Whole.Length)
                    {
                        return Whole.Substring(startIndex);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            set
            {
                Whole = FillFixedIfNecessary(Whole);
                int insertIndex = AppPartExisting() ? AppPartEnd() + 1 : NamingConvention.FixedPartLength;
                if (insertIndex >= 0 && insertIndex <= Whole.Length)
                {
                    Whole = Whole.Substring(0, insertIndex) + value;
                }
                else
                {
                    Whole += value;
                }
            }
        }

        public void SetAppPartFromExtId(string extAppId)
        {
            string zoneType = extAppId.StartsWith("APP") ? "0" : (extAppId.StartsWith("COM") ? "1" : "?");
            int idx = extAppId.IndexOf(separator);
            string appNumber = idx > 0 ? extAppId.Substring(idx + 1, extAppId.Length - idx - 1) : "";
            AppPart = zoneType + appNumber + separator;
        }

        public void SetAppPartFromExtIdAZ(string extAppId)
        {
            string zoneType = extAppId.StartsWith("APP") ? "0" : (extAppId.StartsWith("COM") ? "1" : "?");
            int idx = extAppId.IndexOf("-");
            string appNumber = idx > 0 ? extAppId.Substring(idx + 1, extAppId.Length - idx - 1) : "";
            AppPart = zoneType + appNumber;
            Whole = $"{NamingConvention.AppZone}{zoneType}{appNumber}";
        }

        public void ConvertAreaToAppRoleFixedPart(string areaIdString)
        {
            FixedPart = ConvertAreaToAppRole(areaIdString, NamingConvention);
        }

        public static string ConvertAreaToAppRole(string areaIdString, ModellingNamingConvention namingConvention)
        {
            if (areaIdString.Length >= namingConvention.FixedPartLength)
            {
                return areaIdString.Substring(0, namingConvention.FixedPartLength).Remove(0, namingConvention.NetworkAreaPattern.Length).Insert(0, namingConvention.AppRolePattern);
            }
            return areaIdString;
        }

        public static string ConvertAppRoleToArea(string appRoleIdString, ModellingNamingConvention namingConvention)
        {
            int convLength = namingConvention.AppRolePattern.Length > namingConvention.FixedPartLength ? namingConvention.FixedPartLength : namingConvention.AppRolePattern.Length;
            if (appRoleIdString.Length >= namingConvention.FixedPartLength)
            {
                return appRoleIdString.Substring(0, namingConvention.FixedPartLength).Remove(0, convLength).Insert(0, namingConvention.NetworkAreaPattern);
            }
            return "";
        }


        private int AppPartEnd()
        {
            return Whole.IndexOf(separator);
        }

        private bool AppPartExisting()
        {
            return AppPartEnd() > NamingConvention.FixedPartLength && Whole.Length >= AppPartEnd();
        }

        private string FillFixedIfNecessary(string idString, string filler = " ")
        {
            if (idString.Length < NamingConvention.FixedPartLength)
            {
                int positionsToFill = NamingConvention.FixedPartLength - idString.Length;
                for (int i = 0; i < positionsToFill; i++)
                {
                    idString += filler;
                }
            }
            return idString;
        }
    }
}
