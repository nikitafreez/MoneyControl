   M S B A M L   `   `   `  ���� U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  G CMoneyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MoneyControlMoneyControl    MoneyControl.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     ,localclr-namespace:MoneyControl     -     � Title$  	MoneyStat��$��455.133��$��547.624�� � WindowStartupLocation$ CenterScreen=�.��� $��	0,0,3.4,0q� � ColumnDefinitions .+��� $
��Left=�$	��420��$	��Top=�$	��538��$��2��.@��� $��Главная��$��Century Gothic)�.��� ���	#FFF6F6F6$��0,0,0,-6q�.+��� -    ��Name$ categoryComboBox��$
��Left=�$��
26,121,0,0q�$��Century Gothic)�$	��Top=�$	��201��   LiveCharts.Wpf.PieChart  -   $��pieChart��$��Right=�$	��250��$��0,60,0.2,60q�$��Center=�$	��250��   StartingRotationAngle$ 0Q�$��1���� $)��#Выберите категорию��$
��Left=�$��	26,84,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��201���� $��Сумма траты��$
��Left=�$��
26,142,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��201���� -    ��Name$ costTextBox��$
��Left=�$��23�� �� TextWrapping$
 Wrap=�$	��Top=�$	��201��$��
26,174,0,0q�$��Century Gothic)�$��16(��� -    ��Name$ addCostButton��$��Добавить��$
��Left=�$��
80,202,0,0q�$	��Top=�$	��100��$��32��$��18(��� $��Личный счёт:��$
��Left=�$��
114,10,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��123���� -    ��Name$ totalMoneyLabel��$��
0000000000��$
��Left=�$��
242,10,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��114���� -   $ currencyLabel1��$	��{$}��$
��Left=�$��227,170,0,0q�$	��Top=�$��Century Gothic)�$��16(�$��31��$��32���� -   $ ChooseTimeDay��$��За день��$
��Left=�$��282,326,0,0q�$	��Top=�$	��114��$��28��$��16(��� -	   $ ChooseTimeMonth��$��За месяц��$
��Left=�$��408,326,0,0q�$	��Top=�$	��114��$��28��$��16(��� -
   $ ChooseTimeYear��$��За год��$
��Left=�$��282,359,0,0q�$	��Top=�$	��114��$��28��$��16(��� -   $ ChooseTimeAll��$��За всё время��$
��Left=�$��408,359,0,0q�$	��Top=�$	��114��$��28��$��16(��� $��Категории��$��Century Gothic)�$��21��$��-2.4,0,-2.4,-0.2q�.��� ���	#FFF6F6F6 �� $
��101* ��� $
��431* �.+��� $)��#Добавить категорию��$
��Left=�$��	32,65,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��201��$��2���� -   $ newCategoryTextBox��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��201��$��	32,97,0,0q�$��Century Gothic)�$��16(�$��2���� -   $ addCategoryButton��$��Добавить��$
��Left=�$��
78,125,0,0q�$	��Top=�$	��100��$��32��$��18(�$��2���� -   $ categoryComboBox2��$
��Left=�$��177.033,97,0,0q�$��Century Gothic)�$	��Top=�$	��201��$��1���� $'��!Удалить категорию��$
��Left=�$��177.033,65,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��201��$��1���� -   $ deleteCategoryButton��$��Удалить��$
��Left=�$��231.033,125,0,0q�$	��Top=�$	��100��$��32��$��18(�$��1���� $��Пользователь��$��Century Gothic)�.��� ���	#FFF6F6F6 �� .+��� -   $ 
helloLabel��$F��@Личный кабинет пользователя {UserLogin}��$��Center=�$��109,10,89,0q�$	��Top=�$��16(�$��30��$	��334���� -   $ monthCostsLabel��$5��/Трат в этом месяце: {MonthSpend}��$
��Left=�$��	10,80,0,0q�$	��Top=�$��16(�$	��293��$��30���� $,��&Изменить личный счёт��$
��Left=�$��
10,123,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��210���� -   $ totalMoneyTextBox��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��201��$��
10,155,0,0q�$��Century Gothic)�$��16(��� -   $ updateTotalMoneyButton��$��Изменить��$
��Left=�$��
61,183,0,0q�$	��Top=�$	��100��$��32��$��18(��� -   $ toExcelButton��$!��Выгрузить в Excel��$
��Left=�$��326,339,0,0q�$	��Top=�$	��183��$��32��$��18(��� -   $ currencyComboBox��$
��Left=�$��308,155,0,0q�$��Century Gothic)�$	��Top=�$	��201��.@�	� -   	 	�Name$	 RubCurrenceRadioButton��$��Рубль | RUB | ₽��	� -   $	 UsdCurrenceRadioButton��$��Доллар | USD | $��
 d� 	IsChecked$
 FalseY�	� -   $	 EurCurrenceRadioButton��$��Евро | EUR | €��$
 FalseY�	� -   $	 JpyCurrenceRadioButton��$��Йена | JPY | ¥��$
 FalseY��� $#��Выберите валюту��$
��Left=�$��308,123,0,0q�$	��Top=�$��Century Gothic)�$��18(�$	��201���� -   $ exitAccountButton��$&�� Выйти из аккаунта��$
��Left=�$��
22,339,0,0q�$	��Top=�$	��173��$��32��$��18(��� -   $ currencyLabel2��$	��{$}��$
��Left=�$��211,151,0,0q�$	��Top=�$��Century Gothic)�$��16(�$��31��$��32��