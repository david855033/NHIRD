   M S B A M L   `   `   `  ����@  <NHIRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NHIRDNHIRD  5   	       NHIRD.Window_ID   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:NHIRD     5   	   -   5          �Name$  
thisWindow��5   	    � Title$ Window1��5	   	   $	��700��6   $
��1400��6&   .��5
      �  � ColumnDefinitions 5   
   �� 5      $	��640 �6   �� 5      $��1* �6   .+�5   
   �� $��Center=�6   $��0��62   $
��0,20q�6B   .+�5         NHIRD.IOFolderSelector  -   5          Name$ inputFolderSelect��5   %      Title$ 
Input Dir:��5   %      
FolderPath 5   %   �� *
InputDir+ �� Mode$ TwoWay=�   NHIRD.FileListControl  5      -   5          Name$ fileListControl��5   $   $
��0,50q�5   $      inputFileList 5   $   �� *inputFileList+5   $   5      �� 5   
   $��1��6   $��0,200q�6%   $��Center=�64   .+�5         NHIRD.BirthYearLimit  $��0,0,0,50q�6#   	   isUpperLimitEnabled	 5   #   �� 
 �� Path$
 isUpperLimitEnabled�$ TwoWay=�   isLowerLimitEnabled 5   #   �� $
 isLowerLimitEnabled�$ TwoWay=�   
upperLimit 5   #   �� $
 
upperLimit�$ TwoWay=�   
lowerLimit 5   #   �� $
 
lowerLimit�$ TwoWay=�  5       -   5        $ ouputFolderSelect��5    %   $ 
Ouput Dir:��5!   %    5"   %   �� *str_outputDir+$ TwoWay=��� 5$      $��50��6   $
��0,25q�6!   ��6/   �� *Do_StandarizeID+.��6S   Do _Standarize ID6f   5%      5&      5'      