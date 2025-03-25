public class ApprovalItem
{
    public int ItemId { get; set; }
    // "Registration", "HouseholdTransfer", "HouseholdSeparation"
    public string ItemType { get; set; }
    // The name of the user who created the item
    public string CreatorName { get; set; }
    // The current status (e.g., "Pending", "ApprovedByLeader", etc.)
    public string Status { get; set; }
    // Hold the underlying object (if needed later for details)
    public object UnderlyingItem { get; set; }
}
