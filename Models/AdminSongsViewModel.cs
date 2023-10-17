﻿using MusicApp.Entity;
using System.ComponentModel.DataAnnotations;

namespace MusicApp.Models
{
	public class AdminSongsViewModel
	{
		public List<AdminSongViewModel> Songs { get; set; }
	}
	public class AdminSongViewModel
	{
		public int SongId { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? ImageUrl { get; set; }
		public List<Genre> Genres { get; set; } //many to many relation with Genre
		public int? ReleaseDate { get; set; }
	}
	public class AdminCreateSongModel
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public int[] GenreIds { get; set; }
		public int? ReleaseDate { get; set; } 

	}
	public class AdminEditSongModel
	{
		public int SongId { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public int[] GenreIds { get; set; }
		public int? ReleaseDate { get; set; }
		public string? ImageUrl { get; set; }
	}
}