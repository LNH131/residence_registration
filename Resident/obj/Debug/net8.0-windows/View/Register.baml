   M S B A M L   `   `   `  ����C  ?Resident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Resident.ViewModelsResident.ViewModels  5   	       Resident.View.Register   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8
viewmodels!clr-namespace:Resident.ViewModels     5   	     � Title$  Register��5   	   $	��450��6   $	��400��6'    � WindowStartupLocation$ CenterScreen=�5	   	   .��5      �� $��20q�6   .+�5   
   �� $��
Full Name:��6   �� 5   
   ��6   �� *
FullName+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��Email:��6   �� 5   
   ��6   �� *Email+$ PropertyChanged=��� 5   
   $��CCCD:��6   �� 5   
   ��6   �� *IdentityCard+$ PropertyChanged=��� 5   
   $��	Password:��6   G� 5   
   -   5         G�Name$ PasswordBox��5      6+   �� 5   
   $��Confirm Password:��6   G� 5   
   -   5        $ ConfirmPasswordBox��5      62   �� 5   
   $��Role:��6   �� 5   
   $	��150��5      ��5      �� *Roles+ �� SelectedItem 5      �� *SelectedRole+5      �� 5    
   $��Area:��6   �� 5!   
    �� DisplayMemberPath$ AreaName��5#      $	��150��5$      ��5!      �� *Areas+ 5"      �� *SelectedArea+5$      �� 5&   
   $��Current Address:��6   �� 5'   
   $ Street��5)      $	��150��5*      ��5'      �� *	Addresses+ 5(      �� *SelectedAddress+5*      �� 5,   
    �� Orientation$ 
Horizontal=�6   $��0,20,0,0q�6.   $��Center=�6@   .+�5-      �� $��Register��6   $	��100��5/      $��0,0,10,0q�6!   ��5.      �� *RegisterCommand+5/   !   �� 50      $
��Back��6   $	��100��52      ��51      �� *BackCommand+52      53      �� 55   
   
���  ��$
��Wrap=�6C   ��6   �� *ErrorMessage+6C   56      57      