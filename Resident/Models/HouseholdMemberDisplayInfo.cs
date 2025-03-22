using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resident.Models
{
    public class HouseholdMemberDisplayInfo
    {

        public string FullName { get; set; }
        public string IdentityCard { get; set; }
        public string Relationship { get; set; }
        private bool _isSelected;
        private bool _isNewHead;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public bool IsNewHead
        {
            get { return _isNewHead; }
            set
            {
                if (_isNewHead != value)
                {
                    _isNewHead = value;
                    OnPropertyChanged(nameof(IsNewHead));

                    // Nếu IsNewHead được set, bỏ chọn các thành viên khác (nếu cần).
                    if (_isNewHead)
                    {
                        IsSelected = true; // Đảm bảo IsSelected cũng được chọn khi IsNewHead được chọn
                    }
                }
            }
        }

        public int UserId { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
