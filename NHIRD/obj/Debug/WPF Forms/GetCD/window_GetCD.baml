   M S B A M L   `   `   `  ����@  <NHIRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NHIRDNHIRD  5   	       NHIRD.Window_GetCD   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:NHIRD     5   	   -   5          �Name$  WindowGetCD��5   	    � Title$ Window1��5   	   $	��700��6   $
��1400��6&   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��700 �6   �� 5      $��1* �6   .+�5   
   �� $��Center=�6   $��0��62   $
��0,20q�6B   .+�5         NHIRD.IOFolderSelector  -   5          Name$ inputFolderSelect��5   %      Title$ 
Input Dir:��5   %      
FolderPath 5   %   �� *
InputDir+ �� Mode$ TwoWay=��� 5      $
��0,10q�6   $��Center=�6'    �� Orientation$ 
Horizontal=�6D   .+�5      �� $
��15,0q�6    d� 	IsChecked 6)   �� *IsCDFileTypeEnabled+.��6S   CD6W   �� 5      $
��15,0q�6    6*   �� *IsDDFileTypeEnabled+.��6T   DD6X   5         NHIRD.FileListControl  5      -   5        	  Name$	 fileListControl��5   $   
   inputFileList
 5   $   �� *inputFileList+5      �� 5   
   -   5         ��Name$ criteriaStackPanel��5      $��1��61   $
��0,20q�6A   $��Center=�6O   .+�5      �� $ 
Horizontal=�6   $��Center=�62   .+�5         NHIRD.StringListControl     Title$ ICD Inclusion��5   *      CurrentList 5   *   �� *ICDIncludes+   IsCriteriaEnabled 5    *   �� *IsICDIncludeEnabled+$ TwoWay=�  5!      $ ICD Exclusion��5"   *    5!   *   �� *ICDExcludes+ 5#   *   �� *IsICDExcludeEnabled+$ TwoWay=�5$      �� 5%      $ 
Horizontal=�6   $��Center=�62   .+�5&        $ Procedure Inclusion��5'   *    5&   *   �� *PROCIncludes+ 5(   *   �� *IsPROCIncludeEnabled+$ TwoWay=�  5)      $ Procedure Exclusion��5*   *    5)   *   �� *IsPROCExcludeEnabled+ 5+   *   �� *IsPROCExcludeEnabled+$ TwoWay=�5,      �� 5-      $��Center=�6   $��0, 10q�66   .+�5.      �� $��Center=�6   $ 
Horizontal=�6<   .+�5/      �� $��Center=�6    Age Criteria:6J   �� 50      $
��10,0q�6!   .+�51      �� $ 
Horizontal=�6%   .+�52      �� -   5         ��Name$ Cb_AgeL��52   '   $��80��68    6C   �� *IsAgeLCriteriaEnable+$ TwoWay=�.��6{   Lower Limit6�   �� 53      -   5         ��Name$ Tx_AgeL��53   &   $
��10,0q�67   $��50��6E   ��6P   �� *
str_AgeL+6�   54      �� 55      $ 
Horizontal=�6%   .+�56      �� -   5        $ Cb_AgeU��56   '   $��80��68    6C   �� *IsAgeUCriteriaEnable+$ TwoWay=�.��6{   Upper Limit6�   �� 57      -   5        $ Tx_AgeU��57   '   $
��10,0q�68   $��50��6F   ��6Q   �� *
str_AgeU+6�   58      59      �� 5:      $��Center=�6    
Year Old6E   5;      5=         NHIRD.EXT_FolderSelector  5>      -	   5          Name$ IDCrieteria_FolderSelect��5>   G      Title$ Use Extracted ID Files:��6'      FileType$ CD,DD��5B   '      SubFileName$	 IDS��5C   '      IsCriteriaChecked 5?   '   �� *IsIDCriteriaEnable+$ TwoWay=�   
FolderPath 5@   '   �� *IDCriteriaFolderPath+$ TwoWay=�   FileList 5A   '   �� *IDCriteriaFileList+$ TwoWay=�5C   '     5D      -
   5        $! OrderCrieteria_FolderSelect��5D   J   $  Use Extracted Order Files:��6'   $ OO,DO,GO��5H   '    5E   '   �� *IsActionCriteriaEnable+$ TwoWay=� 5F   '   �� *ActionCriteriaFolderPath+$ TwoWay=� 5G   '   �� *ActionCriteriaFileList+$ TwoWay=�5H   '   �� 5I      -   5        $	��100��5I      .��68   	Reset All6C     5J      -   5        $ ouputFolderSelect��5J   %   $ 
Ouput Dir:��5K   %    5L   %   �� *str_outputDir+$ TwoWay=��� 5O      $��50��6   $
��0,25q�6!   ��6/   �� *Do_ExtractData+.��6R   Do _Extract6_   �� 5P      5Q      5R      5S      