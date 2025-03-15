﻿using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class GymMembershipPlanPackage
    {
        [Key]
        public int GymMembershipPlanPackageId { get; set; }
        public int GymMembershipPlanId { get; set; }
        public long InventoryGeneralItemLineId { get; set; }
        public decimal ServiceCost { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

