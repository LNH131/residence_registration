   M S B A M L   `   `   `  ����  Resident-    'Resident.View.PoliceChatSelectionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Select Chat Partner��5   	   $	��350��6%   $	��400��62    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $!��Select a user to chat with:��6   $��16(�5      $
��Bold"�6"   $��0��64   $��0,0,0,10q�6A   �� 5   
   $��1��6    �� Orientation$ Vertical=�6"   .+�5      �� $��	Citizens:��6   $
��Bold"�6)   $��0,5,0,5q�6;   �� 5       �� DisplayMemberPath$ FullName��5      $��30��64   $��0,0,0,10q�6@   ��5      �� *
Citizens+ �� SelectedItem 5      �� *SelectedCitizen+5   @   �� 5      $��Chat with Citizen��6   $��35��5      $��0,0,0,10q�6!   ��5      �� *ChatWithCitizenCommand+5   !   �� 5      $��Area Leaders:��6   $
��Bold"�6-   $��0,10,0,5q�6?   �� 5      $ FullName��5       $��30��64   $��0,0,0,10q�6@   ��5      �� *AreaLeaders+ 5      �� *SelectedAreaLeader+5    @   �� 5!      $��Chat with Area Leader��6   $��35��5#      $��0,0,0,10q�6!   ��5"      �� *ChatWithAreaLeaderCommand+5#   !   5$      �� 5'   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5(      �� $��Cancel��6   $��80��6&   $��30��61   ��6=   �� *CancelCommand+5)      5*      5+      