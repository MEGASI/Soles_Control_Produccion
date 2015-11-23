using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mindscape.WpfElements;


namespace CrtProduccion.vistas
{
    /// <summary>
    /// Interaction logic for Colordiag.xaml
    /// </summary>
    public partial class Colordiag : Window
    {
        public Colordiag()
        {
            InitializeComponent();



           // CanChangePalette = true;
            

            DataContext = this;
        }



        #region SelectedColor Property

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
          DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
          new FrameworkPropertyMetadata(StandardPalettes.OfficePalette[38].Color));

        #endregion // SelectedColor Property

        #region Palette Property

        public IList<NamedColor> Palette
        {
            get { return (IList<NamedColor>)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        public static readonly DependencyProperty PaletteProperty =
          DependencyProperty.Register("Palette", typeof(IList<NamedColor>), typeof(ColorPicker),
          new FrameworkPropertyMetadata(StandardPalettes.OfficePalette));
   
        #endregion // Palette Property
    
       
        
    }
}
    