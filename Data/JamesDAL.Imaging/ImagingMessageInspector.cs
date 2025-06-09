using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace James.Data.Imaging
{
    public class ImagingMessageInspector :IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            Debug.WriteLine("In AfterReceiveRequest handler");
        }

        public object? BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            Debug.WriteLine("In BeforeSendRequest handler");
            var headers = request.Headers;
            var data = request.GetBody<string>();
            Debug.WriteLine($"Outgoing Message:\r\nHeaders:\r\n{string.Join("\r\n",headers.Select(h=>$"Name: {h.Name}, Namespace: {h.Namespace}, Actor: {h.Actor}, MustUnderstand: {h.MustUnderstand}, IsReferenceParameter: {h.IsReferenceParameter}"))}\r\nBody:\r\n{data}");
            return null;
        }
    }

    public class InspectorEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // No implementation necessary 
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new ImagingMessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // No implementation necessary 
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // No implementation necessary 
        }
    }
    // Configuration element
    //public class SimpleBehaviorExtensionElement : BindingElement
    //{
    //    public override Type BehaviorType
    //    {
    //        get { return typeof(SimpleEndpointBehavior); }
    //    }

    //    protected override object CreateBehavior()
    //    {
    //        // Create the  endpoint behavior that will insert the message  
    //        // inspector into the client runtime  
    //        return new SimpleEndpointBehavior();
    //    }

    //    public override BindingElement Clone()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override T GetProperty<T>(BindingContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
