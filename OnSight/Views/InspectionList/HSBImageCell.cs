﻿using Xamarin.Forms;

namespace OnSight
{
	public class HSBImageCell : ImageCell
	{
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var item = BindingContext as InspectionModel;

			Text = item?.InspectionTitle;
			ImageSource = Device.OnPlatform("MunichREIcon", "MunichREIcon", "Assets/MunichREIcon.png");

        }
	}
}
