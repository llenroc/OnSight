﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;
using Xamarin.Forms;

namespace OnSight
{
	public static class InspectionModelDatabase
	{
		#region Constant Fields
		static readonly SQLiteAsyncConnection _databaseConnection = DependencyService.Get<ISQLite>()?.GetConnection();
		#endregion

		#region Fields
		static bool _isDatabaseInitialized;
		#endregion

		#region Methods
		public static async Task InitializeDatabase()
		{
			await _databaseConnection.CreateTablesAsync<InspectionModel, PhotoModel>();
			_isDatabaseInitialized = true;
		}

		public static async Task<List<InspectionModel>> GetAllInspectionModelsAsync()
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();

			return await _databaseConnection?.Table<InspectionModel>()?.ToListAsync();
		}

		public static async Task<InspectionModel> GetInspectionModelAsync(int id)
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();

			return await _databaseConnection?.Table<InspectionModel>()?.Where(x => x.Id.Equals(id))?.FirstOrDefaultAsync() ?? null;
		}

		public static async Task<int> SaveInspectionModelAsync(InspectionModel inspectionModel)
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();

			if (await GetInspectionModelAsync(inspectionModel.Id) != null)
			{
				await _databaseConnection?.UpdateAsync(inspectionModel);
				return inspectionModel.Id;
			}

			return await _databaseConnection?.InsertAsync(inspectionModel);
		}

		public static async Task<int> SavePhoto(PhotoModel photoModel)
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();
			
			if(await GetPhoto(photoModel.Id)!= null)
			{
				await _databaseConnection?.UpdateAsync(photoModel);
				return photoModel.Id;
			}

			return await _databaseConnection?.InsertAsync(photoModel);
		}

		public static async Task<PhotoModel> GetPhoto(int id)
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();
			
			return await _databaseConnection?.Table<PhotoModel>()?.Where(x => x.Id.Equals(id))?.FirstOrDefaultAsync();
		}

		public static async Task<List<PhotoModel>> GetAllPhotosForInspection(int inspectionModelId)
		{
			if (!_isDatabaseInitialized)
				await InitializeDatabase();

			return await _databaseConnection?.Table<PhotoModel>().Where(x=>x.InspectionModelId.Equals(inspectionModelId)).ToListAsync();
		}
		#endregion
	}
}

