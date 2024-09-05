using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Imaging;
using System.Web.Services.Description;
using James.Shared.Server.Kong0;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Versioning;
using System.ServiceModel.Channels;
using James.Shared;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using WcfCoreMtomEncoder;

namespace James.Data.Imaging
{
    public class ImagingKong0Helper(IHttpClientFactory httpClientFactory, IKongCredentialCache credentialCache, ILoggingService loggingService) : Kong0HelperBase(httpClientFactory, credentialCache, loggingService)
    {
        protected override string HttpClientName => "P8FileNetTokens";
        private CustomBinding? _customBinding;

        protected override CustomBinding? CustomBinding
        {
            get
            {
                if (null == _customBinding)
                {
                    //Set the P8 soap service client binding options using mtom enabling larger file transfer and setting reasonable timeouts and buffer options
                    var customP8SoapServiceBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
                        {
                            ReceiveTimeout = new TimeSpan(0, 2, 0),
                            SendTimeout = new TimeSpan(0, 2, 0),
                            MaxBufferSize = 314572800,
                            MaxReceivedMessageSize = 314572800
                        };

                    //Get the message encoding type and the binding elements of the newly created custom binding.
                    //retreive all non encoding elements first, then our encoding element, then create an mtom encoding element from that.
                    //finally, prepend our new mtom to the elements without encoding in a new custom binding
                    var messageEncodingBindingElementType = typeof(MessageEncodingBindingElement);
                    var elements = customP8SoapServiceBinding.CreateBindingElements();
                    IEnumerable<BindingElement> elementsWithoutEncodingElement = elements.Where(item => !messageEncodingBindingElementType.IsAssignableFrom(item.GetType()));
                    var existingEncodingElement = (MessageEncodingBindingElement)elements.Where(item => messageEncodingBindingElementType.IsAssignableFrom(item.GetType())).First();
                    var newEncodingElement = new MtomMessageEncoderBindingElement(existingEncodingElement);

                    // Encoding is before transport, so we prepend the MTOM message encoding binding element
                    // https://learn.microsoft.com/en-us/dotnet/framework/wcf/extending/custom-bindings
                    _customBinding = new CustomBinding(elementsWithoutEncodingElement.Prepend(newEncodingElement));
                }
                return _customBinding;
            }
        }
    }

    public static class ImagingConfiguration
    {
        /// <summary>
        /// Setup services for P8 Imaging
        /// </summary>
        /// <param name="services"></param>
        /// <param name="kong0ClientId"></param>
        /// <param name="kong0ClientSecret"></param>
        /// <param name="kong0Audience"></param>
        /// <remarks>Must be called after ILoggingService and ServerDataAccess are added to the dependancy injection container.</remarks>
        [SupportedOSPlatform("windows")]
        public static void SetupImagingForKong(this IServiceCollection services,
            string kong0ClientId, string kong0ClientSecret, string kong0Audience)
        {
            KongTokenRequest.SetRequest(typeof(ImagingKong0Helper), new KongTokenRequest { Audience = kong0Audience, ClientId = kong0ClientId, ClientSecret = kong0ClientSecret });
            services.AddScoped<IImagingAccess, ServerImagingAccess>();
            services.AddSingleton<IKongCredentialCache, KongCredentialCache>();
            services.AddScoped<ImagingKong0Helper>();
        }
    }
}
