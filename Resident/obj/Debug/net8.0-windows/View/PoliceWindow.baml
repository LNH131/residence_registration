   M S B A M L   `   `   `  ����  Resident     Resident.View.PoliceWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Dashboard Police��5   	   $	��600��5   	   $	��900��5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5	      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
����İ� �� Padding$ 10q�6:   ._�5      �� $��Dashboard Police��6   $��24(�5      $
��Bold"�5      
����  �5      5      � 5   
   $��1��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��3* �6#   �� 5!      $��* �6#   .+�5&      �� $��0��6    �� Orientation$ Vertical=�6)   $��5q�6@   .+�5(      �� $"��Approvals (ApprovedByLeader)��6   $��0,0,0,10q�6A   .��5)      �  5*      �� 5+      $��* �6,   �� 5,      $
��Auto �6,   .+�5.      &   System.Windows.Controls.DataGrid  $��0��6#      AutoGenerateColumns$ False��51         
IsReadOnly$
 True��52         SelectionMode$ Single=�53      	   SelectionUnit$	 FullRow=�54      $��5q�55      ��5/      �� *ApprovalItems+
 �� SelectedItem
 50      �� *SelectedApprovalItem+ �� Mode$ TwoWay=�   Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57   "   ,  &System.Windows.Controls.DataGridColumn   Header$ ID��65      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6\   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6A   �� *ItemId+6\     58   "   $
 Type��65   $	 120 6`    6C   �� *
ItemType+6`     59   "   $ Creator��65   $	 150 6f    6F   �� *CreatorName+6f     5:   "   $ Status��65   $	 100 6`    6E   �� *Status+6`   5<      �� 5>      $��1��6%   $ 
Horizontal=�62   $��Right=�6K   $��5q�6g   .+�5?      �� $��Process��6%   $��5q�6Z   ��67   �� *ProcessCommand+6Z   �� 5@      $��View Detail��6%   $��5q�6b   ��6;   �� *ViewDetailsCommand+6b   �� 5A      $��Refresh��6%   $��5q�6Z   ��67   �� *RefreshCommand+6Z   5B      5C      5D      �� 5G      $��Household Monitoring��6   .��5H        $ False��5J      $
 True��5K      $��5q�5L      ��5H      �� *
Households+
 5I      �� *SelectedHousehold+ 5M        5N      $ ID��61   $
 Auto 6]    6=   �� *HouseholdId+6]     5O      $ Street��61   $ * 6d    6A   �� *Address.Street+6d     5P      $
 City��61   $ * 6`    6?   �� *Address.City+6`     5Q      $ Created Date��61   $
 Auto 6g    6G   �� *CreatedDate+6g   5S      5T      5U      �� 5X      $��1��6   $ Vertical=�6)   $��Right=�6@   $��5q�6\   .+�5Y      �� $
��Chat��6   $��40��6(   $��0,0,0,10q�64   ��6F   �� *ChatCommand+�� 5Z      $��View Household Detail��6   $��40��69   $��0,0,0,10q�6E   ��6W   �� *ViewHouseholdDetailCommand+�� 5[      $��Notifications��6   $��40��61   $��0,0,0,10q�6=   ��6O   �� *NotificationCommand+�� 5\      $��View All Approvals��6   $��40��66   $��0,0,0,10q�6B   ��6T   �� *ViewAllRegistrationsCommand+5]      5^      �� 5a   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5c      �� -   5         ��Name$ 	btnLogout��5c   >   $��Logout��6   $	��100��6&   $��30��62   6Q   5d      5e      5f      