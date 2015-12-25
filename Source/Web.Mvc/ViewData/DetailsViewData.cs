using System;

namespace ReusableLibrary.Web.Mvc
{
    [Serializable]
    public class DetailsViewData<TDetails>
        where TDetails : new()
    {
        public DetailsViewData()
        {
            Details = new TDetails();
        }

        public string Message { get; set; }

        public TDetails Details { get; set; }
    }
}
