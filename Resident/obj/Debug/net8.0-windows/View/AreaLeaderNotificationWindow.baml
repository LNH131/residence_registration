   M S B A M L   `   `   `  ����  Resident0    *Resident.View.AreaLeaderNotificationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  View Notifications��5   	   $	��400��6$   $	��600��61    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $!��Notifications for Your Area��6   $��20(�5      $
��Bold"�6"   $��0,0,0,10q�64   $��0��5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��6/   $��0,0,0,10q�5      ��5       �� *Notifications+ �� SelectedItem 5      �� *SelectedNotification+ �� Mode$ TwoWay=�   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 50 6T   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *NotificationId+6T     5      $ Message��6%   $	 * 6R   
 66   �� *	Message+6R     5      $ 	Sent Date��6%   $		 150 5   X   
 6%   �� *
SentDate+ �� StringFormat$ {0:G}��6X   4  .System.Windows.Controls.DataGridCheckBoxColumn  5      $
 Read��6)   $	 60 6R   
 67   �� *IsRead+6R   5      �� 5   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� $��Mark as Read��6   $	��100��6,   $��5q�68   ��6C   �� *MarkAsReadCommand+�� 5       $��Refresh��6   $	��100��6'   $��5q�63   ��6>   �� *RefreshCommand+�� 5!      $��Close��6   $	��100��6%   $��5q�61   ��6<   �� *CloseCommand+5"      5#      5$      