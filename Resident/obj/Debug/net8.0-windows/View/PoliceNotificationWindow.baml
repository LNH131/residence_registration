   M S B A M L   `   `   `  ����  Resident,    &Resident.View.PoliceNotificationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Thông báo cho Cảnh sát��5   	   $	��400��5   	   $	��600��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $!��Thông báo cho Cảnh sát��6!   $��20(�5      $
��Bold"�6"   $��0,0,0,10q�5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         SelectionMode$ Single=�5      $��0,5,0,5q�5      ��5      �� *Notifications+ �� SelectedItem 5      �� *SelectedNotification+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5       ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 5"   %   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 5!   %   �� *NotificationId+5"   %     5#      $ 
Nội dung��6%   $	 * 5%   %   
 5$   %   �� *	Message+5%   %     5&      $ Ngày gửi��6%   $		 150 5(   %   
 5'   %   �� *
SentDate+5(   %   4  .System.Windows.Controls.DataGridCheckBoxColumn  5)      $ Đã đọc?��6)      
IsReadOnly$
 True��5+   )   $	 80 5,   )   
 5*   )   �� *IsRead+5,   )   5.      �� 51   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�52      �� $��Tải lại��6   $��5q�53      ��54      �� *LoadNotificationsCommand+�� 55      $��Đánh dấu đã đọc��6   $��5q�56      ��57      �� *MarkAsReadCommand+�� 58      $��Đóng��6   $��5q�59      ��5:      �� *CloseWindowCommand+5;      5<      5=      