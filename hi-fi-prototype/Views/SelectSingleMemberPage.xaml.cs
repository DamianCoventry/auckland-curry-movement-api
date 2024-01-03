using CommunityToolkit.Maui.Alerts;
using hi_fi_prototype.Services;
using hi_fi_prototype.ViewModels;
using System.Collections.ObjectModel;

namespace hi_fi_prototype.Views
{
    public partial class SelectSingleMemberPage : ContentPage
    {
        private const int MIN_REFRESH_TIME_MS = 500;
        private ObservableCollection<MemberViewModel> _members = [];
        private int _totalPages = 0;
        private int _currentPage = 0;

        public SelectSingleMemberPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public Action<MemberViewModel>? AcceptFunction { get; set; } = null;
        public Action? CancelFunction { get; set; } = null;
        public MemberViewModel? SelectedMember { get; set; } = null;
        private MemberViewModel? OriginalSelection { get; set; } = null;
        public int PageSize { get; set; } = 10;

        public ObservableCollection<MemberViewModel> Members
        {
            get => _members;
            set
            {
                _members = [];
                foreach (var member in value)
                {
                    _members.Add(new MemberViewModel()
                    {
                        ID = member.ID,
                        Name = member.Name,
                        ArchiveReason = member.ArchiveReason,
                        IsArchived = member.IsArchived,
                    });
                }
                OnPropertyChanged(nameof(Members));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (SelectedMember != null)
            {
                OriginalSelection = new MemberViewModel()
                {
                    ID = SelectedMember.ID,
                    Name = SelectedMember.Name,
                    ArchiveReason = SelectedMember.ArchiveReason,
                    IsArchived = SelectedMember.IsArchived,
                };
            }

            MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
        }

        private static IAcmService AcmService
        {
            get { return ((AppShell)Shell.Current).AcmService; }
        }

        private async Task DownloadSinglePageOfMembers()
        {
            try
            {
                var startTime = DateTime.Now;

                LoadingIndicator.IsRunning = true;
                AcceptChanges.IsEnabled = false;
                DiscardChanges.IsEnabled = false;
                LoadMoreButton.IsEnabled = false;

                // TODO: Get the club ID from somewhere
                var pageOfData = await AcmService.ListClubMembersAsync(1, _currentPage * PageSize, PageSize);
                if (pageOfData != null && pageOfData.PageItems != null)
                {
                    _totalPages = pageOfData.TotalPages;
                    LoadMoreButton.IsVisible = _currentPage < _totalPages - 1;
                    MergePageIntoListView(pageOfData.PageItems);
                }

                if (OriginalSelection != null)
                {
                    foreach (var member in Members)
                    {
                        if (OriginalSelection != null && OriginalSelection.ID == member.ID && OriginalSelection.Name == member.Name)
                        {
                            OriginalSelection = null;
                            MemberItems.SelectedItem = member;
                        }
                    }
                }

                var elapsed = DateTime.Now - startTime;
                if (elapsed.Milliseconds < MIN_REFRESH_TIME_MS)
                    await Task.Delay(MIN_REFRESH_TIME_MS - elapsed.Milliseconds);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.ToString(), "Close");
            }
            finally
            {
                LoadingIndicator.IsRunning = false;
                AcceptChanges.IsEnabled = true;
                DiscardChanges.IsEnabled = true;
                LoadMoreButton.IsEnabled = true;
            }
        }

        private async void DiscardChanges_Clicked(object sender, EventArgs e)
        {
            CancelFunction?.Invoke();
            await Navigation.PopAsync();
        }

        private async void AcceptChanges_Clicked(object sender, EventArgs e)
        {
            if (MemberItems.SelectedItem == null)
            {
                var toast = Toast.Make("Please select a member");
                await toast.Show();
                return;
            }

            if (MemberItems.SelectedItem is MemberViewModel x)
            {
                SelectedMember = x;
                AcceptFunction?.Invoke(SelectedMember);
                await Navigation.PopAsync();
            }
        }

        private void MergePageIntoListView(List<acm_models.Member> pageOfMembers)
        {
            MemberItems.BatchBegin();
            foreach (var member in pageOfMembers)
            {
                var exists = _members.FirstOrDefault(y => y.ID == member.ID);
                if (exists == null)
                {
                    var vm = MemberViewModel.FromModel(member);
                    if (vm != null)
                        _members.Add(vm);
                }
            }
            MemberItems.BatchCommit();
        }

        private void LoadMoreButton_Clicked(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages - 1)
            {
                ++_currentPage;
                MainThread.BeginInvokeOnMainThread(async () => { await DownloadSinglePageOfMembers(); });
            }
        }

        private async void ManageNotifications_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("manage_notifications");
        }
    }
}
