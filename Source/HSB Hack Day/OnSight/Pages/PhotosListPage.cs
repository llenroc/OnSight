﻿using System;

using Xamarin.Forms;

namespace OnSight
{
    public class PhotosListPage : ContentPage
    {
        #region Constant Fields
        readonly int _inspectionId;
        readonly ListView _photosListView;
        readonly PhotosListViewModel _viewModel;
        readonly ToolbarItem _addPhotoToolbarItem;
        #endregion

        #region Constructors
        public PhotosListPage(int inspectionId)
        {
            _inspectionId = inspectionId;

            _viewModel = new PhotosListViewModel(inspectionId);
            BindingContext = _viewModel;

            Title = "Photos";
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _addPhotoToolbarItem.Clicked += HandleAddPhotoToolbarItemClicked;
            _viewModel.PullToRefreshCompleted += HandlePullToRefreshCompleted;

            _photosListView.BeginRefresh();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _addPhotoToolbarItem.Clicked -= HandleAddPhotoToolbarItemClicked;
            _viewModel.PullToRefreshCompleted -= HandlePullToRefreshCompleted;
        }

        void HandlePullToRefreshCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(_photosListView.EndRefresh);
        }

        void HandleAddPhotoToolbarItemClicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var addPhotoNavigationPage = new NavigationPage(new AddPhotoPage(_inspectionId))
                {
                    BarBackgroundColor = ColorConstants.NavigationBarBackgroundColor,
                    BarTextColor = ColorConstants.NavigationBarTextColor
                };

                await Navigation.PushModalAsync(addPhotoNavigationPage);

                _photosListView.SelectedItem = null;
            });
        }
        #endregion
    }
}
