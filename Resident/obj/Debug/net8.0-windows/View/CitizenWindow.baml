   M S B A M L   `   `   `  ����C  ?Resident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Resident.ViewModelsResident.ViewModels  5   	   !    Resident.View.CitizenWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0vm!clr-namespace:Resident.ViewModels     5   	     � Title$  Dashboard Citizen��5   	   $	��500��5   	   $	��800��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5	      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� 
�������$��0��6(    �� Padding$ 10q�65   ._�5      �� $��Dashboard Citizen��6   $��24(�5      $
��Bold"�6&   
����  �68   5      � 5   
   $��1��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��* �6#   .+�5      �� $��Thông tin cá nhân��6   $��0��62   $��5q�6B   .��5       �� $
��Auto=�6   .��5!      �� $��10q�6!   .+�5"      �� $��Họ và tên:��6$   $
��Bold"�66   �� 5#      $��0,0,0,5q�6J   ��6$   �� *CurrentUser.FullName+6J   �� 5%      $��Email:��6$   $
��Bold"�62   �� 5&      $��0,0,0,5q�6G   ��6$   �� *CurrentUser.Email+6G   �� 5(      $��CCCD:��6$   $
��Bold"�61   �� 5)      $��0,0,0,5q�6N   ��6$   �� *CurrentUser.IdentityCard+6N   �� 5+      $��Ngày sinh:��6$   $
��Bold"�66   �� 5,      $��0,0,0,5q�6e   ��6$   �� *CurrentUser.Birthday+ �� StringFormat$ 
dd/MM/yyyy��6e   �� 5.      $��Giới tính:��6$   $
��Bold"�66   �� 5/      $��0,0,0,5q�6E   ��6$   �� *CurrentUser.Sex+6E   �� 51      -   5        $��Theo dõi hồ sơ��51   !   $	��150��6:   $��30��6F   $��0,10,0,0q�6R   6d   52      53      54      �� 57      $��1��6   $��5q�6)   $	��Top=�64   .+�58      �� $��Xem thông báo��6   $��40��61   $��0,0,0,10q�6=   ��59      �� *LoadNotificationsCommand+�� 5:      $��Quản lý hộ khẩu��6   $��40��63   $��0,0,0,10q�6?   ��5;      �� *ManageHouseholdCommand+�� 5<      $��Chat với Police Khu vuc��6   $��40��6;   $��0,0,0,10q�6G   ��5=      �� *OpenChatCommand+5>      5?      �� 5B   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5C      �� -   5         ��Name$ 	btnLogout��5C   >   $��Logout��6   $	��100��6&   $��30��62   6>   5D      5E      5F      