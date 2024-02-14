// using System;
//
// namespace UNO_Server.ViewModel;
//
// public class CardViewModel : ViewModelBase
// {
//     public int Id { get; set; }
//     private string _value;
//     private string _color;
//     private string _imageUri;
//
//     public string Value
//     {
//         get => _value;
//         set
//         {
//             if (value == _value) return;
//             _value = value;
//             OnPropertyChanged();
//         }
//     }
//
//     public string Color
//     {
//         get => _color;
//         set
//         {
//             if (value == _color) return;
//             _color = value;
//             OnPropertyChanged();
//         }
//     }
//
//     public string ImageUri
//     {
//         get => _imageUri;
//         set
//         {
//             if (Equals(value, _imageUri)) return;
//             _imageUri = value;
//             OnPropertyChanged();
//         }
//     }
// }