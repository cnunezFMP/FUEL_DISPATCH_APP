using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.Utils.Constants
{
    public struct ValidationConstants
    {

        public const decimal ZeroGallons = 0;
        public const string InactiveStatus = "Inactive";
        public const string ActiveStatus = "Active";
        public const string CompletedStatus = "Completed";
        public const string PendingStatus = "Pending";
        public const string RejectedStatus = "Rejected";
        public const string ApprovedStatus = "Approved";
        public const string NotAvailableStatus = "Not Available";
        public const string CanceledStatus = "Canceled";
        public const int CreditCardMethod = 1;
        public const int GallonsMethod = 2;
        public const int TickecMethod = 3;
    }
}
