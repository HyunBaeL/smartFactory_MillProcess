   M S B A M L   `   `   `  ����S  OsmartFactory_MillProcess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:smartFactory_MillProcess.ViewssmartFactory_MillProcess.Views  5      9    3smartFactory_MillProcess.Views.ProcessMainConductor   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:smartFactory_MillProcess.Views     5        N� Title$  ProcessMainConductor��5	      .��5
      �� $	��700��6    �� Padding$ 90q�6    �� CornerRadius$ 8��6&   ���#2B2F3A���#00C2FF$��1q�5      $��Center=�5   	   $��Center=�6&    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6      
BlurRadius$ 15Q�6-      ShadowDepth$ 0Q�6=      Opacity$	 0.5Q�6M   ._�5   
   � $	��280��6    � RowDefinitions 5      �� 5      $
��121* �6    �� 5      $
��125* �6    .+�5      �� $��0��6   $��Main Control��5      ���#00C2FF$��20(�5      $��SemiBold"�6&   $��Center=�5      $��0,0,0,23q�5      �� 5      -   5        	 ��Name$	 
imgMachine��5      $	��400��5      $	��100��6    $T��Npack://application:,,,/smartFactory_MillProcess;component/Resource/machine.png��5       $��59,32,59,116q�5!      $��2��5"      �� 5$      $��1��6   $��	가열로��6%   ���#00C2FF$��13(�6F   $��SemiBold"�6T   $
��Left=�6j   $��
100,64,0,0q�6�   �� 5%      $��1��6   $��	압연기��6%   ���#00C2FF$��13(�6E   $��SemiBold"�6S   $��Right=�6i   $��
0,64,100,0q�6�   �� 5'      $��1��6   $��80��6"   $��35��6-   $ 8��69   ���#00C2FF
 � Cursor$

 Hand��6_   $��358,83,80,24q�6m    J� InputBindings 5(      d� 5)       d� Gesture$ 	LeftClicka�6#    �� Command 67   �� *FurnaceStatus+._�5+      �� $��현재 상태��6   ���#1B1F2B$
��Bold"�6>   $��Center=�6P   $��Center=�6k   5,      �� 5-      $��2��6   $��80��6"   $��35��6-   $ 8��69   ���#00C2FF$

 Hand��6_   $��
0,65,280,6q�6m    5.      d� 5/      $ 	LeftClicka�6#    67   �� *FurnaceStatus+._�51      �� $��현재 상태��6   ���#1B1F2B$
��Bold"�6>   $��Center=�6P   $��Center=�6k   52      � 53      -   5         �Name$ FurnaceProgress��53       � Minimum$ 0Q�63    � Maximum$	 100Q�6?   $��1��6M   $��7��6Z   $	��100��6e   $��70,120,348,15q�6�    � Value 6q   �� *FurnacePercent+6�   � 54      -   5        $ RollProgress��54      $ 0Q�60   $	 100Q�6<   $��1��6J   $��7��6W   $	��100��6b   $ 0Q�6n   $��348,120,70,10q�6x   55      56      57      