using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class FamilyModel
    {
        public int FamilyId { get; set; }

        public string FamilyName { get; set; } = null!;
        
    }
    public class FamilyMemberModel
    {
        public int FamilyId { get; set; }

        public string FamilyName { get; set; } = null!;
        public List<UserProfileModel>? FamilyMembers { get; set; }
    }

    public partial class FamilyMemberRequestModel
    {
        public int FamilyMemberRequestId { get; set; }

        public int? RequestedUserId { get; set; }

        public string UserMessage { get; set; } = null!;

        public string FamilyEmailIds { get; set; } = null!;

        public bool? IsEmailSent { get; set; }

        public bool? IsProcessed { get; set; }
    }
}
