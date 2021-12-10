using System;

namespace Jeorje
{
    public abstract class PredicateType : ICloneable
    {
        public abstract object Clone();
        public abstract bool IsNull();
    }
}