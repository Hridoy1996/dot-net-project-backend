namespace Domains.ResponseDataModels
{
    public class AppointmentHistoryResponse
    {
        public AppointmentHistoryResponse()
        {
            AppointmentDetailsList = new List<AppointmentDetails>();
        }

        public long TotalCount { get; set; }
        public List<AppointmentDetails> AppointmentDetailsList{ get; set; }

    }
}
