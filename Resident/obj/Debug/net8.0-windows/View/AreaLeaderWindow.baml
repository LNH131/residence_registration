   M S B A M L   `   `   `  ����C  ?Resident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Resident.ViewModelsResident.ViewModels  5   	   $    Resident.View.AreaLeaderWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   0vm!clr-namespace:Resident.ViewModels     5   	     � Title$  Dashboard AreaLeader��5   	   $	��500��5   	   $	��800��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� 
����ح�$��0��6(    �� Padding$ 10q�65   ._�5      �� $��Dashboard AreaLeader��6   $��24(�5      $
��Bold"�5      
����  �5      5      � 5   
   $��1��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��3* �6#   �� 5      $��* �6#   .+�5       �� $:��4Hồ sơ, chuyển hộ và tách hộ cần duyệt��6   $��0��5!      $��5q�5"      .��5#      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��5$         
IsReadOnly$
 True��5%         SelectionMode$ Single=�5&      ��5#      �� *ApprovalItems+ �� SelectedItem 5'      �� *SelectedApprovalItem+	   Columns	 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 5*   -   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5+   -   �� *ItemId+  5,      $
 Loại��6-   $	 120 5-   -    5.   -   �� *
ItemType+  5/      $
 Người tạo��6-   $	 150 50   -    51   -   �� *CreatorName+  52      $
 Trạng thái��6-   $	 100 53   -    54   -   �� *Status+4  .System.Windows.Controls.DataGridTemplateColumn  55      $
 Hành động��61   $	 120 6D      CellTemplate 56      �� 57   "   .R�58   &   �� $��Xem chi tiết��6-   ��59   -   �� * DataContext.ViewDetailsCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5;   -   �� 5<   #   5>      5@      5A      �� 5D      $��1��6   $��5,5,5,0q�6)   $	��Top=�6:   .+�5E      �� $1��+Gửi thông báo đến tất cả Citizen��6   $��40��5F      $��0,0,0,10q�5G      ��5H      �� *OpenNotificationWindowCommand+�� 5I      $
��Chat��6   $��40��5J      $��0,0,0,10q�5K      ��5L      �� *ChatCommand+�� 5M      $��Xem tất cả hồ sơ��6   $��40��5N      $��0,0,0,10q�5O      ��5P      �� *ViewAllRegistrationsCommand+�� 5Q      $&�� Xem tổng quan duyệt hồ sơ��6   $��40��5R      $��0,0,0,10q�5S      ��5T      �� *ViewApprovalsOverviewCommand+5U      5V      �� 5Y   
   $��2��6    �� Orientation$ 
Horizontal=�5Z      $��Right=�5[      $��10q�5\      .+�5]      �� -   5         ��Name$ 	btnLogout��5`      $��Logout��5]      $	��100��5^      $��30��5_      5a      5b      5c      5d      