using Domain.enums;
using Domain.Klase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using static System.Reflection.Metadata.BlobBuilder;
using Path = System.IO.Path;

namespace WitcherBooks
{
    /// <summary>
    /// Interaction logic for BookEditWindow.xaml
    /// </summary>
    public partial class BookEditWindow : Window
    {
        private Book currentBook;
        public ObservableCollection<Book> books { get; set; }
        public DataIO serializer = new DataIO();
        private MediaPlayer _bgMusicPlayer;
        public BookEditWindow(Book book)
        {
            InitializeComponent();
            InitializeBackgroundMusic();
            currentBook = book;

            books = serializer.DeSerializeObject<ObservableCollection<Book>>("Books.xml");


            if (books == null)
            {
                books = new ObservableCollection<Book>();
            }
            DataContext = this;

            
            TitleBox.Text = currentBook.Title;
            DateBlock.Text = $"Added on: {currentBook.Date}";
            ImagePreview.Source = new BitmapImage(new Uri(book.Imgpath));

            string filePath = book.Rtfpath;
            if (File.Exists(filePath))
            {
                using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                TextRange range = new TextRange(ContentBox.Document.ContentStart, ContentBox.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
            }
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;
            string imagesFolder = Path.Combine(AppContext.BaseDirectory, "images");
            openDialog.InitialDirectory = imagesFolder;

            bool? result = openDialog.ShowDialog();

            if (result == true)
            {
                string selectedFile = openDialog.FileName;


                if (selectedFile.StartsWith(imagesFolder, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        ImagePreview.Source = new BitmapImage(new Uri(selectedFile));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load image:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an image from the allowed images folder only.", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        private void SaveBT_Click(object sender, RoutedEventArgs e)
        {

           
            bool hasError = false;

            
            titleError.Visibility = Visibility.Collapsed;
            imageError.Visibility = Visibility.Collapsed;
            descError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                titleError.Visibility = Visibility.Visible;
                hasError = true;
            }

            
            if (ImagePreview.Source == null)
            {
                imageError.Visibility = Visibility.Visible;
                hasError = true;
            }

            
            TextRange textRange = new TextRange(ContentBox.Document.ContentStart, ContentBox.Document.ContentEnd);
            if (string.IsNullOrWhiteSpace(textRange.Text.Trim()))
            {
                descError.Visibility = Visibility.Visible;
                hasError = true;
            }

            
            if (hasError)
                return;

            var result = MessageBox.Show("Are you sure you want to change this item?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _bgMusicPlayer.IsMuted = true;
                currentBook.Title = TitleBox.Text;

                string filePath = $"rtfdata/{currentBook.Id}.rtf";
                using FileStream fs = new FileStream(filePath, FileMode.Create);
                TextRange range = new TextRange(ContentBox.Document.ContentStart, ContentBox.Document.ContentEnd);
                range.Save(fs, DataFormats.Rtf);

                foreach (Book b in books)
                {
                    if (b.Id == currentBook.Id)
                    {
                        b.Title = TitleBox.Text;
                        b.Imgpath = ((BitmapImage)ImagePreview.Source).UriSource.LocalPath;
                    }
                }

                serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
                tableWindow tw = new tableWindow(UserType.Administrator);
                tw.Show();
                this.Close();
            }
            else 
            {
                return;
            }
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
            tableWindow tw = new tableWindow(UserType.Administrator);
            tw.Show();
            this.Close();
            
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, ContentBox);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, ContentBox);
        }

        private void FontSizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeBox.SelectedItem is ComboBoxItem selectedItem)
            {
                double size = Convert.ToDouble(selectedItem.Content);
                ContentBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            ContentBox.Selection.ApplyPropertyValue(
                TextElement.ForegroundProperty,
                new SolidColorBrush(Colors.Black));
        }
        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null, ContentBox);
        }
        private void FontFamilyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string font = selectedItem.Content.ToString();
                ContentBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(font));
            }
        }
        private void InitializeBackgroundMusic()
        {
            _bgMusicPlayer = new MediaPlayer();
            _bgMusicPlayer.Open(new Uri("music/bg1.mp3", UriKind.Relative)); // Update the path accordingly
            _bgMusicPlayer.Volume = 0.01; // Full volume
            _bgMusicPlayer.MediaEnded += (s, e) => _bgMusicPlayer.Position = TimeSpan.Zero; // Loop
            _bgMusicPlayer.Play();
        }
        private void MuteButton_Checked(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
        }
        private void MuteButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = false;
        }

        private void TitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                titleError.Visibility = Visibility.Collapsed;
            }
        }
    }
}
