   M S B A M L   `   `   `  ����  Resident,    &Resident.View.HouseHoldSelectionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Chọn hộ khẩu��5   	   $	��450��6   $	��800��6+   .��5      �  � 	Resources 5
   
    �� 
TargetType �� Property �� Value��%|   '��       �� 5      " ��6   .�5      ��  � ��$ 0,0,5,0q�6+   �� 5       � ��$	 8,4q�6,   5      5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgHouseholds��5         AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5      	   SelectionMode$	 Single=�5      $��10q�5      
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Số hộ khẩu��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6Y   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *HouseholdId+6Y     5      $ Địa chỉ��6%   $ * 6[    66   �� *FormattedAddress+6[   5       �� 5#   
   $��1��6    �� Orientation$ 
Horizontal=�5$      $��Right=�5%      $��10q�5&      .+�5'      �� -   5        $��Chọn��5'      6$   �� 5(      -   5        $��Hủy��5(      $��5,0,0,0q�6>   5)      5*      5+      