using Xamarin.Forms.Maps;

namespace DWPennyFinder
{
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Machine { get; set; }
        public string Location { get; set; }
        public int MachineID { get; set; }
    }
}
