using Board.Application.Abstractions.Repositories;
using Board.Application.DTOs;
using Board.Domain.Contracts.Enums;
using Board.Domain.Contracts.Security;
using Board.Domain.Entities;
using Board.Domain.Security;
using FastEndpoints;

namespace Board.Api.Features.Board.UpdateBoardUsers;

public class UpdateBoardUsersEndpoint : Endpoint<UpdateBoardUsersRequest>
{
    private readonly IBoardUserRepository _boardUserRepository;

    public UpdateBoardUsersEndpoint(IBoardUserRepository boardUserRepository)
    {
        _boardUserRepository = boardUserRepository;
    }

    public override void Configure()
    {
        Put("/api/board-users/{boardId}");
        Policies(Auth.BuildPermissionPolicy(Permission.ManageBoard, Context.Board, "boardId"));
    }

    public override async Task HandleAsync(UpdateBoardUsersRequest request,  CancellationToken cancellationToken)
    {
        Guid boardId = Route<Guid>("boardId");

        var boardUsers = request.BoardUsers.Where(x => x.Role == UserAccessEnum.BoardUser).ToArray();
        var boardAdmins = request.BoardUsers.Where(x => x.Role == UserAccessEnum.BoardMGR).ToArray();

        await UpdateUsersAsync(boardUsers, UserAccessEnum.BoardUser, boardId, cancellationToken);
        await UpdateUsersAsync(boardAdmins, UserAccessEnum.BoardMGR, boardId, cancellationToken);

        await Send.OkAsync(cancellationToken);
    }

    private async Task UpdateUsersAsync(BoardUserDto[] users, UserAccessEnum role, Guid boardId, CancellationToken cancellationToken)
    {
        var boardUsers = await _boardUserRepository.GetAllAsync(
            x => x.BoardId == boardId && x.Role == role,
            cancellationToken
        );

        var requestUserEmails = users.Select(x => x.Email)
            .ToArray();

        await DeleteUsersAsync(boardUsers, requestUserEmails, cancellationToken);
        await CreateUsersAsync(boardUsers, users, boardId, cancellationToken);
        await UpdateUsersAsync(boardUsers, users, requestUserEmails, cancellationToken);
    }

    private async ValueTask DeleteUsersAsync(IList<BoardUser> boardUsers, string[] requestUserEmails, CancellationToken cancellationToken)
    {
        var usersToDelete = boardUsers.Where(
            x => !requestUserEmails.Contains(x.Email)
        )
        .ToArray();

        if (usersToDelete.Any())
        {
            return;
        }

        await _boardUserRepository.DeleteRangeAsync(
            usersToDelete,
            cancellationToken
        );
    }

    private async ValueTask CreateUsersAsync(IList<BoardUser> boardUsers, BoardUserDto[] users, Guid boardId, CancellationToken cancellationToken)
    {
        var originUserEmails = boardUsers.Select(x => x.Email)
            .ToArray();

        var usersToCreate = users.Where(x => !originUserEmails.Contains(x.Email))
            .Select(x => new BoardUser
            {
                BoardId = boardId,
                Email = x.Email,
                Role = x.Role,
            })
            .ToArray();

        if (!usersToCreate.Any())
        {
            return;
        }

        await _boardUserRepository.AddRangeAsync(
            usersToCreate,
            cancellationToken
        );
    }

    private async ValueTask UpdateUsersAsync(IList<BoardUser> boardUsers, BoardUserDto[] users, string[] requestUserEmails, CancellationToken cancellationToken)
    {
        var usersToUpdate = boardUsers.Where(x => requestUserEmails.Contains(x.Email))
            .ToArray();

        if (!usersToUpdate.Any())
        {
            return;
        }

        foreach (var user in usersToUpdate)
        {
            var updatedUserInfo = users.First(x => x.Email == user.Email);

            user.Email = updatedUserInfo.Email;
            user.Role = updatedUserInfo.Role;
        }

        await _boardUserRepository.UpdateRangeAsync(
            usersToUpdate,
            cancellationToken
        );
    }
}
