using System;

namespace FacetAPI.Runtime.Core
{
    public class ActionFacet<T1> : FacetCallback<Action<T1>>
    {
        public ActionFacet(bool reactive = false)
            : base(reactive)
        {
        }

        public new void Invoke(T1 arg1) => base.Invoke(arg1);
    }

    public class ActionFacet<T1, T2> : FacetCallback<Action<T1, T2>>
    {
        public ActionFacet(bool reactive = false)
            : base(reactive)
        {
        }

        public new void Invoke(T1 arg1, T2 arg2) => base.Invoke(arg1, arg2);
    }
}