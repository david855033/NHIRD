   M S B A M L   `   `   `  ����@  <NHIRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NHIRDNHIRD  5   	   &     NHIRD.AgeSpecificIncidenceWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:NHIRD     5   	   -   5          � Title$  PatientBasedDataWindow��5   	   $	��700��6(   $
��1400��65   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��700 �6   �� 5      $��1* �6   .+�5   
   �� $��Center=�6   $��0��62   $
��0,20q�6B   .+�5         NHIRD.EXT_FolderSelector  -   5          Name$$ patientBasedDataFolderSelector��5   '      Title$ Patient Based Data��5   %      SubFileName$	 PBD��5   %      
FolderPath 5   %   ��  �� Path$  patientBasedDataFolderPath� �� Mode$ TwoWay=�5   %     5      -   5        $! standarizedIDFolderSelector��5   '   $ Standarized ID Data��5   %   $	 IDS��5   %    5   %   �� $ standarizedIDFolderPath�$ TwoWay=�5   %   ]� 5      $��0,0,0,30q�6   .+�5      �� $��Data End date = ��6   �� 5      ��6   �� *dataEndDate+5        5      -   5        $&  IDIncludeCrieteriaFolderSelector��5   '   $ ID include criteria��5   %   $	 PBD��5!   %    5    %   �� $" IDIncludeCrieteriaFolderPath�$ TwoWay=�5!   %      NHIRD.IOFolderSelector  5#      -   5          Name$ ouputFolderSelect��5#   %   	   Title$	 
Ouput Dir:��5$   %   
   
FolderPath
 5%   %   �� *	outputDir+$ TwoWay=�]� 5(      $
��0,25q�6   .+�5)      �� $��Matching Result = ��6   �� 5*      ��6   �� *matchResult+5+      �� 5,      -   5        $	��250��5,   ,   .��68   	Calculate6C   5-      �� 5.   
   $��Center=�6   $��1��62   $
��0,20q�6B   50      51      52      