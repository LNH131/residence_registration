namespace Resident.Models
{
    public class ApprovalItem
    {
        public int ItemId { get; set; }
        // Type of the item: e.g., "Registration", "HouseholdTransfer", "HouseholdSeparation"
        public string ItemType { get; set; }
        // Name of the user who created the item
        public string CreatorName { get; set; }
        // Current status of the item
        public string Status { get; set; }
        // You can store the underlying object if needed
        public object UnderlyingItem { get; set; }
    }
}
