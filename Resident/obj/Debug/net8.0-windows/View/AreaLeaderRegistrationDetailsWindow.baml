   M S B A M L   `   `   `  ����  Resident7    1Resident.View.AreaLeaderRegistrationDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$4  .Chi tiết hồ sơ (Tổ trưởng khu phố)��5   	   $	��400��5   	   $	��600��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6   .+�5   
   �� .+�5	      �� $*��$Chi tiết hồ sơ (Tổ trưởng)��6   $��20(�5
      $
��Bold"�5      $��0,0,0,10q�5      �� 5       �� Orientation$ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $��Mã hồ sơ:��6   $
��Bold"�6-   $	��120��6?   �� 5      ��6   �� *Registration.RegistrationId+5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $��
Họ tên:��6   $
��Bold"�6+   $	��120��6=   �� 5      ��6   �� *Registration.User.FullName+5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $��Loại hồ sơ:��6   $
��Bold"�6/   $	��120��6A   �� 5      ��6   �� *Registration.RegistrationType+5      �� 5!      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5"      �� $��Trạng thái:��6   $
��Bold"�6/   $	��120��6A   �� 5#      ��6   �� *Registration.Status+5$      �� 5'      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5(      �� $��Ngày đăng ký:��6   $
��Bold"�61   $	��120��6C   �� 5)      ��6   �� *Registration.StartDate+ �� StringFormat$ {0:yyyy-MM-dd}��5*      �� 5-      $ 
Horizontal=�6   $��0,20,0,0q�62   $��Center=�6D   .+�5.      �� $��Duyệt sơ bộ��6   $	��100��6/   $��5q�6;   ��5/      �� *ApproveCommand+�� 50      $��Từ chối��6   $	��100��6+   $��5q�67   ��51      �� *RejectCommand+52      �� 54      -   5        $��Đóng��54      $	��100��6$   $��0,20,0,0q�60   $��Right=�6B   55      56      57      58      