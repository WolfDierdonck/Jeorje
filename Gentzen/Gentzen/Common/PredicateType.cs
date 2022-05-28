using System;

namespace Gentzen.Gentzen.Common
{
    public abstract class PredicateType : ICloneable
    {
        public abstract object Clone();
        public abstract bool IsNull();
    }
}