namespace FWO.Data.Report
{
    public class CustomVersion : IComparable<CustomVersion>
    {
        public List<int> Parts { get; }

        public CustomVersion(string versionString)
        {
            if(string.IsNullOrWhiteSpace(versionString))
                throw new ArgumentException("Version string cannot be null or empty.");

            Parts = [.. versionString
                .Split('.')
                .Select(p => int.TryParse(p, out int n) ? n : throw new FormatException("Invalid version part: " + p))];
        }

        public static int GetDeepnessLevel(string versionString) => new CustomVersion(versionString).Parts.Count;
        
        public int CompareTo(CustomVersion other)
        {
            int maxLength = Math.Max(Parts.Count, other.Parts.Count);

            for(int i = 0; i < maxLength; i++)
            {
                int a = i < Parts.Count ? Parts[i] : 0;
                int b = i < other.Parts.Count ? other.Parts[i] : 0;

                if(a != b)
                    return a.CompareTo(b);
            }

            return 0;
        }

        public override string ToString() => string.Join(".", Parts);

        public override bool Equals(object obj)
        {
            return obj is CustomVersion other && CompareTo(other) == 0;
        }

        public override int GetHashCode() => string.Join(".", Parts).GetHashCode();

        public CustomVersion GetPrefix(int count)
        {
            if(count <= 0)
                throw new ArgumentException("Count must be positive.");

            List<int>? prefixParts = Parts.Take(count).ToList();
            return new CustomVersion(string.Join(".", prefixParts));
        }

        public bool EqualsUpTo(CustomVersion other, int partsToCompare)
        {
            for(int i = 0; i < partsToCompare; i++)
            {
                int a = i < Parts.Count ? Parts[i] : 0;
                int b = i < other.Parts.Count ? other.Parts[i] : 0;
                if(a != b)
                    return false;
            }
            return true;
        }

        public static bool operator >(CustomVersion a, CustomVersion b) => a.CompareTo(b) > 0;
        public static bool operator <(CustomVersion a, CustomVersion b) => a.CompareTo(b) < 0;
        public static bool operator ==(CustomVersion a, CustomVersion b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(CustomVersion a, CustomVersion b) => !(a == b);
    }

}
