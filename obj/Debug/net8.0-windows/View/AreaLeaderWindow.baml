   M S B A M L   `   `   `  ����  Project#    Project.View.AreaLeaderWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Dashboard AreaLeader��5   	   $	��500��6&   $	��800��63    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� 
����ح�$��0��6(    �� Padding$ 10q�65   ._�5      �� $��Dashboard AreaLeader��6   $��24(�5      $
��Bold"�6&   
����  �68   5      � 5   
   $��1��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��* �6#   .+�5      �� $��Hồ sơ cần duyệt��6   $��0��60   $��5q�6@   .��5      �� -   5        $��lvApplications��5       �� View 5      �� 5      .P�5      �� $��Mã hồ sơ��6-    �� Width$ 80��6?    �� DisplayMemberBinding 6J   �� *ApplicationId+�� 5      $��	Họ tên��6-   $	 150��6=    6I   �� *
FullName+�� 5      $��Trạng thái��6-   $	 100��6A    6M   �� *Status+5      5!      5"      �� 5$      $��1��6   $��5q�6)   $	��Top=�64   .+�5%      �� $��Duyệt hồ sơ��6   $��40��6/   $��0,0,0,10q�6;   �� 5&      $$��Gửi nhận xét cho Công an��6   $��40��6<   $��0,0,0,10q�6H   �� 5'      $��Xem thông báo��6   $��40��61   $��0,0,0,10q�6=   �� 5(      $
��Chat��6   $��40��6(   $��0,0,0,10q�64   5)      5*      �� 5,   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5-      �� -   5        	 ��Name$	 	btnLogout��5-   >   $��Logout��6   $	��100��6&   $��30��62   6>   5.      5/      50      