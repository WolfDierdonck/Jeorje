using System;

namespace Gentzen.Gentzen.Common
{
    public class ValueType : PredicateType, IEquatable<PredicateType>, ICloneable
    {
        public string TypeName;

        public ValueType(string typeName)
        {
            TypeName = typeName;
        }

        public override bool IsNull()
        {
            return !string.IsNullOrEmpty(TypeName);
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
        
        public override object Clone()
        {
            return new ValueType(TypeName);
        }

        public override string ToString()
        {
            return TypeName;
        }
    }
}