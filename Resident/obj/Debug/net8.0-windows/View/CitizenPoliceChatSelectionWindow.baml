   M S B A M L   `   `   `  ����  Resident4    .Resident.View.CitizenPoliceChatSelectionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Select Police to Chat��5   	   $	��300��5   	   $	��400��6    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Select Police to Chat��6   $��18(�5      $
��Bold"�5      $��0��5      $��0,0,0,10q�6!   �� 5   
   $��1��6    �� Orientation$ Vertical=�6"   $��Center=�69   .+�5      �� $��Police in your area:��6   $��0,0,0,5q�5      $
��Bold"�5      �� 5       �� DisplayMemberPath$ FullName��5      $	��200��5       $��30��6#   $��0,0,0,10q�5!      ��5      �� *AvailablePolice+ �� SelectedItem 5      �� *SelectedPolice+5!      �� 5#      $��
Start Chat��6   $	��120��5$      $��35��6!   $��0,0,0,20q�5&      ��5%      �� *StartChatCommand+5&      5'      �� 5*   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5+      �� $��Cancel��6   $��80��6&   $��30��61   ��5,      �� *CancelCommand+5-      5.      5/      