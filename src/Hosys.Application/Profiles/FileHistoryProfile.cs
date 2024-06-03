using AutoMapper;
using Hosys.Application.Data.Outputs.File;
using Hosys.Domain.Models.Files;

namespace Hosys.Application.Profiles
{
    public class FileHistoryProfile : Profile
    {
        public FileHistoryProfile()
        {
            CreateMap<CreateFileHistoryDTO, FileHistory>();
            CreateMap<UpdateFileHistoryDTO, FileHistory>();
            CreateMap<FileHistory, ReadFileHistoryDTO>();
        }
    }
}