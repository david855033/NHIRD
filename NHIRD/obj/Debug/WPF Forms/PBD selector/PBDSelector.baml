   M S B A M L   `   `   `  ����@  <NHIRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NHIRDNHIRD  5   	       NHIRD.PBDSelectorWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:NHIRD     5   	   -   5          � Title$  PBDSelector��5   	   $	��700��6   $
��1400��6*   .��5	      �  � ColumnDefinitions 5
      �� 5   
   $	��700 �6   �� 5   
   $��1* �6   .+�5      �� $��Center=�6   $��0��6.   $
��0,20q�6>   .+�5   
      NHIRD.EXT_FolderSelector  -   5          Name$$ patientBasedDataFolderSelector��5   #      Title$ Patient Based Data��5   %      SubFileName$	 PBD��5   %      
FolderPath 5   %   ��  �� Path$  patientBasedDataFolderPath� �� Mode$ TwoWay=�5   %   ]� 5      $
��0,25q�6   .+�5      �� $7��1Select Field(可用order名稱或是診斷名稱)��6   �� 5      ��6   �� *selectField+5      ]� 5      $
��0,25q�6   .+�5      �� $L��FExclude Field(可用order名稱或是診斷名稱, 可用逗號隔開)��6   �� 5      ��6   �� *excludeField+5         NHIRD.IOFolderSelector  5      -   5          Name$ ouputFolderSelect��5   %   	   Title$	 
Ouput Dir:��5   %   
   
FolderPath
 5    %   �� *	outputDir+$ TwoWay=��� 5#      -   5        $	��250��5#   *   $
��0,50q�66   .��6D   	Calculate6O   5$      5%      5&      