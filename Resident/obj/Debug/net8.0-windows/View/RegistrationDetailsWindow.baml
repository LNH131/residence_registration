   M S B A M L   `   `   `  ����  Resident-    'Resident.View.RegistrationDetailsWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$'  !Chi tiết đăng ký hộ khẩu��5   	   $	��450��5   	   $	��600��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $$��Chi tiết hồ sơ đăng ký��5      $��20(�5      $
��Bold"�5      $��0,0,0,10q�5      �� 5   
   $��1��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,5q�62   .+�5      �� $��Registration ID: ��6   $
��Bold"�65   $	��150��6G   �� 5      ��6   �� *Registration.RegistrationId+5      �� 5      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5       �� $��User: ��6   $
��Bold"�6*   $	��150��6<   �� 5!      ��6   �� *Registration.User.FullName+5"      �� 5%      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5&      �� $��Type: ��6   $
��Bold"�6*   $	��150��6<   �� 5'      ��6   �� *Registration.RegistrationType+5(      �� 5+      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�5,      �� $��Address ID: ��6   $
��Bold"�60   $	��150��6B   �� 5-      ��6   �� *Registration.AddressId+5.      �� 51      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�52      �� $��Status: ��6   $
��Bold"�6,   $	��150��6>   �� 53      ��6   �� *Registration.Status+54      �� 57      $ 
Horizontal=�6   $��0,0,0,5q�62   .+�58      �� $��
Comments: ��6   $
��Bold"�6.   $	��150��6@   �� 59      ��6   �� *Registration.Comments+5:      5;      �� 5>   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5?      �� $��Approve��6   $	��100��5@      $��5q�5A      ��5B      �� *ApproveCommand+{�5C      �� *	CanModify+�� 5E      $��Reject��6   $	��100��5F      $��5q�5G      ��5H      �� *RejectCommand+{�5I      �� *	CanModify+�� 5K      -   5        $��Close��5K      $	��100��5L      $��5q�5M      5N      5O      5P      5Q      