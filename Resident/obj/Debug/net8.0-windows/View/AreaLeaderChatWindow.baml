   M S B A M L   `   `   `  ����  Resident(    "Resident.View.AreaLeaderChatWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Area Leader Chat��5   	   $	��500��6"   $	��600��6/    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��1��5      $��10q�6   
�������$��1q�5   *   
���������5      �� *ChatMessages+��5      �� 5      .R�5      �� $��5q�6!   .+�5      ��  �� Orientation$ 
Horizontal=�6%   .+�5      �� $
��Bold"�5   (   $��0,0,5,0q�6:   ��5   (   �� *FromUserFullName+5   :   �� 5      $��:��6(   $��0,0,5,0q�61   �� 5      $
��Wrap=�6A   ��6(   �� *	Content+6A   5      �� 5      $��10(�5   $   
�������$��Right=�6D   ��5   $   �� *
SentDate+ �� StringFormat$ 	{0:HH:mm}��5   D   5      5       5"      �� 5%   
   $ 
Horizontal=�6   $��2��6.   $��10q�6;   .+�5&      �� $	��400��6   $��0,0,5,0q�5(      ��5'      �� *
NewMessage+ �� UpdateSourceTrigger$ PropertyChanged=�5(      �� 5)      $
��Send��6   $��80��5*      ��5+      �� *SendMessageCommand+5,      5-      5.      