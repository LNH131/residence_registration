   M S B A M L   `   `   `  ����C  ?Resident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Resident.ViewResident.View  5   	   !    Resident.View.AddUserWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Resident.View     5   	     � Title$  AddUserWindow��5   	   $	��450��6   $	��800��6,    � 	Resources 5         WindowBackgroundBrush �� 
StartPoint �� EndPoint � Color � Offset �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� CornerRadius �� Triggers P� Property P� Value&   System.Windows.Controls.DataGrid��%�  &	        '�� �     '�� 0    '��     '�� �    '  j    �� 5   
   $	 0,0&�6<   $	 1,1&�6M   .��5      � $ #a1c4fd��6   $	 0.0Q�6+   � 5      $ #c2e9fb��6   $	 1.0Q�6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   �� 5       � �� �#2c3e506+   �� 5       � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ 3q�6(   �� 5       � �� �#B0BEC56,   �� 5       � ��$ 1q�60   �� 5       � ��
 �����6+   5       �� 5#   
   " ��6   .�5$      ��  � ��$ 15(�6)   �� 5%       � ��
 �����6+   �� 5&       � �� �#1abc9c6+   �� 5'       � �� �#16a0856,   �� 5(       � ��$ 1q�60   �� 5)       � ��$	 5,3q�6(   �� 5*       � ��$
 Bold"�6+   �� 5+       � �� 5,      �� 5-      "	 ��6&   .R�5.      �� $
 3��6!   #��z��5/   !   #��z��50   !   #��z��51   !   ._�52      �� $��Center=�6/   $��Center=�6L   53       54      P� 55       �@J�IsMouseOver$
 True��6=   . �56   "   ��  � �� �#16a0856?   57      59      5;      5<      �� 5?   
   " ��6   .�5@      ��  � ��$ 15(�6)   �� 5A       � ��$ 3q�6(   �� 5B       � �� �#B0BEC56,   �� 5C       � ��$ 1q�60   �� 5D       � ��
 �����6+   5E      �� 5H   
   "  6   .�5I      ��  � ��$ 15(�6)   �� 5J       � ��
 �����6+   �� 5K       �@ RowBackground �#ECF0F16.   �� 5L        �@ AlternatingRowBackground �#F7F7F769   �� 5M       � �� �#B0BEC56,   �� 5N       � ��$ 1q�60   5O      5P      .��5S      � #��[  6    � ColumnDefinitions 5T   
   �� 5U      $��0* �6   �� 5V      .+�5X   
   �� $��Full name :��6   $
��Left=�6&   $��6,14,0,0q�6A   $	��Top=�6S   $��1��6k   $��30��6{   $��81��6�   �� 5Y   
   $��Email:��6   $
��Left=�6!   $��6,44,0,0q�6<   $	��Top=�6N   $��1��6f   $��30��6v   $��49��6�   �� 5Z   
   $��
Passworld:��6   $
��Left=�6%   $��6,74,0,0q�6@   $	��Top=�6R   $��1��6j   $��30��6z   $��79��6�   �� 5[   
   $��Role : ��6   $
��Left=�6"   $��	6,104,0,0q�6=   $	��Top=�6P   $��1��6h   $��30��6x   $��50��6�   �� 5\   
   -   5         ��Name$ fullName��5\      $
��Left=�6$   $��	84,14,0,0q�6?    �� TextWrapping$
 Wrap=�6R   $	��Top=�6f   $	��186��6~   $��30��6�   $��1��6�   �� 5]   
   -   5        $ email��5]      $
��Left=�6!   $��	84,44,0,0q�6<   $
 Wrap=�6O   $	��Top=�6c   $	��186��6{   $��30��6�   $��1��6�   �� 5^   
   -   5        $ 	passworld��5^      $
��Left=�6%   $��	84,74,0,0q�6@   $
 Wrap=�6S   $	��Top=�6g   $	��186��6   $��30��6�   $��1��6�   �� 5`   
   -   5         ��Name$ 	Add_click��5`      $	��Add��6$   $
��Left=�62   $��102,171,0,0q�6M   $	��Top=�6b   $��1��6�   $��20��6�   $��42��6�   �� 5a   
   -   5        $��Update��5a      $
��Left=�6"   $��144,171,0,0q�6=   $	��Top=�6R   $��1��6   $��20��6�   $��55��6�   �� 5b   
   -   5        $��Delete��5b      $
��Left=�6"   $��200,171,0,0q�6=   $	��Top=�6R   $��1��6�   $��20��6�   $��54��6�   �� 5c   
   -   5        $
��Save��5c      $
��Left=�6    $��456,176,0,0q�6;   $	��Top=�6P   $��52��6h   $��1��6�   $��20��6�   �� 5d   
   -   5        $
��Back��5d      $
��Left=�6    $��656,378,0,0q�6;   $	��Top=�6P   $��1��6h   $��30��6x   $��65��6�   6�     5e   
   -	   5          Name$ dgUser��5e         AutoGenerateColumns$ False��6#   $��275,14,9,263q�6?   $��1��6�      Columns 5f      0  *System.Windows.Controls.DataGridTextColumn  5g      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Full Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
FullName+6U     5h      $ Email��6%   $ * 6N    64   �� *Email+6N     5i      $ Password��6%   $ * 6T    67   �� *
Password+6T     5j      $
 Role��6%   $ * 6L    63   �� *Role+6L   5l        5n   
   -
   5        $ dtUser��5n      $��0,201,10,61q�6N   $��1��6c   $ False��6s    5o        5p      $ UserId��6%   $ * 65    6?   �� *UserId+  5q      $ 	Full Name��6%   $ * 68    6B   �� *
FullName+  5r      $ Email��6%   $ * 64    6>   �� *Email+  5s      $
 Role��6%   $ * 63    6=   �� *Role+5u      �� 5v   
   -   5         ��Name$
 role��5v      $
��Left=�5x   9   $��84,104,0,274q�6T   $	��186��6j   $��1��6v   ��5w      �� *Roles+ �� SelectedItem 5x      �� *SelectedRole+6v   5y      5z      