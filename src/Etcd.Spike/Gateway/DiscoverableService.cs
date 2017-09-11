using System;

namespace Gateway
{
    public class DiscoverableService : IEquatable<DiscoverableService>
    {
        public string Type { get; }
        public string Address { get; }

        private DiscoverableService(string Type, string Address)
        {
            this.Type = Type;
            this.Address = Address;
        }

        public static DiscoverableService CreateFromEtcdKey(string etcdKey)
        {
            var splitted = etcdKey.Split('|');
            var type = splitted[1];
            var address = splitted[2];
            return new DiscoverableService(type, address);
        }

        public bool Equals(DiscoverableService other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return this.Address == other.Address && this.Type == other.Type;
        }

        public override bool Equals(object value)
        {

            var other = value as DiscoverableService;
            return Equals(other);
        }

        public static bool operator ==(DiscoverableService a, DiscoverableService b)
        {
            if(Object.ReferenceEquals(a, null))
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(DiscoverableService a, DiscoverableService b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + Type.GetHashCode();
            hash = (hash * 7) + Address.GetHashCode();
            return hash;
        }
    }
}
