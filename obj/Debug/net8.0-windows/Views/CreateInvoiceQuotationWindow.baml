   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      A =Medici, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Medici.ViewModelsMedici.ViewModels 5      /   )Medici.Views.CreateInvoiceQuotationWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      6sys&clr-namespace:System;assembly=mscorlib     5      X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      0
viewmodelsclr-namespace:Medici.ViewModels  5      .��5      � $��10q�6     � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��2��6   $��Create Invoice/Quotation��6(   $��18(�6H   $
��Bold"�6V   $��0,0,0,10q�6h   $��Center=�6z   �� 5   
    �� Orientation$ Vertical=�6   $��0,20,0,0q�6,   .+�5      	� -   5         	�Name$ InvoiceSelectRadioButton��5      $��Invoice��6<   $��60��6N   $
��Left=�6Y   6t   	� 5       -   5        $  QuotationSelectRadioButton��5       $��	Quotation��6>   $��75��6R   $
��Left=�6]   6x   5!      �� 5$   
   $��1��6   $��0��6"   $ Vertical=�62   $��	0,5,10,10q�6I   .+�5%      �� -   5         ��Name$ DetailsTextBlock��5%      $��Details��62   $��14(�6A   $��SemiBold"�6O   $��0,0,0,5q�6e   �� 5&      $ 
Horizontal=�6   .+�5'      �� $��Number:��6   �� 5(      -   5        $ InvoiceNumberTextBlock��5(      $��80��6<   $��13,0,0,0q�6G   5)      �� 5*      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5+      �� $!��Terms of Payment (i.e COD):��6   �� 5,      -   5         ��Name$ TermsOfPaymentTextBox��5,      $��10,0,0,0q�69   $��60��6K   5-      �� 5.      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5/      �� $��Client��6   $��0,0,10,0q�6*   �� 50      -   5         ��Name$ SelectClientComboBox��50      $	��150��69   51      �� 52      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�53      �� $��Issue Date:��6   (  "System.Windows.Controls.DatePicker  54      -   5          Name$ InvoiceDatePicker��54      
 �� Now	   SelectedDate#	 Z 68   $	��180��6c   $��13,0,0,0q�6o   55      �� 56      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�57      �� $��	Due Date:��6     58      -   5        $ DueDatePicker��58      $	��180��64   $��13,0,0,0q�6@   59      �� 5:      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5;      �� $��VAT No:��6   �� 5<      -	   5        $ VATNoTextBox��5<      $	��100��60   $��10,0,0,0q�6<   $��
4220284485��6N   5=      �� 5>      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5?      �� $��Reg No:��6   �� 5@      -
   5        $ RegNoTextBox��5@      $	��100��60   $��10,0,0,0q�6<   $��2015/242557/07��6N   5A      �� 5B      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5C      �� $��Our Ref No.:��6   �� 5D      -   5        $ OurRefNoTextBox��5D      $	��100��63   $��10,0,0,0q�6?   5E      �� 5F      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5G      �� $��Your Order No:��6   �� 5H      -   5        $ YourOrderNoTextBox��5H      $	��100��66   $��10,0,0,0q�6B   5I      5J      �� 5L   
   $��1��6   $��1��6"   $ Vertical=�62   $��	10,5,0,10q�6I   .+�5M      �� -   5        $ ItemsTextBlock��5M      $��Items��60   $��14(�6=   $��SemiBold"�6K   $��0,0,0,5q�6a   �� 5N      $ 
Horizontal=�6   .+�5O      �� $��Item Description��6   $��0,0,10,0q�64   �� 5P      -   5        $ ItemDescTextBox��5P      $	��150��63   5Q      �� 5R      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5S      �� $��Quantity��6   $��0,0,10,0q�6,   %  Xceed.Wpf.Toolkit.IntegerUpDown  5T      -   5        
  Name$
 QuantityUpDown��5T   %   $��50��6=   $��20��6H   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]]   Minimum 16T      Maximum 1006`      Value 16n   5U      �� 5V      $ 
Horizontal=�6   $��0,10,0,0q�62   .+�5W      �� $��Unit Price (ZAR)��6   $��0,0,10,0q�64   �� 5X      -   5        $ UnitPriceTextBox��5X      $��80��64   5Y      �� 5Z      -   5         ��Name$ AddItemButton��5Z      $��Add Item��6,   $��65��6?   $��Center=�6J   $��0,20,0,0q�6g   6y   5[      &   System.Windows.Controls.DataGrid  5^   
   -   5        $��ItemDataGrid��5^   p   $��2��6   $��2��6       AutoGenerateColumns$ False��64   $	��150��6P   $��	0,10,0,10q�6]      CanUserAddRows$ False��5_         SelectionMode$ Single=�6"      SelectionUnit$ FullRow=�69   ��6Q   �� *	LineItems+   Columns 5`      0  *System.Windows.Controls.DataGridTextColumn  5a      ,  &System.Windows.Controls.DataGridColumn   Header$ Item Description��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6_   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   �� *Description+6_     5b      $ Quantity��6%   $	 100 6T    67   �� *
Quantity+6T     5c      $ 
Unit Price��6%   $	 100 6W    69   �� *	UnitPrice+6W     5d      $ Total��6%   $	 100 6N    64   �� *Total+6N   5f      �� 5i   
   -   5        $ DeleteItemsButton��5i      $��3��6,   $��2��69   $��Delete Selected Item��6M   $	��200��6l   $��Right=�6x   $��10q�6�   6�   �� 5l   
   $��4��6   $��1��6"   $ Vertical=�62   $��Right=�6I   $��10,5,0,0q�6e   .+�5n      �� $ 
Horizontal=�6   .+�5o      �� $��	Subtotal:��6   $
��Bold"�6-   $��0,0,10,0q�6?   �� 5p      -   5        $ SubtotalTextBox��5p      $	��100��63    �� 
IsReadOnly$
 True��6?   $��0,0,0,5q�6Q   5q      �� 5t      $ 
Horizontal=�6   .+�5u      �� $��
VAT (15%):��6   $
��Bold"�6.   $��0,0,10,0q�6@   �� 5v      -   5        $ 
VATTextBox��5v      $	��100��6.   $
 True��6:   $��0,0,0,5q�6L   5w      �� 5z      $ 
Horizontal=�6   .+�5{      �� $��Total Amount:��6   $
��Bold"�61   $��0,0,10,0q�6C   �� 5|      -   5        $ TotalAmountTextBox��5|      $	��100��66   $
 True��6B   5}      5~      �� 5�   
   $��5��6   $��2��6"   $ 
Horizontal=�66   $��Center=�6O   $��	10,0,0,10q�6l   .+�5�      �� -   5        $ 
SaveButton��5�      $
��Save��6)   $	��100��68   $��5q�6D   6O   �� 5�      -   5        $ CancelButton��5�      $��Cancel��6+   $	��100��6<   $��5q�6H   6S   5�      5�      5�      