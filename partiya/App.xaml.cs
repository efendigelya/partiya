using Microsoft.Maui.Controls;

namespace partiya
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Устанавливаем NavigationPage с белым текстом заголовка
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Colors.Black, // Цвет фона навигационного бара
                BarTextColor = Colors.White // Цвет текста заголовка
            };
        }
    }
}
