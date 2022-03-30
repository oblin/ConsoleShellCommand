
namespace ConsoleShellCommand
{
    public class BloodResult
    {
        public bool Failed { get; set; }
        public int Status { get; set; }
        public string Version { get; set; }
        public BloodValue? Result { get; set; }
    }

    public class BloodValue
    {
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public int Pulse { get; set; }
        public string Message { get; set; }
    }
}
