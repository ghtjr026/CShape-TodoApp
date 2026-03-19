using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoApp.Presentation.ViewModel
{
    /// <summary>
    /// 모든 ViewModel의 기반 클래스. INotifyPropertyChanged 구현.
    /// SetProperty로 값 변경 시에만 이벤트 발생 → 불필요한 UI 갱신 방지.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
