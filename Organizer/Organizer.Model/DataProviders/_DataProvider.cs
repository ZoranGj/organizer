﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataProviders {
	public class DataProvider<T> where T : class {
		private DataContext _dbContext;
		private DbSet<T> _dbSet;

		public DataProvider() {
			_dbContext = new DataContext();
			_dbSet = _dbContext.Set<T>();
		}

		public IEnumerable<T> GetAll() {
			return _dbSet.ToList();
		}

		public T GetById(object Id) {
			return _dbSet.Find(Id);
		}

		public void Insert(T obj) {
			_dbSet.Add(obj);
		}

		public void Update(T obj) {
			_dbContext.Entry(obj).State = EntityState.Modified;
		}

		public void Delete(object Id) {
			T getObjById = _dbSet.Find(Id);
			_dbSet.Remove(getObjById);
		}

		public void Save() {
			_dbContext.SaveChanges();
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				if (this._dbContext != null) {
					this._dbContext.Dispose();
					this._dbContext = null;
				}
			}
		}
	}
}
