using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class ManageMembersPage : ContentPage
    {
        private ObservableCollection<MemberViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public ManageMembersPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public int PageSize { get; set; } = 10;

        public ObservableCollection<MemberViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var member in value)
                {
                    var copy = new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        SponsorID = member.SponsorID,
                        Sponsor = member.Sponsor,
                        CurrentLevelID = member.CurrentLevelID,
                        CurrentLevel = member.CurrentLevel,
                        AttendanceCount = member.AttendanceCount,
                        IsArchived = member.IsArchived,
                        ArchiveReason = member.ArchiveReason,
                    };
                    _members.Add(copy);
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
        }

        private async void MemberItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is MemberViewModel memberViewModel)
            {
                Dictionary<string, object> parameters = new() { { "Member", memberViewModel } };
                await Shell.Current.GoToAsync("edit_member", true, parameters);
            }
        }

        private async Task LoadData()
        {
            try
            {
                LoadingIndicator.IsRunning = true;
                MemberItems.IsEnabled = false;

                await Task.Delay(750);

                var level1 = new LevelViewModel()
                {
                    ID = 1,
                    Name = "Nausikhie",
                    Description = "A level new to and inexperienced in the club.",
                    RequiredAttendances = 0,
                };
                var level2 = new LevelViewModel()
                {
                    ID = 2,
                    Name = "Shuruaatee",
                    Description = "A level just starting to learn curry and take part in the club.",
                    RequiredAttendances = 10,
                };
                var level3 = new LevelViewModel()
                {
                    ID = 3,
                    Name = "Pratiyogee",
                    Description = "A level who actively takes part in the club.",
                    RequiredAttendances = 25,
                };
                var level4 = new LevelViewModel()
                {
                    ID = 4,
                    Name = "Peshevar",
                    Description = "Engaged in the club as one's main vocation rather than as a pastime.",
                    RequiredAttendances = 50,
                };
                var level5 = new LevelViewModel()
                {
                    ID = 6,
                    Name = "Visheshagy",
                    Description = "A level who is very knowledgeable about curry and the club.",
                    RequiredAttendances = 100,
                };

                var pretendRdbms = new List<MemberViewModel>() {
                    new() { ID = 1, Name = "Alfred Sanchez", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 2, Name = "Kyle Scott", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 3, Name = "Kaiser Farley", SponsorID = null, Sponsor = null, CurrentLevelID = 2, CurrentLevel = level2 },
                    new() { ID = 4, Name = "Reginald Weber", SponsorID = null, Sponsor = null, CurrentLevelID = 3, CurrentLevel = level3 },
                    new() { ID = 5, Name = "Cullen Schmitt", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 6, Name = "Kenneth Allen", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 7, Name = "Jaxtyn Erickson", SponsorID = null, Sponsor = null, CurrentLevelID = 3, CurrentLevel = level3 },
                    new() { ID = 8, Name = "Barrett Herrera", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 9, Name = "Lee Dickerson", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 10, Name = "Edwin Spears", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 11, Name = "Luke Wood", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 12, Name = "Ben Vega", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 13, Name = "Marlowe Ho", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 14, Name = "Jaxton Hopkins", SponsorID = null, Sponsor = null, CurrentLevelID = 3, CurrentLevel = level3 },
                    new() { ID = 15, Name = "Allen Rangel", SponsorID = null, Sponsor = null, CurrentLevelID = 2, CurrentLevel = level2 },
                    new() { ID = 16, Name = "Fisher Willis", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 17, Name = "Terrance Cain", SponsorID = null, Sponsor = null, CurrentLevelID = 3, CurrentLevel = level3 },
                    new() { ID = 18, Name = "Alonso Pratt", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 19, Name = "Sonny Brandt", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 20, Name = "Asher Gates", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 21, Name = "Bruno Holmes", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 22, Name = "Hunter Gonzalez", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 23, Name = "Kendrick Shields", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 24, Name = "Braydon Brooks", SponsorID = null, Sponsor = null, CurrentLevelID = 2, CurrentLevel = level2 },
                    new() { ID = 25, Name = "Thomas Norris", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 26, Name = "Gage Suarez", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 27, Name = "Kolby Stone", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 28, Name = "Eden Berger", SponsorID = null, Sponsor = null, CurrentLevelID = 2, CurrentLevel = level2 },
                    new() { ID = 29, Name = "Memphis Blake", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 30, Name = "Eugene Callahan", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 31, Name = "Landin Mayer", SponsorID = null, Sponsor = null, CurrentLevelID = 4, CurrentLevel = level4 },
                    new() { ID = 32, Name = "Reginald Cervantes", SponsorID = null, Sponsor = null, CurrentLevelID = 1, CurrentLevel = level1 },
                    new() { ID = 33, Name = "Trevon Bird", SponsorID = null, Sponsor = null, CurrentLevelID = 5, CurrentLevel = level5 },
                    new() { ID = 34, Name = "Mauricio Patterson", SponsorID = null, Sponsor = null, CurrentLevelID = 3, CurrentLevel = level3 },
                };

                var ff1 = pretendRdbms.Find(x => x.ID == 8);
                var ff2 = pretendRdbms.Find(x => x.ID == 9);
                var ff3 = pretendRdbms.Find(x => x.ID == 11);
                var ff4 = pretendRdbms.Find(x => x.ID == 18);
                var ff5 = pretendRdbms.Find(x => x.ID == 19);
                var ff6 = pretendRdbms.Find(x => x.ID == 26);
                var ff7 = pretendRdbms.Find(x => x.ID == 33);

                var temp = pretendRdbms.Find(x => x.ID == 1);
                if (temp != null && ff1 != null)
                {
                    temp.SponsorID = ff1.ID; temp.Sponsor = ff1;
                }
                temp = pretendRdbms.Find(x => x.ID == 2);
                if (temp != null && ff1 != null)
                {
                    temp.SponsorID = ff1.ID; temp.Sponsor = ff1;
                }
                temp = pretendRdbms.Find(x => x.ID == 3);
                if (temp != null && ff1 != null)
                {
                    temp.SponsorID = ff1.ID; temp.Sponsor = ff1;
                }
                temp = pretendRdbms.Find(x => x.ID == 4);
                if (temp != null && ff1 != null)
                {
                    temp.SponsorID = ff1.ID; temp.Sponsor = ff1;
                }

                temp = pretendRdbms.Find(x => x.ID == 5);
                if (temp != null && ff2 != null)
                {
                    temp.SponsorID = ff2.ID; temp.Sponsor = ff2;
                }
                temp = pretendRdbms.Find(x => x.ID == 6);
                if (temp != null && ff2 != null)
                {
                    temp.SponsorID = ff2.ID; temp.Sponsor = ff2;
                }
                temp = pretendRdbms.Find(x => x.ID == 7);
                if (temp != null && ff2 != null)
                {
                    temp.SponsorID = ff2.ID; temp.Sponsor = ff2;
                }
                temp = pretendRdbms.Find(x => x.ID == 10);
                if (temp != null && ff2 != null)
                {
                    temp.SponsorID = ff2.ID; temp.Sponsor = ff2;
                }
                temp = pretendRdbms.Find(x => x.ID == 12);
                if (temp != null && ff2 != null)
                {
                    temp.SponsorID = ff2.ID; temp.Sponsor = ff2;
                }

                temp = pretendRdbms.Find(x => x.ID == 13);
                if (temp != null && ff3 != null)
                {
                    temp.SponsorID = ff3.ID; temp.Sponsor = ff3;
                }
                temp = pretendRdbms.Find(x => x.ID == 14);
                if (temp != null && ff3 != null)
                {
                    temp.SponsorID = ff3.ID; temp.Sponsor = ff3;
                }
                temp = pretendRdbms.Find(x => x.ID == 15);
                if (temp != null && ff3 != null)
                {
                    temp.SponsorID = ff3.ID; temp.Sponsor = ff3;
                }
                temp = pretendRdbms.Find(x => x.ID == 16);
                if (temp != null && ff3 != null)
                {
                    temp.SponsorID = ff3.ID; temp.Sponsor = ff3;
                }

                temp = pretendRdbms.Find(x => x.ID == 17);
                if (temp != null && ff4 != null)
                {
                    temp.SponsorID = ff4.ID; temp.Sponsor = ff4;
                }
                temp = pretendRdbms.Find(x => x.ID == 20);
                if (temp != null && ff4 != null)
                {
                    temp.SponsorID = ff4.ID; temp.Sponsor = ff4;
                }
                temp = pretendRdbms.Find(x => x.ID == 21);
                if (temp != null && ff4 != null)
                {
                    temp.SponsorID = ff4.ID; temp.Sponsor = ff4;
                }
                temp = pretendRdbms.Find(x => x.ID == 22);
                if (temp != null && ff4 != null)
                {
                    temp.SponsorID = ff4.ID; temp.Sponsor = ff4;
                }

                temp = pretendRdbms.Find(x => x.ID == 23);
                if (temp != null && ff5 != null)
                {
                    temp.SponsorID = ff5.ID; temp.Sponsor = ff5;
                }
                temp = pretendRdbms.Find(x => x.ID == 24);
                if (temp != null && ff5 != null)
                {
                    temp.SponsorID = ff5.ID; temp.Sponsor = ff5;
                }
                temp = pretendRdbms.Find(x => x.ID == 25);
                if (temp != null && ff5 != null)
                {
                    temp.SponsorID = ff5.ID; temp.Sponsor = ff5;
                }
                temp = pretendRdbms.Find(x => x.ID == 27);
                if (temp != null && ff5 != null)
                {
                    temp.SponsorID = ff5.ID; temp.Sponsor = ff5;
                }

                temp = pretendRdbms.Find(x => x.ID == 28);
                if (temp != null && ff6 != null)
                {
                    temp.SponsorID = ff6.ID; temp.Sponsor = ff6;
                }
                temp = pretendRdbms.Find(x => x.ID == 29);
                if (temp != null && ff6 != null)
                {
                    temp.SponsorID = ff6.ID; temp.Sponsor = ff6;
                }
                temp = pretendRdbms.Find(x => x.ID == 30);
                if (temp != null && ff6 != null)
                {
                    temp.SponsorID = ff6.ID; temp.Sponsor = ff6;
                }
                temp = pretendRdbms.Find(x => x.ID == 31);
                if (temp != null && ff6 != null)
                {
                    temp.SponsorID = ff6.ID; temp.Sponsor = ff6;
                }

                temp = pretendRdbms.Find(x => x.ID == 32);
                if (temp != null && ff7 != null)
                {
                    temp.SponsorID = ff7.ID; temp.Sponsor = ff7;
                }
                temp = pretendRdbms.Find(x => x.ID == 34);
                if (temp != null && ff7 != null)
                {
                    temp.SponsorID = ff7.ID; temp.Sponsor = ff7;
                }

                _totalPages = pretendRdbms.Count / PageSize;
                if (pretendRdbms.Count % PageSize > 0)
                    _totalPages++;
                _currentPage = Math.Min(Math.Max(0, _currentPage), _totalPages - 1);

                LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;

                MergePageIntoListView(pretendRdbms.Skip(_currentPage * PageSize).Take(PageSize).ToList());

                await Task.Delay(750);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                MemberItems.IsEnabled = true;
            }
        }

        private void MergePageIntoListView(List<MemberViewModel> pageOfMembers)
        {
            MemberItems.BatchCommit();
            foreach (var member in pageOfMembers)
            {
                var x = _members.FirstOrDefault(y => y.ID == member.ID);
                if (x == null)
                {
                    _members.Add(new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        SponsorID = member.SponsorID,
                        Sponsor = member.Sponsor,
                        CurrentLevelID = member.CurrentLevelID,
                        CurrentLevel = member.CurrentLevel,
                        AttendanceCount = member.AttendanceCount,
                        IsArchived = member.IsArchived,
                        ArchiveReason = member.ArchiveReason,
                    });
                }
            }
            MemberItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await LoadData(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
