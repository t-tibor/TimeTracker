using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Data;
using TimeTracker.Models;

namespace TimeTracker
{
    public partial class MainWindowViewModel: ObservableObject
    {
        private TimeTrackerContext _context;

        #region Public Properties

        [ObservableProperty]
        private ObservableCollection<Project> allProjects;
        
        [ObservableProperty]
        private ObservableCollection<WorkLog> allWorkLogs;

        [ObservableProperty]
        private ICollectionView allWorkLogsView;        

        [ObservableProperty]
        private string statusMessage;

        // DailyReport
        [ObservableProperty]
        private DateTime dailyReportTargetDate = DateTime.Now;

        [ObservableProperty]
        private List<(int, double)> dailyReport;


        public DateTime NewWorkItemDateTime { get; set; } = DateTime.Now;
        public TimeTracker.Models.Task NewWorkItemTask { get; set; }

        [ObservableProperty]
        private WorkLog selectedWorkItem;
        #endregion

        public MainWindowViewModel() { }

        partial void OnAllWorkLogsChanged(ObservableCollection<WorkLog> value)
        {
            AllWorkLogsView = CollectionViewSource.GetDefaultView(value);
            AllWorkLogsView.SortDescriptions.Clear();
            AllWorkLogsView.SortDescriptions.Add(new SortDescription(nameof(WorkLog.WorkDay), ListSortDirection.Descending));
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task ReloadDb()
        {
            _context?.Dispose();
            this.StatusMessage = "Connection to DB...";
            _context = new TimeTrackerContext();
            await _context.Projects.LoadAsync();
            await _context.WorkLogs.LoadAsync();
            await _context.Tasks.LoadAsync();
            AllProjects = _context.Projects.Local.ToObservableCollection();
            AllWorkLogs = _context.WorkLogs.Local.ToObservableCollection();
            this.StatusMessage = "DB readload completed";
            await System.Threading.Tasks.Task.Delay(1000);
            this.StatusMessage = string.Empty;            
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task SaveToDb()
        {
            this.StatusMessage = "Saving to DB...";
            await System.Threading.Tasks.Task.Delay(100);
            try
            {
                _context.ChangeTracker.DetectChanges();

                await _context.SaveChangesAsync();
                this.StatusMessage = "Saving successfull.";
                await System.Threading.Tasks.Task.Delay(1000);
            }
            catch(Exception ex) when (ex is DbUpdateException || ex is DBConcurrencyException)
            {
                this.StatusMessage = "DB update FAILED.";
                await System.Threading.Tasks.Task.Delay(2000);
            }
            finally
            {
                this.StatusMessage = string.Empty;
            }
        }

        [RelayCommand]
        private void AddNewWorkItem()
        {
            if(this.NewWorkItemTask != null)
            {
                var today = DateOnly.FromDateTime(NewWorkItemDateTime);
                
                try
                {
                    _context.WorkLogs.Add(new WorkLog() { 
                        WorkDay = today,
                        Task = this.NewWorkItemTask
                    });
                }
                catch(System.InvalidOperationException _ex)
                {
                    MessageBox.Show($"Cannot add new work item: {_ex.Message}");
                }
            }
        }

        [RelayCommand]
        private void RemoveSelectedWorkItem()
        {
            if(this.SelectedWorkItem != null)
            {
                _context.WorkLogs.Remove(this.SelectedWorkItem);
            }
        }

        [RelayCommand]
        void UpdateDailyReport()
        {
            this.DailyReport = AllWorkLogs
                .Where(log => (log.WorkDay.Month == DailyReportTargetDate.Month) && (log.WorkDay.Year == DailyReportTargetDate.Year))
                .GroupBy(log => log.WorkDay)
                .Select(group => (Day: group.Key.Day, SumWorkHours: group.Sum(log => log.Duration)))
                .ToList();

        }
    }
}
