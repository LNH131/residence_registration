   M S B A M L   `   `   `  ����  Resident2    ,Resident.View.HouseholdTransferDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Chi tiết chuyển hộ��5   	   $	��400��5   	   $	��600��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Chi tiết chuyển hộ��5      $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      �� 5   
   $��1��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $��Transfer ID:��6   $
��Bold"�60   $	��150��6B   �� 5      ��6   �� *Transfer.TransferId+5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5       �� $��Household ID:��6   $
��Bold"�61   $	��150��6C   �� 5!      ��6   �� *Transfer.HouseholdId+5"      �� 5%      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5&      �� $��From Address:��6   $
��Bold"�61   $	��150��6C   �� 5'      ��6   �� *Transfer.FromAddressId+5(      �� 5)      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5*      �� $��To Address:��6   $
��Bold"�6/   $	��150��6A   �� 5+      ��6   �� *Transfer.ToAddressId+5,      �� 5/      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�50      �� $��Status:��6   $
��Bold"�6+   $	��150��6=   �� 51      ��6   �� *Transfer.Status+52      �� 55      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�56      �� $��	Comments:��6   $
��Bold"�6-   $	��150��6?   �� 57      ��6   �� *Transfer.Comments+58      59      �� 5<   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5=      �� $��Approve��6   $	��100��6'   $��5q�63   ��5>      �� *ApproveCommand+{�5?      �� *	CanModify+�� 5@      $��Reject��6   $	��100��6&   $��5q�62   ��5A      �� *RejectCommand+{�5B      �� *	CanModify+�� 5C      -   5        $��Close��5C      $	��100��6%   $��5q�61   6<   5D      5E      5F      