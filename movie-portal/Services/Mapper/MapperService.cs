using System;
using System.Globalization;
using movie_portal.Models.Media;

namespace movie_portal.Services.Mapper
{
	public class MapperService : AutoMapper.Profile
	{
		public MapperService()
		{
			this.CreateMap<Movie, MediaDTO>()
				.ForMember(m => m.Description, option => option.MapFrom(m => m.Description))
				.ForMember(m => m.Director, option => option.MapFrom(m => m.Director))
				.ForMember(m => m.Genres, option => option.Ignore())
				.ForMember(m => m.GenresId, option => option.Ignore())
				.ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
				.ForMember(m => m.ImageFile, option => option.Ignore())
				.ForMember(m => m.ImageFileName, option => option.MapFrom(m => m.ImageFileName))
				.ForMember(m => m.InsertDateTime, option => option.MapFrom(m => m.InsertDateTime))
				.ForMember(m => m.InsertUserId, option => option.MapFrom(m => m.InsertUserId))
				.ForMember(m => m.User, option => option.MapFrom(m => m.User))
				.ForMember(m => m.IsDelete, option => option.MapFrom(m => m.IsDelete))
				.ForMember(m => m.MediaFiles, option => option.Ignore())
				.ForMember(m => m.MediaFilesName, option => option.Ignore())
				.ForMember(m => m.ReleaseYear, option => option.MapFrom(m => m.ReleaseYear))
				.ForMember(m => m.Title, option => option.MapFrom(m => m.Title))
				.ForMember(m => m.UpdateDateTime, option => option.MapFrom(m => m.UpdateDateTime))
				.ForMember(m => m.Views, option => option.MapFrom(m => m.Views))
				.ReverseMap()
				.ForMember(m => m.Comments, option => option.Ignore())
				.ForMember(m => m.Description, option => option.MapFrom(m => m.Description))
				.ForMember(m => m.Genres, option => option.Ignore())
				.ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
				.ForMember(m => m.ImageFileName, option => option.MapFrom(m => m.ImageFileName))
				.ForMember(m => m.InsertDateTime, option => option.MapFrom(m => m.InsertDateTime))
				.ForMember(m => m.InsertUserId, option => option.MapFrom(m => m.InsertUserId))
				.ForMember(m => m.User, option => option.MapFrom(m => m.User))
				.ForMember(m => m.IsDelete, option => option.MapFrom(m => m.IsDelete))
				.ForMember(m => m.MediaFiles, option => option.Ignore())
				.ForMember(m => m.ReleaseYear, option => option.MapFrom(m => m.ReleaseYear))
				.ForMember(m => m.Title, option => option.MapFrom(m => m.Title))
				.ForMember(m => m.UpdateDateTime, option => option.MapFrom(m => m.UpdateDateTime))
				.ForMember(m => m.User, option => option.Ignore())
				.ForMember(m => m.Views, option => option.MapFrom(m => m.Views));

			this.CreateMap<Genre, GenreDTO>()
				.ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
				.ForMember(m => m.Name, option => option.MapFrom(m => m.Name))
				.ReverseMap()
				.ForMember(m => m.Id, option => option.MapFrom(m => m.Id))
				.ForMember(m => m.Name, option => option.MapFrom(m => m.Name));
		}
	}
}