using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace onvif
{
    public class MulticastCapabilitiesBindingElement : BindingElement, IBindingMulticastCapabilities
    {
        private bool isMulticast;
        public MulticastCapabilitiesBindingElement(bool isMulticast)
        {
            this.isMulticast = isMulticast;
        }
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(IBindingMulticastCapabilities))
            {
                return (T)(object)this;
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.GetInnerProperty<T>();
        }
        bool IBindingMulticastCapabilities.IsMulticast
        {
            get { return isMulticast; }
        }

        public override BindingElement Clone()
        {
            return this;
        }
    }
}
