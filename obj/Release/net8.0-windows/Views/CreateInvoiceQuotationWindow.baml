   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  A =Medici, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Medici.ViewModelsMedici.ViewModels /   )Medici.Views.CreateInvoiceQuotationWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6sys&clr-namespace:System;assembly=mscorlib     X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  0
viewmodelsclr-namespace:Medici.ViewModels  .��� $��10q�  � RowDefinitions  �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $��* ��� $��* �.+��� $��2��$��Create Invoice/Quotation��$��18(�$
��Bold"�$��0,0,0,10q�$��Center=���  �� Orientation$ Vertical=�$��0,20,0,0q�.+�	� -    	�Name$ InvoiceSelectRadioButton��$��Invoice��$��60��$
��Left=�	� -   $  QuotationSelectRadioButton��$��	Quotation��$��75��$
��Left=��� $��1��$��0��$ Vertical=�$��	0,5,10,10q�.+��� -    ��Name$ DetailsTextBlock��$��Details��$��14(�$��SemiBold"�$��0,0,0,5q��� $ 
Horizontal=�.+��� $��Number:���� -   $ InvoiceNumberTextBlock��$��80��$��13,0,0,0q��� $ 
Horizontal=�$��0,10,0,0q�.+��� $!��Terms of Payment (i.e COD):���� -    ��Name$ TermsOfPaymentTextBox��$��10,0,0,0q�$��60���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Client��$��0,0,10,0q��� -    ��Name$ SelectClientComboBox��$	��150���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Issue Date:��(  "System.Windows.Controls.DatePicker  -     Name$ InvoiceDatePicker��
 �� Now	   SelectedDate#	 Z $	��180��$��13,0,0,0q��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��	Due Date:��  -   $ DueDatePicker��$	��180��$��13,0,0,0q��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��VAT No:���� -	   $ VATNoTextBox��$	��100��$��10,0,0,0q�$��
4220284485���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Reg No:���� -
   $ RegNoTextBox��$	��100��$��10,0,0,0q�$��2015/242557/07���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Our Ref No.:���� -   $ OurRefNoTextBox��$	��100��$��10,0,0,0q��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Your Order No:���� -   $ YourOrderNoTextBox��$	��100��$��10,0,0,0q��� $��1��$��1��$ Vertical=�$��	10,5,0,10q�.+��� -   $ ItemsTextBlock��$��Items��$��14(�$��SemiBold"�$��0,0,0,5q��� $ 
Horizontal=�.+��� $��Item Description��$��0,0,10,0q��� -   $ ItemDescTextBox��$	��150���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Quantity��$��0,0,10,0q�%  Xceed.Wpf.Toolkit.IntegerUpDown  -   
  Name$
 QuantityUpDown��$��50��$��20���  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]]   Minimum 1   Maximum 100   Value 1�� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Unit Price (ZAR)��$��0,0,10,0q��� -   $ UnitPriceTextBox��$��80���� -    ��Name$ AddItemButton��$��Add Item��$��65��$��Center=�$��0,20,0,0q�&   System.Windows.Controls.DataGrid  -   $��ItemDataGrid��$��2��$��2��   AutoGenerateColumns$ False��$	��150��$��	0,10,0,10q�   CanUserAddRows$ False��   SelectionMode$ Single=�   SelectionUnit$ FullRow=����� *	LineItems+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Item Description��   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Description+  $ Quantity��$	 100  �� *
Quantity+  $ 
Unit Price��$	 100  �� *	UnitPrice+  $ Total��$	 100  �� *Total+�� -   $ DeleteItemsButton��$��3��$��2��$��Delete Selected Item��$	��200��$��Right=�$��10q��� $��4��$��1��$ Vertical=�$��Right=�$��10,5,0,0q�.+��� $ 
Horizontal=�.+��� $��	Subtotal:��$
��Bold"�$��0,0,10,0q��� -   $ SubtotalTextBox��$	��100�� �� 
IsReadOnly$
 True��$��0,0,0,5q��� $ 
Horizontal=�.+��� $��
VAT (15%):��$
��Bold"�$��0,0,10,0q��� -   $ 
VATTextBox��$	��100��$
 True��$��0,0,0,5q��� $ 
Horizontal=�.+��� $��Total Amount:��$
��Bold"�$��0,0,10,0q��� -   $ TotalAmountTextBox��$	��100��$
 True���� $��5��$��2��$ 
Horizontal=�$��Center=�$��	10,0,0,10q�.+��� -   $ 
SaveButton��$
��Save��$	��100��$��5q��� -   $ CancelButton��$��Cancel��$	��100��$��5q�