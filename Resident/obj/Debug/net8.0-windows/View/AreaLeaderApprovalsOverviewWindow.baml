   M S B A M L   `   `   `  ����  Resident5    /Resident.View.AreaLeaderApprovalsOverviewWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$0  *Tổng quan duyệt hồ sơ (Area Leader)��5   	   $	��450��65   $	��800��6B    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
����ح� �� Padding$ 10q�65   ._�5      �� $0��*Tổng quan duyệt hồ sơ (Area Leader)��6   $��24(�5      $
��Bold"�6&   
����  �68   5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��6/      SelectionMode$ Single=�6A   ��5       �� *ApprovalItems+ �� SelectedItem 5      �� *SelectedApprovalItem+ �� Mode$ TwoWay=�	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *ItemId+6L     5      $

 Type��6%   $	 120 6P    63   �� *
ItemType+6P     5      $
 Creator��6%   $	 150 6V    66   �� *CreatorName+6V     5      $
 Status��6%   $	 100 6P    65   �� *Status+6P   4  .System.Windows.Controls.DataGridTemplateColumn  5      $
 Action��6)   $	 120 69      CellTemplate 5       �� 5!      .R�5"      �� $��View Detail��6%   ��5#   %   �� * DataContext.ViewDetailsCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5$   %   �� 5%      5'      5)      �� 5,   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5-      �� $��Refresh��6   $	��100��6'   ��63   �� *RefreshCommand+5.      5/      50      