   M S B A M L   `   `   `  ����S  OsmartFactory_MillProcess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:smartFactory_MillProcesssmartFactory_MillProcess  5   	   /    )smartFactory_MillProcess.Views.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:smartFactory_MillProcess     5   	     � Title$  Mill Process��5   	    � WindowState$ 	Maximized=�5	   	    � 
ResizeMode$ 	CanResize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5         	BoolToVis��%   &	        �� 5   
   6'   5      .��5      � .+�5   
   ]� $
��True��6   .+�5      �� $
��Left=�6   $	��100��5      ���#2C3E50y�5      �� *
IsMenuOpen+ �� 	Converter# [  .+�5      �� $
��Menu��6   ���#00c2ff$
��Bold"�5      $��18(�5      $��10q�5      $��Center=�5      �� 5!      $��	Dashboard��6   $��10q�5"      $��5q�5#      ���#34495E
�������
������ 5&      �� 5(      $��사원 관리��6   $��10q�5*      $��5q�5+      ���#34495E
�������
������ ��5)      �� *MoveEmployeeManagementCommand+5.      �� 51      $��뒤로가기��6   $��10q�53      $��5q�54      ���#34495E
�������
������ ��52      �� *GoBackCommand+57      �� 59      $��로그 아웃��6   $��10q�5;      $��5q�5<      ���#E74C3C
�������
������ ��5:      �� *LogoutCommand+5?      5A       � 5D      -   5          �Name$ 	MainFrame��5D        � NavigationUIVisibility$ Hidden=�6'   5E      5F      5G      