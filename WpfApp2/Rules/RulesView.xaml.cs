using System.Windows;
using System.Windows.Controls;
using WpfApp2.Rules;

namespace WpfApp2;

public partial class RulesView : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(RulesViewModel), typeof(RulesView), new PropertyMetadata(default(RulesViewModel)));

    public RulesViewModel ViewModel
    {
        get => (RulesViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public RulesView()
    {
        InitializeComponent();
    }
}