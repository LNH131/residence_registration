   M S B A M L   `   `   `  ����  Resident1    +Resident.View.PoliceApprovalsOverviewWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$+  %Tổng quan duyệt hồ sơ (Police)��5   	   $	��450��60   $	��800��6=    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� 
����ح�$��0��6(    �� Padding$ 10q�65   ._�5      �� $+��%Tổng quan duyệt hồ sơ (Police)��6   $��24(�5      $
��Bold"�6&   
����  �68   5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��6/      SelectionMode$ Single=�6A   ��5       �� *ApprovalItems+ �� SelectedItem 5      �� *SelectedApprovalItem+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 80 61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *ItemId+  5      $
	 Type��6%   $	
 120 63    6?   �� *
ItemType+  5      $	 Creator��6%   $	
 150 66    6B   �� *CreatorName+  5      $	 Status��6%   $	
 100 65    6A   �� *Status+4  .System.Windows.Controls.DataGridTemplateColumn  5      $	 Action��6)   $	
 120 69      CellTemplate 5      �� 5      .R�5      �� $��View Detail��6%   ��5    %   �� * DataContext.ViewDetailsCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5!   %   �� 5"      5$      5&      �� 5)   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��10q�6W   .+�5*      �� $��Refresh��6   $	��100��6'   ��63   �� *RefreshCommand+5+      5,      5-      