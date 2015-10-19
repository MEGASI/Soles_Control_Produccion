using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CrtProduccion.comunes
{
    class libreria
    {
        public static void limpiaControles(DependencyObject obj)
        {
            // Limpia todos los conbroles de un contenedor
            TextBox tb = obj as TextBox;
            RadioButton rb = obj as RadioButton;
            PasswordBox pb = obj as PasswordBox;
            CheckBox ch = obj as CheckBox;
            
            if (tb != null) tb.Text = "";
            if (rb != null) rb.IsChecked = false;
            if (pb != null) pb.Password = "";
            if (ch != null) ch.IsEnabled = false;
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj as DependencyObject); i++)
                limpiaControles(VisualTreeHelper.GetChild(obj, i));
        }

        public static void estadoControles(DependencyObject obj, bool isenabled)
        {
            // habilita todos los controles de estas clase
            TextBox tb = obj as TextBox;
            RadioButton rb = obj as RadioButton;
            PasswordBox pb = obj as PasswordBox;
            ComboBox cb = obj as ComboBox;
            CheckBox ch = obj as CheckBox;

            if (tb != null) tb.IsEnabled = isenabled;
            if (rb != null) rb.IsEnabled = isenabled;
            if (pb != null) pb.IsEnabled = isenabled;
            if (cb != null) cb.IsEnabled = isenabled;
            if (ch != null) ch.IsEnabled = isenabled;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj as DependencyObject); i++)
                estadoControles(VisualTreeHelper.GetChild(obj, i), isenabled);
        }
        public static void Next_if_Enter(object sender, KeyEventArgs e)
        {
            // Hace que la tecla enter pase al siguiente control.
            TextBox s = e.Source as TextBox;
            if (s != null)
            {
                if (e.Key == Key.Enter)
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
        //  validar solo Numero
        public static bool soloNumero(string pValor, System.Windows.Input.KeyEventArgs e)
        {
            bool ret = true;
            if (e.Key != System.Windows.Input.Key.Enter)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(pValor, "[^0-9]"))
                {
                    MessageBox.Show("Solo Admiten Numero");
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
        }
        //  validar solo Letras
        public static bool SoloLetra(string pValor, System.Windows.Input.KeyEventArgs e)
        {
            bool ret = true;
            if (e.Key != System.Windows.Input.Key.Enter)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(pValor, "[^A-Z]"))
                {
                    MessageBox.Show("Solo Admiten Letras");
                }
                else
                {
                    ret = false;
                }
            }
            return ret;
     }   
  }
}

