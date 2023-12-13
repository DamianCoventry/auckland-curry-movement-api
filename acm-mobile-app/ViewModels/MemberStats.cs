using System.ComponentModel;

namespace acm_mobile_app.ViewModels
{
    public class MemberStats : INotifyPropertyChanged
    {
        static public MemberStats? FromModel(acm_models.MemberStats? model)
        {
            if (model == null) return null;
            return new MemberStats()
            {
                _id = model.ID,
                _name = model.Name,
                _currentLevelID = model.CurrentLevelID,
                _membershipCount = model.MembershipCount,
                _isFoundingFather = model.IsFoundingFather != 0,
                _dinnersAttendedCount = model.DinnersAttendedCount,
                _exemptionsAwardedCount = model.ExemptionsAwardedCount,
                _exemptionsReceivedCount = model.ExemptionsReceivedCount,
                _kotCCount = model.KotCCount,
                _reservationOrganiserCount = model.ReservationOrganiserCount,
                _rotYPresenterCount = model.RotYPresenterCount,
                _violationsAwardedCount = model.ViolationsAwardedCount,
                _violationsReceivedCount = model.ViolationsReceivedCount,
            };
        }

        private int? _id;
        private string _name = string.Empty;
        private int _currentLevelID;
        private int _membershipCount;
        private bool _isFoundingFather;
        private int _dinnersAttendedCount;
        private int _exemptionsAwardedCount;
        private int _exemptionsReceivedCount;
        private int _kotCCount;
        private int _reservationOrganiserCount;
        private int _rotYPresenterCount;
        private int _violationsAwardedCount;
        private int _violationsReceivedCount;

        public int? ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public int CurrentLevelID
        {
            get => _currentLevelID;
            set
            {
                if (_currentLevelID != value)
                {
                    _currentLevelID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLevelID)));
                }
            }
        }

        // TODO: How to make this programatic?
        public bool IsAuditor { get => Name == "Damian Coventry" && ID == 6; }

        // TODO: How to make this programatic?
        public bool IsGuru { get => CurrentLevelID == 2; }
        public bool IsMaharaja { get => CurrentLevelID == 3; }

        public int MembershipCount
        {
            get => _membershipCount;
            set
            {
                if (_membershipCount != value)
                {
                    _membershipCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MembershipCount)));
                }
            }
        }

        public bool HasMultipleMemberships { get => MembershipCount > 1; }
        public string MembershipToolTip { get { return "Is a member of " + MembershipCount.ToString() + " curry clubs"; } }

        public bool IsFoundingFather
        {
            get => _isFoundingFather;
            set
            {
                if (_isFoundingFather != value)
                {
                    _isFoundingFather = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFoundingFather)));
                }
            }
        }

        public int DinnersAttendedCount
        {
            get => _dinnersAttendedCount;
            set
            {
                if (_dinnersAttendedCount != value)
                {
                    _dinnersAttendedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DinnersAttendedCount)));
                }
            }
        }

        public bool HasAttendedDinners { get => DinnersAttendedCount > 0; }
        public string DinnersAttendedToolTip { get { return DinnersAttendedCount.ToString() + " " + Plural("dinner", DinnersAttendedCount) + " attended"; } }

        public int ExemptionsAwardedCount
        {
            get => _exemptionsAwardedCount;
            set
            {
                if (_exemptionsAwardedCount != value)
                {
                    _exemptionsAwardedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExemptionsAwardedCount)));
                }
            }
        }

        public bool HasAwardedExemptions { get => ExemptionsAwardedCount > 0; }
        public string ExemptionsAwardedToolTip { get { return ExemptionsAwardedCount.ToString() + " " + Plural("exemption", ExemptionsAwardedCount) + " awarded"; } }

        public int ExemptionsReceivedCount
        {
            get => _exemptionsReceivedCount;
            set
            {
                if (_exemptionsReceivedCount != value)
                {
                    _exemptionsReceivedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExemptionsReceivedCount)));
                }
            }
        }

        public bool HasReceivedExemptions { get => ExemptionsReceivedCount > 0; }
        public string ExemptionsReceivedToolTip { get { return ExemptionsReceivedCount.ToString() + " " + Plural("exemption", ExemptionsReceivedCount) + " received"; } }

        public int KotCCount
        {
            get => _kotCCount;
            set
            {
                if (_kotCCount != value)
                {
                    _kotCCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KotCCount)));
                }
            }
        }

        public bool HasBeenKotC { get => KotCCount > 0; }
        public string KotCToolTip { get { return "Has been KotC " + KotCCount.ToString() + " " + Plural("time", KotCCount); } }

        public int ReservationOrganiserCount
        {
            get => _reservationOrganiserCount;
            set
            {
                if (_reservationOrganiserCount != value)
                {
                    _reservationOrganiserCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReservationOrganiserCount)));
                }
            }
        }

        public bool HasOrganisedReservations { get => ReservationOrganiserCount > 0; }
        public string ReservationOrganiserToolTip { get { return ReservationOrganiserCount.ToString() + " " + Plural("reservation", ReservationOrganiserCount) + " made"; } }

        public int RotYPresenterCount
        {
            get => _rotYPresenterCount;
            set
            {
                if (_rotYPresenterCount != value)
                {
                    _rotYPresenterCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RotYPresenterCount)));
                }
            }
        }

        public bool HasPresentedRotY { get => RotYPresenterCount > 0; }
        public string RotYPresenterToolTip { get { return "Presented RotY award " + RotYPresenterCount.ToString() + " " + Plural("time", RotYPresenterCount); } }

        public int ViolationsAwardedCount
        {
            get => _violationsAwardedCount;
            set
            {
                if (_violationsAwardedCount != value)
                {
                    _violationsAwardedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViolationsAwardedCount)));
                }
            }
        }

        public bool HasAwardedViolations { get => ViolationsAwardedCount > 0; }
        public string ViolationsAwardedToolTip { get { return ViolationsAwardedCount.ToString() + " " + Plural("violation", ViolationsAwardedCount) + " awarded"; } }

        public int ViolationsReceivedCount
        {
            get => _violationsReceivedCount;
            set
            {
                if (_violationsReceivedCount != value)
                {
                    _violationsReceivedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViolationsReceivedCount)));
                }
            }
        }

        public bool HasReceivedViolations { get => ViolationsReceivedCount > 0; }
        public string ViolationsReceivedToolTip { get { return ViolationsReceivedCount.ToString() + " " + Plural("violation", ViolationsReceivedCount) + " received"; } }

        public event PropertyChangedEventHandler? PropertyChanged;

        private static string Plural(string singularText, int count, string suffix = "s")
        {
            return count == 1 ? singularText : singularText + suffix;
        }
    }
}
