   M S B A M L   `   `   `  ����@  <NHIRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NHIRDNHIRD  5   	   "    NHIRD.PatientBasedDataWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:NHIRD     5   	   -   5          � Title$  PatientBasedDataWindow��5   	   $	��700��6(   $
��1400��65   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��700 �6   �� 5      $��1* �6   .+�5   
   �� $��Center=�6   $��0��62   $
��0,20q�6B   .+�5         NHIRD.EXT_FolderSelector  -   5          Name$ ActionFolderSelector��5   B      Title$ Action file Folder��6'      IsEXTO .    FileType$ CD,DD,GD��5   '      
FolderPath 5   '   �� *
inputDir+ �� Mode$ TwoWay=�   FileList 5   '   �� *
inputFiles+$ TwoWay=�5   '       NHIRD.DiagnosisGroupEditor  5      -   5        	  Name$	 diagnosisGroupEditor��5   )   5      �� 5   
   $��Center=�6   $��1��62   $
��0,20q�6B   .+�5         NHIRD.OrderGroupEditor  -   5        
  Name$
 orderGroupEditor��5   %      NHIRD.IOFolderSelector  5      -   5          Name$ OutputFolderSelector��5   4      Title$ Output��6%      
FolderPath 5   %   �� *	outputDir+�� 5      ��6   �� *!generatePatientBasedDataCommand+.��6I   Generate Patient Based Data6h   5      5      5      