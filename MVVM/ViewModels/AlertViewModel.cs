using GamingAlerts.MVVM.Models;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using GamingAlerts.Managers;
using System.ComponentModel;
using GamingAlerts.Helpers;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GamingAlerts.MVVM.ViewModels
{
    public class AlertViewModel : ObservableObject
    {
		public Alert Alert { get; set; }

		public ICommand ChangeAlertStatusCommand { get; }
		public ICommand ForceStartAlertCommand { get; }
		public ICommand EditButtonAlertCommand { get; }
		public ICommand DeleteButtonAlertCommand { get; }

		public ICommand ChangeIsFavoriteCommand { get; }
		public ICommand ChangeIsRepeatableCommand { get; }

		private CancellationTokenSource _cancellationTokenSource;

		private SolidColorBrush _darkGoldBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdb79"));

		public AlertViewModel(Alert alert)
		{
			Alert = alert;

			ChangeAlertStatusCommand = new AsyncRelayCommand(ChangeAlertStatusAsync, AsyncRelayCommandOptions.AllowConcurrentExecutions);
			ForceStartAlertCommand = new AsyncRelayCommand(ForceStartAlertAsync, AsyncRelayCommandOptions.AllowConcurrentExecutions);

			EditButtonAlertCommand = new RelayCommand(EditAlert);
			DeleteButtonAlertCommand = new RelayCommand(RemoveAlert);

			ChangeIsFavoriteCommand = new RelayCommand(ChangeIsFavorite);
			ChangeIsRepeatableCommand = new RelayCommand(ChangeIsRepeatable);

			PropertyChanged += AlertPropertyChangedHandler;


			Status = AlertStatus.Waiting;
			ProgressBarColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffdb79"));

			UpdateProgressInPercent();
		}

		public int Id
		{
			get { return Alert.Id; }
		}

		public string Name
		{
			get { return Alert.Name; }
			set
			{
				Alert.Name = value;
				OnPropertyChanged();
			}
		}

		public DateTime Interval
		{
			get
			{
				return Alert.Interval;
			}
			set
			{
				if (Alert.Interval != value)
				{
					Alert.Interval = value;
					OnPropertyChanged();
				}
			}
		}

		public int Duration
		{
			get
			{
				return Convert.ToInt32(Interval.TimeOfDay.TotalSeconds);
			}
		}

		public DateTime? StartTime 
		{ 
			get
			{
				return Alert.StartTime;
			}
			set
			{
				if (Alert.StartTime != value)
				{
					Alert.StartTime = value;
					OnPropertyChanged();
				}
			}
		}

		private AlertStatus _status;
		public AlertStatus Status
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged();

				if (_status == AlertStatus.Waiting || _status == AlertStatus.Finished)
				{
					IsActive = false;
					ProgressBarColor = SetProgressBarColor();
				}
				else if(_status == AlertStatus.Running)
				{
					IsActive = true;
				}
			}
		}

		private int _progress;
		public int Progress
		{
			get { return _progress; }
			set
			{
				if (_progress != value)
				{
					_progress = value;
					OnPropertyChanged();

					UpdateProgressInPercent();
				}
			}
		}

		private string _progressInPercent;
		public string ProgressInPercent
		{
			get
			{
				return _progressInPercent;
			}
			set
			{
				if (_progressInPercent != value)
				{
					_progressInPercent = value;
					OnPropertyChanged();
				}		
			}
		}

		private Brush _progressBarColor;
		public Brush ProgressBarColor
		{
			get
			{
				return _progressBarColor;
			}
			set
			{
				if(_progressBarColor != value)
				{
					_progressBarColor = value;
					OnPropertyChanged();
				}	
			}
		}

		public bool IsRepeatable
		{
			get
			{
				return Alert.IsRepeatable;
			}
			set
			{
				if (Alert.IsRepeatable != value)
				{
					Alert.IsRepeatable = value;
					OnPropertyChanged();
				}	
			}
		}

		public bool IsFavorite
		{
			get
			{
				return Alert.IsFavorite;
			}
			set
			{
				if (Alert.IsFavorite != value)
				{
					Alert.IsFavorite = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isActive;
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					OnPropertyChanged();
				}		
			}
		}

		public DateTime LastModified
		{
			get
			{
				return Alert.LastModified;
			}
			set
			{
				if (Alert.LastModified != value)
				{
					Alert.LastModified = value;
					OnPropertyChanged();
				}
			}
		}

		public Brush SetProgressBarColor()
		{
			Brush color;

			if (Status == AlertStatus.Finished)
			{
				color = Brushes.Gold;
			}
			else
			{
				color = _darkGoldBrush;
			}

			return color;
		}

		private void UpdateProgressInPercent()
		{
			if (Duration <= 0) return;

			double percent = (double)Progress * 100 / Duration;

			int timeToEnd = Duration - Progress;
			TimeSpan timeLeft = TimeSpan.FromSeconds(timeToEnd);

			ProgressInPercent = $"{Math.Round(percent, 1)}% ({timeLeft:h\\h\\ m\\m\\ s\\s})";
		}

		private async Task ChangeAlertStatusAsync()
		{
			if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = new CancellationTokenSource();
			}

			if (Status == AlertStatus.Waiting)
			{
				await StartAlertAsync(_cancellationTokenSource);
			}
			else
			{
				if (Status == AlertStatus.Running)
				{
					await _cancellationTokenSource.CancelAsync();
				}	
				else if(Status == AlertStatus.Finished)
				{
					ResetAlert();

					await StartAlertAsync(_cancellationTokenSource);
				}
			}
		}

		private async Task StartAlertAsync(CancellationTokenSource cancellationTokenSource)
		{
			Progress = 0;
			Status = AlertStatus.Running;

			if (StartTime.HasValue)
			{
				CalculateProgressBasedOnStartTime();
			}

			while (Progress < Duration)
			{
				if (cancellationTokenSource.Token.IsCancellationRequested)
				{
					ResetAlert();
					return;
				}

				Progress++;

				if (Progress == Duration)
				{
					break;
				}

				await Task.Delay(1000);
			}

			if (IsRepeatable)
			{
				Status = AlertStatus.Finished;

				await Task.Delay(1000);

				ResetAlert();

				await StartAlertAsync(cancellationTokenSource);
			}
			else
			{
				Status = AlertStatus.Finished;
			}
		}

		private async Task ForceStartAlertAsync()
		{
			if (Status == AlertStatus.Running)
			{
				await StopStartedAlertAsync();

				while (Status != AlertStatus.Waiting) 
				{
					await Task.Delay(100);
				};
			}

			if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
			{
				_cancellationTokenSource?.Dispose();
				_cancellationTokenSource = new CancellationTokenSource();
			}

			await StartAlertAsync(_cancellationTokenSource);
		}

		private void ResetAlert()
		{
			Progress = 0;
			Status = AlertStatus.Waiting;
		}

		private async Task StopStartedAlertAsync()
		{
			await _cancellationTokenSource.CancelAsync();
		}

		private void CalculateProgressBasedOnStartTime()
		{
			var startTime = StartTime!.Value;
			var currentTime = DateTime.Now;
			var intervalTimeOfDay = Interval.TimeOfDay;

			var cyclesElapsed = Math.Floor((currentTime - startTime).TotalSeconds / intervalTimeOfDay.TotalSeconds);

			var newStartTime = startTime.AddSeconds(cyclesElapsed * intervalTimeOfDay.TotalSeconds);

			var progressToSet = (currentTime - newStartTime).TotalSeconds;

			if (progressToSet < 0)
			{
				progressToSet += Duration;
			}

			Progress = Convert.ToInt32(progressToSet);
		}

		private void EditAlert()
		{
			Mediator.Notify("EditAlert", this);
		}

		private void RemoveAlert()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();

			AlertManager.Instance.RemoveAlert(this);
		}

		private void ChangeIsFavorite()
		{
			IsFavorite = !IsFavorite;
		}

		private void ChangeIsRepeatable()
		{
			IsRepeatable = !IsRepeatable;
		}

		private void AlertPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Status)
				&& sender is AlertViewModel AlertViewModel
				&& AlertViewModel.Status == AlertStatus.Finished)
			{
				Mediator.Notify("ShowBalloon", new BalloonArgs("Finished", AlertViewModel.Name));
			}
		}
	}
}
