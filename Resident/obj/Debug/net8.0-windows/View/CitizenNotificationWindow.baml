   M S B A M L   `   `   `  ����  Resident-    'Resident.View.CitizenNotificationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Thông báo của bạn��5   	   $	��450��5   	   $	��600��5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $ ��Thông báo cho công dân��5      $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      �� 5   
   $��1��6   $��	0,10,0,10q�5      ��5      �� *Notifications+ �� SelectedItem 5      �� *SelectedNotification+ �� View 5      �� 5      .P�5      �� $��
Nội dung��6%    �� Width$	 300��67    �� DisplayMemberBinding 5   %   �� *	Message+�� 5       $��Ngày��6%    5!   %   �� *
SentDate+�� 5"      $��Đã đọc?��6%    5#   %   �� *IsRead+5$      5&      �� 5)   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5*      �� $��Tải lại��6   $��80��5+      $��5q�5,      ��5-      �� *LoadNotificationsCommand+�� 5.      $��Đánh dấu đã đọc��6   $	��120��5/      $��5q�50      ��51      �� *MarkAsReadCommand+�� 52      -   5        $��Đóng��52      $��80��53      $��5q�54      55      56      57      58      