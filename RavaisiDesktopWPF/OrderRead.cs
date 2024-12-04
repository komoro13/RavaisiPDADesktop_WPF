using Newtonsoft.Json;
using System.Windows;

namespace RavaisiDesktopWPF
{
    class OrderRead
    {
        public dynamic JSON(string orderJSON)
        {
             dynamic jsonObject = JsonConvert.DeserializeObject(orderJSON);             
             return jsonObject;
        }
    }
}
