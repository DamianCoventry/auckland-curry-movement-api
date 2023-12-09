using System.Collections.ObjectModel;

namespace acm_mobile_app.Utils
{
    public static class MemberUtils
    {
        public static List<acm_models.Member> ToModelsMemberList(ObservableCollection<ViewModels.Member> members)
        {
            List<acm_models.Member> x = [];
            foreach (var viewModel in members)
            {
                x.Add(new acm_models.Member()
                {
                    ID = viewModel.ID,
                    Name = viewModel.Name,
                    SponsorID = viewModel.SponsorID,
                    CurrentLevelID = viewModel.CurrentLevelID,
                    AttendanceCount = viewModel.AttendanceCount,
                });
            }
            return x;
        }
    }
}
