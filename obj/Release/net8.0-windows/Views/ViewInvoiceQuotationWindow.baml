   M S B A M L   `   `   `  ���� A  =Medici, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Medici.ViewsMedici.Views  -    'Medici.Views.ViewInvoiceQuotationWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:Medici.Views     $	��500��$	��500��.��� $��10q�  � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��View Invoice/Quotation��$��20(�$
��Bold"�$��Center=�$��0,0,0,20q���  �� Orientation$ 
Horizontal=�$��0,40,0,0q�.+��� -   $��UploadHeaderButton��$��20��$��10,0,0,0q�$��Upload Header Image���� -   $��HeaderImage��$	��200��$	��100���� -   $��HeaderImagePathTextBox�� �� 
IsReadOnly$
 True��$��10q�$	��130��$��20���� $��1��$ 
Horizontal=�$��Center=�$��	0,10,0,10q�.+��� $ 
Horizontal=�$��0,0,20,0q�.+��� $��Issue Start Date:��$��Center=�$��0,0,5,0q�(  "System.Windows.Controls.DatePicker  -     Name$ StartDatePicker��$	��120���� $ 
Horizontal=�.+��� $��Issue End Date:��$��Center=�$��0,0,5,0q�  -   $ EndDatePicker��$	��120���� -    ��Name$ 
LoadButton��$
��Load��$��40��$��5,0,0,0q�&   System.Windows.Controls.DataGrid  -     Name$ InvoicesDataGrid��$��2��   AutoGenerateColumns$ False��   
IsReadOnly$
 True��   SelectionMode$ Single=�$��0,0,0,10q�	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$
 Invoice/Quotation No.��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *InvoiceNumber+  $
 Client��$ *  �� *
ClientName+  $
 Date Issued��$
 Auto  �� *
DateIssued+  $
 Total Amount��$
 Auto  �� *TotalAmount+�� $��3��$ 
Horizontal=�$��Center=�.+��� -   $ PrintButton��$��Print��$	��100��$��5q��� -	   $ 
EditButton��$
��Edit��$	��100��$��5q�${�False��