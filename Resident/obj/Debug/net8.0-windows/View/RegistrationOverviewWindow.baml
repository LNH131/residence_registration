   M S B A M L   `   `   `  ����  Resident.    (Resident.View.RegistrationOverviewWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$'  !Tất cả hồ sơ chờ duyệt��5   	   $	��450��6(   $	��800��65    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $��Danh sách hồ sơ��6   $��20(�6+   $
��Bold"�69   $��0,0,0,10q�6K   $��0��6]   &   System.Windows.Controls.DataGrid  5   
      AutoGenerateColumns$ False��69   $��1��6U   $��0,0,0,10q�6b      
IsReadOnly$
 True��5         SelectionMode$ Single=�6%   ��5      �� *ApprovalItems+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6L   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 61   �� *ItemId+6L     5      $ Loại��6%   $	 150 6P   	 63   �� *
ItemType+6P     5      $ Người tạo��6%   $	 200 6X   	 68   �� *CreatorName+6X     5      $ Trạng thái��6%   $	 150 6T   	 69   �� *Status+6T   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Hành động��6)   $	 120 6<   
   CellTemplate
 5      �� 5      .R�5      �� $��Xem chi tiết��6%   ��6<   �� * DataContext.ViewDetailsCommand+ �� RelativeSource ��  �� AncestorType
 Window��5   %   �� 5      5      5      �� 5!   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��2��6J   .+�5"      �� $��
Làm mới��6   $	��100��6'   $��0,0,10,0q�63   ��6E   �� *RefreshCommand+�� 5#      -   5        $��Đóng��5#      $	��100��6$   60   5$      5%      5&      