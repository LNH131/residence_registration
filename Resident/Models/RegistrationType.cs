namespace Resident.Enums
{
    public enum RegistrationType
    {
        HouseholdRegistration,  // Đăng ký hộ khẩu (mới)
        TemporaryResidence,     // Đăng ký tạm trú
        TemporaryAbsence,      // Đăng ký tạm vắng
        // Add other registration types as needed
        HouseholdModification,  //Thay đổi thông tin hộ khẩu (thêm/xóa thành viên)
        AddressChange
    }
}
