   M S B A M L   `   `   `  ����  Resident4    .Resident.View.HouseholdSeparationDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Chi tiết tách hộ��5   	   $	��500��5   	   $	��700��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Chi tiết tách hộ��5      $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��2* �6#   �� 5      $��* �6#   .+�5      �� $��0��6   $��0,0,10,0q�6)   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,5q�66   .+�5      �� $��Separation ID:��6    $
��Bold"�66   $	��150��6H   �� 5       ��6    �� *Separation.SeparationId+5!      �� 5"      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5#      �� $��Original Household:��6    $
��Bold"�6;   $	��150��6M   �� 5$      ��6    �� * Separation.OriginalHouseholdId+5%      �� 5&      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5'      �� $��New Household:��6    $
��Bold"�66   $	��150��6H   �� 5(      ��6    �� *Separation.NewHouseholdId+5)      �� 5*      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5+      �� $��Trạng thái:��6    $
��Bold"�63   $	��150��6E   �� 5,      ��6    �� *Separation.Status+5-      �� 5.      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�5/      �� $��Requested Date:��6    $
��Bold"�67   $	��150��6I   �� 50      ��6    �� *Separation.RequestDate+ �� StringFormat$ {0:yyyy-MM-dd HH:mm}��51      �� 52      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�53      �� $��Approved By:��6    $
��Bold"�64   $	��150��6F   �� 54      ��6    �� *Separation.ApprovedBy+55      �� 56      $ 
Horizontal=�6   $��0,0,0,5q�66   .+�57      �� $��Approval Date:��6    $
��Bold"�66   $	��150��6H   �� 58      ��6    �� *Separation.ApprovalDate+$ {0:yyyy-MM-dd HH:mm}��59      5:      �� 5=      $��1��6   .+�5>      �� $ ��Original Household Members��6   $��0,0,0,10q�6?   .��5?      ��  �� DisplayMemberPath$ FullName��6O   ��6   �� *OriginalHouseholdMembers+6O   5@      �� 5B      $��New Household Members��6   .��5C      �� $ FullName��6J   ��6   �� *NewHouseholdMembers+6J   5D      5E      5F      �� 5I   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5J      �� $��Approve��6   $	��100��6'   $��5q�63   ��5K      �� *ApproveCommand+{�5L      �� *	CanModify+�� 5M      $��Reject��6   $	��100��6&   $��5q�62   ��5N      �� *RejectCommand+{�5O      �� *	CanModify+�� 5P      -   5        $��Close��5P      $	��100��6%   $��5q�61   6<   5Q      5R      5S      