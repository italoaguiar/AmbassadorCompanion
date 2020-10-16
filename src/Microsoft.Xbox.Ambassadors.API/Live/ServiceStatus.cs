using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Microsoft.Xbox.Live
{

    [XmlRoot(ElementName = "ServiceStatus")]
    public class ServiceStatus
    {
        private ServiceStatus()
        {

        }
        public static async Task<ServiceStatus> GetStatusAsync(string region = "US")
        {
            
            Uri url = new Uri("https://xnotify.xboxlive.com/servicestatusv4/" + region);
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (HttpClient http = new HttpClient())
                {
                    using (HttpResponseMessage response = await http.SendAsync(msg))
                    {

                        XmlSerializer ser = new XmlSerializer(typeof(ServiceStatus));
                        string r = await response.Content.ReadAsStringAsync();

                        using (StringReader sr = new StringReader(r))
                        {
                            return (ServiceStatus)ser.Deserialize(sr);
                        }
                    }
                }
            }
            
        }


        [XmlElement(ElementName = "Status")]
        public StatusElement Status { get; set; }
        [XmlElement(ElementName = "CoreServices")]
        public ServiceInfo CoreServices { get; set; }
        [XmlElement(ElementName = "Apps")]
        public ServiceInfo Apps { get; set; }
        [XmlElement(ElementName = "Games")]
        public ServiceInfo Games { get; set; }
        [XmlElement(ElementName = "Website")]
        public Category Website { get; set; }
    }

    public class StatusElement
    {
        [XmlElement(ElementName = "State")]
        public string State { get; set; }
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "LastUpdated")]
        public string LastUpdated { get; set; }
    }

    public class Status
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
    }

    public class Category
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Status")]
        public Status Status { get; set; }
        [XmlElement(ElementName = "Scenarios")]
        public Scenarios Scenarios { get; set; }
    }


    public class ServiceInfo
    {
        [XmlElement(ElementName = "Category")]
        public List<Category> Category { get; set; }

        public Category this[string name] => Category.FirstOrDefault(x=> x.Name == name);
    }



    [XmlRoot(ElementName = "Device")]
    public class Device
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Devices")]
    public class Devices
    {
        [XmlElement(ElementName = "Device")]
        public List<Device> Device { get; set; }
    }

    [XmlRoot(ElementName = "Stage")]
    public class Stage
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }

    [XmlRoot(ElementName = "ImpactedDevices")]
    public class ImpactedDevices
    {
        [XmlElement(ElementName = "DeviceId")]
        public string DeviceId { get; set; }
    }

    [XmlRoot(ElementName = "Incident")]
    public class Incident
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Begin")]
        public string Begin { get; set; }
        [XmlElement(ElementName = "Stage")]
        public Stage Stage { get; set; }
        [XmlElement(ElementName = "ImpactedDevices")]
        public ImpactedDevices ImpactedDevices { get; set; }
    }

    [XmlRoot(ElementName = "Incidents")]
    public class Incidents
    {
        [XmlElement(ElementName = "Incident")]
        public Incident Incident { get; set; }
    }

    [XmlRoot(ElementName = "Scenario")]
    public class Scenario
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Devices")]
        public Devices Devices { get; set; }
        [XmlElement(ElementName = "Incidents")]
        public Incidents Incidents { get; set; }
    }

    [XmlRoot(ElementName = "Scenarios")]
    public class Scenarios
    {
        [XmlElement(ElementName = "Scenario")]
        public Scenario Scenario { get; set; }
    }





}
