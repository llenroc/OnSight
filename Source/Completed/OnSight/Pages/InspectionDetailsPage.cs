﻿using System;

using Xamarin.Forms;

namespace OnSight
{
	public class InspectionDetailsPage : ContentPage
	{
		#region Constant Fields
		readonly InspectionDetailsViewModel _viewModel;
		readonly int _inspectionId;
		Button _viewPhotosButton;
		#endregion

		#region Constructors
		public InspectionDetailsPage(int inspectionId)
		{
			_inspectionId = inspectionId;

			_viewModel = new InspectionDetailsViewModel(inspectionId);
			BindingContext = _viewModel;

			var titleEntry = new Entry
			{
				Placeholder = "Title",
				HorizontalTextAlignment = TextAlignment.Center
			};
			titleEntry.SetBinding(Entry.TextProperty, nameof(_viewModel.TitleText));

			var notesEditor = new Editor
			{
				BackgroundColor = ColorConstants.EditorBackgroundColor
			};
			notesEditor.SetBinding(Editor.TextProperty, nameof(_viewModel.NotesText));

			_viewPhotosButton = new Button
			{
				Text = "Photos"
			};

			this.SetBinding(TitleProperty, nameof(_viewModel.TitleText));

			Padding = new Thickness(20, 10);

			var relativeLayout = new RelativeLayout();

			Func<RelativeLayout, double> getPhotosButtonHeight = (p) => _viewPhotosButton.Measure(relativeLayout.Width, relativeLayout.Height).Request.Height;
			Func<RelativeLayout, double> getPhotosButtonWidth = (p) => _viewPhotosButton.Measure(relativeLayout.Width, relativeLayout.Height).Request.Width;

			Func<RelativeLayout, double> getTitleEntryHeight = (p) => titleEntry.Measure(relativeLayout.Width, relativeLayout.Height).Request.Height;
			Func<RelativeLayout, double> getTitleEntryWidth = (p) => titleEntry.Measure(relativeLayout.Width, relativeLayout.Height).Request.Width;

			relativeLayout.Children.Add(titleEntry,
									   Constraint.Constant(0),
									   Constraint.Constant(0),
									   Constraint.RelativeToParent(parent => parent.Width));
			relativeLayout.Children.Add(notesEditor,
									   Constraint.Constant(0),
									   Constraint.RelativeToView(titleEntry, (parent, view) => view.Height + view.Y + 10),
									   Constraint.RelativeToParent(parent => parent.Width),
									   Constraint.RelativeToParent(parent => parent.Height / 2 - getTitleEntryHeight(parent) - getPhotosButtonHeight(parent) - 30));
			relativeLayout.Children.Add(_viewPhotosButton,
									   Constraint.RelativeToParent(parent => parent.Width / 2 - getPhotosButtonWidth(parent) / 2),
									   Constraint.RelativeToView(notesEditor, (parent, view) => view.Height + view.Y + 10));
			Content = new ScrollView
			{
				Content = relativeLayout
			};
		}
		#endregion

		#region Methods

		protected override void OnAppearing()
		{
			base.OnAppearing();

			_viewPhotosButton.Clicked += HandleViewPhotosButtonClicked;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			_viewModel?.SaveDataCommand?.Execute(null);

			_viewPhotosButton.Clicked -= HandleViewPhotosButtonClicked;
		}

		void HandleViewPhotosButtonClicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new PhotosListPage(_inspectionId)));
		}
		#endregion
	}
}
