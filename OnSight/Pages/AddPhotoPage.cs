﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace OnSight
{
	public class AddPhotoPage : ContentPage
	{
		#region Constant Fields
		AddPhotoViewModel _viewModel;
		readonly ToolbarItem _saveButton;
		readonly Entry _photoImageNameEntry;
		#endregion

		#region Constructors
		public AddPhotoPage(int inspectionId)
		{
			_viewModel = new AddPhotoViewModel(inspectionId);
			BindingContext = _viewModel;

			_saveButton = new ToolbarItem();
			_saveButton.Text = "Save";
			_saveButton.SetBinding(ToolbarItem.CommandProperty,nameof(_viewModel.SaveButtonCommand));
			ToolbarItems.Add(_saveButton);

			var cancelButton = new ToolbarItem();
			cancelButton.Text = "Cancel";
			cancelButton.Clicked += (sender, e) => DismissPage();
			ToolbarItems.Add(cancelButton);


			var photoImage = new Image();
			photoImage.SetBinding(Image.SourceProperty, nameof(_viewModel.PhotoImageSource));

			_photoImageNameEntry = new Entry();
			_photoImageNameEntry.SetBinding(Entry.TextProperty, nameof(_viewModel.PhotoImageNameText));

			var takePhotoButton = new Button
			{
				Text = "Take Photo"
			};
			takePhotoButton.SetBinding(Button.CommandProperty, nameof(_viewModel.TakePhotoButtonCommand));

			Padding = new Thickness(20, 10);

			this.SetBinding(TitleProperty, nameof(_viewModel.PhotoImageNameText));

			Content = new StackLayout
			{
				Children = {
					photoImage,
					_photoImageNameEntry,
					takePhotoButton
				}
			};
		}
		#endregion

		#region Methods
		protected override void OnAppearing()
		{
			base.OnAppearing();

			_viewModel.PhotoSavedToDatabaseCompleted += HandlePhotoSavedToDatabaseCompleted;
			_viewModel.DisplayNoCameraAvailableAlert += HandleDisplayNoCameraAvailableAlert;
			_viewModel.DuplicateImageNameDetected += HandleDuplicateImageNameDetected;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			_viewModel.PhotoSavedToDatabaseCompleted -= HandlePhotoSavedToDatabaseCompleted;
			_viewModel.DisplayNoCameraAvailableAlert -= HandleDisplayNoCameraAvailableAlert;
			_viewModel.DuplicateImageNameDetected -= HandleDuplicateImageNameDetected;
		}

		void HandleDuplicateImageNameDetected(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => DisplayAlert("Error: Duplicate Photo Name", $"A Photo Entitled {_photoImageNameEntry.Text} Already Exists", "Ok"));
		}

		void HandleDisplayNoCameraAvailableAlert(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => DisplayAlert("Error", "Camera Unavailable", "Ok"));
		}

		void HandlePhotoSavedToDatabaseCompleted(object sender, EventArgs e)
		{
			DismissPage();
		}

		void DismissPage()
		{
			Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
		}
		#endregion
	}
}