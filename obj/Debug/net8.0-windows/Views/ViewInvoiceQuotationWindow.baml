   M S B A M L   `   `   `  ����A  =Medici, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Medici.ViewsMedici.Views  5   	   -    'Medici.Views.ViewInvoiceQuotationWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Medici.Views     5   	   $	��500��5   	   $	��500��6   .��5	      � $��10q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��View Invoice/Quotation��6!   $��20(�6?   $
��Bold"�6M   $��Center=�6_   $��0,0,0,20q�6|   �� 5   
    �� Orientation$ 
Horizontal=�6   $��0,40,0,0q�6.   .+�5      �� -   5        $��UploadHeaderButton��5      $��20��6/   $��10,0,0,0q�6;   $��Upload Header Image��6M   6k   �� 5      -   5        $��HeaderImage��5      $	��200��6'   $	��100��63   �� 5      -   5        $��HeaderImagePathTextBox��5       �� 
IsReadOnly$
 True��64   $��10q�6F   $	��130��6R   $��20��6^   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   $��Center=�6;   $��	0,10,0,10q�6X   .+�5      �� $ 
Horizontal=�6   $��0,0,20,0q�62   .+�5       �� $��Issue Start Date:��6   $��Center=�65   $��0,0,5,0q�6P   (  "System.Windows.Controls.DatePicker  5!      -   5          Name$ StartDatePicker��5!      $	��120��66   5"      �� 5#      $ 
Horizontal=�6   .+�5$      �� $��Issue End Date:��6   $��Center=�63   $��0,0,5,0q�6N     5%      -   5        $ EndDatePicker��5%      $	��120��64   5&      �� 5'      -   5         ��Name$ 
LoadButton��5'      $
��Load��6)   $��40��68   $��5,0,0,0q�6C   6T   5(      &   System.Windows.Controls.DataGrid  5+   
   -   5          Name$ InvoicesDataGrid��5+       $��2��6      AutoGenerateColumns$ False��6:      
IsReadOnly$
 True��6V      SelectionMode$ Single=�6h   $��0,0,0,10q�6   	   Columns	 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Invoice/Quotation No.��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6f   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   �� *InvoiceNumber+6f     5.      $
 Client��6%   $ * 6T    65   �� *
ClientName+6T     5/      $
 Date Issued��6%   $
 Auto 6Y    6:   �� *
DateIssued+6Y     50      $
 Total Amount��6%   $
 Auto 6[    6;   �� *TotalAmount+6[   52      �� 55   
   $��3��6   $ 
Horizontal=�6"   $��Center=�6;   .+�56      �� -   5        $ PrintButton��56      $��Print��6*   $	��100��6:   $��5q�6F   6Q   �� 57      -	   5        $ 
EditButton��57      $
��Edit��6)   $	��100��68   $��5q�6D   ${�False��6O   58      59      5:      