   M S B A M L   `   `   `  ����  Resident(    "Resident.View.StatusOverviewWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Theo dõi hồ sơ��5   	   $	��500��6    $	��800��6-    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � .+�5   
   �� .@�5	      �� $��Đăng ký hộ khẩu��6   .��5
      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��5      $��10q�67      
IsReadOnly$
 True��5      ��5
      �� *Registrations+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6\   1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *RegistrationId+6\     5      $ Ngày đăng ký��6-   $ * 6|    6C   �� *	StartDate+ �� StringFormat$ 
dd/MM/yyyy��6|     5      $ Trạng thái��6-   $ * 6\    6A   �� *Status+6\     5      $ Ghi chú��6-   $ 2* 6[    6>   �� *
Comments+6[   5      5      �� 5      $��Đơn tách hộ��6   .��5        $ False��5      $��10q�67   $
 True��5      ��5      �� *HouseholdSeparations+ 5        5      $ ID��6-   $
 Auto 6Z    69   �� *SeparationId+6Z     5      $ Ngày yêu cầu��6-   $ * 6~    6C   �� *RequestDate+$ 
dd/MM/yyyy��6~     5      $ Trạng thái��6-   $ * 6\    6A   �� *Status+6\     5      $ Ghi chú��6-   $ 2* 6[    6>   �� *
Comments+6[   5!      5"      �� 5%      $ ��Đơn chuyển hộ khẩu��6   .��5&        $ False��5'      $��10q�67   $
 True��5(      ��5&      �� *HouseholdTransfers+ 5)        5*      $ ID��6-   $
 Auto 6X    69   �� *
TransferId+6X     5+      $ Ngày yêu cầu��6-   $ * 6~    6C   �� *RequestDate+$ 
dd/MM/yyyy��6~     5,      $ Trạng thái��6-   $ * 6\    6A   �� *Status+6\     5-      $ Ghi chú��6-   $ 2* 6[    6>   �� *
Comments+6[   5/      50      51      �� 54   
   -   5        $��Đóng��54      $	��100��6    $��30��6,   $��Right=�55      $��Bottom=�6-   $��10q�6H   56      57      58      