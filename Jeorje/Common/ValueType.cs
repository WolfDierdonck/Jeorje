using System;

namespace Jeorje
{
    public class ValueType : PredicateType, IEquatable<PredicateType>
    {
        public string TypeName;

        public ValueType(string typeName)
        {
            TypeName = typeName;
        }

        public bool Equals(PredicateType type)
        {
            if (type is null || type is not ValueType)
            {
                return false;
            }

            return ((ValueType) type).TypeName == TypeName;
        }
        
        public static bool operator ==(ValueType lhs, ValueType rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(ValueType lhs, ValueType rhs) => !(lhs == rhs);
    }
}