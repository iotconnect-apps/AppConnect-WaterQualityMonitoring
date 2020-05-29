namespace iot.solution.entity.Response
{
    public class EntityDetailResponse
    {
        public int Temperature { get; set; }
        public double pHLevel { get; set; }
        public int Salinity { get; set; }
        public int TDS { get; set; }
        public double Turbidity { get; set; }
        public int Conductivity { get; set; }
        public int Chloride { get; set; }
        public int Nitrate { get; set; }
        public int BOD { get; set; }
        public int WQI { get; set; }
        public int TotalAlerts { get; set; }
       // public int TotalDevices { get; set; }
    }
}
