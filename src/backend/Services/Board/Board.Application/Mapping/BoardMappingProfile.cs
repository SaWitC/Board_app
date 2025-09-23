using AutoMapper;
using Board.Application.DTOs;
using Board.Domain.Contracts.Pagination;
using Board.Application.DTOs.BoardItems;
using Board.Domain.Entities;

namespace Board.Application.Mapping;

public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

        CreateMap<Domain.Entities.Board, BoardDto>()
            .ForMember(d => d.BoardUsers, o => o.MapFrom(s => s.BoardUsers))
            .ForMember(d => d.BoardColumns, o => o.MapFrom(s => s.BoardColumns))
            .ReverseMap();

        CreateMap<BoardUser, BoardUserDto>()
            .ReverseMap()
            .ForMember(d => d.BoardId, o => o.Ignore());

        CreateMap<BoardColumn, BoardColumnDto>()
            .ReverseMap()
            .ForMember(d => d.Elements, o => o.Ignore());

        CreateMap<BoardItem, BoardItemDto>()
            .ReverseMap()
            .ForMember(d => d.Tags, o => o.Ignore())
            .ForMember(d => d.BoardColumn, o => o.Ignore())
            .ForMember(d => d.SubItems, o => o.Ignore());
    }
}
