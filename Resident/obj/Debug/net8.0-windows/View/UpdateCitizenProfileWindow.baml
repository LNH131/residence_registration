   M S B A M L   `   `   `  ����C  ?Resident, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Resident.ViewResident.View  5   	   3clr-namespace:Resident.ServiceResident.Service  63   .    (Resident.View.UpdateCitizenProfileWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Resident.View     5   	   5
convertersclr-namespace:Resident.Service     63     � Title$   UpdateCitizenProfileWindow��5   	   $	��600��6,   $	��800��69    � 	Resources 5
      (   "Resident.Service.DateOnlyConverter   DateOnlyConverter��%   &	          5   
   6'   5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
������� �� Padding$ 10q�65    �� CornerRadius$ 8��6B   $��0,0,0,10q�6S   ._�5      �� $��Update Profile��6   $��24(�5      $
��Bold"�5      
����  �$��Center=�5      5      �� 5   
   $��1��6   $��Disabled=�6$   $
��Auto=�6M   .��5       ��  �� Orientation$ Vertical=�6   $��Center=�60   $	��Top=�6M   .+�5"      �� $ 
Horizontal=�6   $	��0,5q�66   .+�5#      �� $��
Full Name:��6    $	��150��62   $��Center=�6>   $
��Bold"�6Y   �� 5$      $	��300��6   ��6*   �� *CurrentUser.FullName+5%      �� 5'      $ 
Horizontal=�6   $	��0,5q�66   .+�5(      �� $��Email:��6    $	��150��6.   $��Center=�6:   $
��Bold"�6U   �� 5)      $	��300��6   ��6*   �� *CurrentUser.Email+5*      �� 5,      $ 
Horizontal=�6   $	��0,5q�66   .+�5-      �� $��ID Card:��6    $	��150��60   $��Center=�6<   $
��Bold"�6W   �� 5.      $	��300��6   ��6*   �� *CurrentUser.IdentityCard+5/      �� 51      $ 
Horizontal=�6   $	��0,5q�66   .+�52      �� $��Birth Date:��6    $	��150��63   $��Center=�6?   $
��Bold"�6Z   (  "System.Windows.Controls.DatePicker  53      $	��300��6!      SelectedDate 6-   �� *CurrentUser.Birthday+ �� 	Converter# [  54      �� 56      $ 
Horizontal=�6   $	��0,5q�66   .+�57      �� $��Gender:��6    $	��150��6/   $��Center=�6;   $
��Bold"�6V   �� 58      $	��300��6    �� SelectedValuePath$ Content��5:      	 �� SelectedValue	 59      �� *CurrentUser.Sex+.@�5;      �� $
��Male��6'   �� 5<      $��Female��6'   �� 5=      $��Other��6'   5>      5?      �� 5B      $��Address:��6   $
��Bold"�6,   $��0,15,0,5q�6>   �� 5C      $ Vertical=�6   $��10,0,0,0q�64   .+�5E      �� $ 
Horizontal=�6!   $	��0,5q�6:   .+�5F      �� $��Street:��6$   $	��150��63   $��Center=�6?   �� 5G      $	��300��6"   ��6.   �� *CurrentAddress.Street+5H      �� 5J      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5K      �� $��City:��6$   $	��150��61   $��Center=�6=   �� 5L      $	��300��6"   ��6.   �� *CurrentAddress.City+5M      �� 5O      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5P      �� $��State:��6$   $	��150��62   $��Center=�6>   �� 5Q      $	��300��6"   ��6.   �� *CurrentAddress.State+5R      �� 5T      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5U      �� $��Country:��6$   $	��150��64   $��Center=�6@   �� 5V      $	��300��6"   ��6.   �� *CurrentAddress.Country+5W      �� 5Y      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5Z      �� $��Ward:��6$   $	��150��61   $��Center=�6=   �� 5[      $	��300��6"   ��6.   �� *CurrentAddress.Ward+5\      �� 5^      $ 
Horizontal=�6!   $	��0,5q�6:   .+�5_      �� $��	District:��6$   $	��150��65   $��Center=�6A   �� 5`      $	��300��6"   ��6.   �� *CurrentAddress.District+5a      5b      5c      5d      �� 5g   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $��0,20,0,0q�6X   .+�5h      �� $
��Save��6   $	��100��6$   $��35��60   $
��10,0q�6<   
������$
��Bold"�5i   -   �� 5j      -   5        $��Cancel��5j      $	��100��6&   $��35��62   $
��10,0q�6>   
�������$
��Bold"�5k   -   6?   5l      5m      5n      