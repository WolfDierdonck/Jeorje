using System;
using System.Collections.Generic;
using System.Linq;

namespace Gentzen.Gentzen.Common
{
    public class FunctionType : PredicateType, IEquatable<PredicateType>, ICloneable
    {

        public List<PredicateType> ParamTypes;
        public PredicateType ReturnType;

        public FunctionType(List<PredicateType> paramTypes, PredicateType returnType)
        {
            ParamTypes = paramTypes;
            ReturnType = returnType;
        }

        public override bool IsNull()
        {
            return (ReturnType == null || ReturnType.IsNull()) && 
                   (ParamTypes == null || ParamTypes.All(paramType => paramType == null || paramType.IsNull()));
        }
        
        public bool Equals(PredicateType type)
        {
            if (type is null || type is not FunctionType)
            {
                return false;
            }

            return ((FunctionType) type).ParamTypes.SequenceEqual(ParamTypes) && ((FunctionType) type).ReturnType == ReturnType;
        }
        
        public static bool operator ==(FunctionType lhs, FunctionType rhs)
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
        
        public static bool operator !=(FunctionType lhs, FunctionType rhs) => !(lhs == rhs);
        
        public override object Clone()
        {
            return new FunctionType(ParamTypes?.Select(type => (PredicateType) type?.Clone()).ToList(), 
                (PredicateType) ReturnType?.Clone());
        }

        public override string ToString()
        {
            return $"{string.Join(", ", ParamTypes)} --> {ReturnType}";
        }
    }
}