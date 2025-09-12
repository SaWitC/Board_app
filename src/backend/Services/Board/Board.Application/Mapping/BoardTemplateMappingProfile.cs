using AutoMapper;
using Board.Application.DTOs;
using Board.Domain.Entities;

namespace Board.Application.Mapping;

public class BoardTemplateMappingProfile : Profile
{
    public BoardTemplateMappingProfile()
    {
        CreateMap<BoardTemplate, BoardTemplateDto>()
            .ReverseMap();

    }
}
