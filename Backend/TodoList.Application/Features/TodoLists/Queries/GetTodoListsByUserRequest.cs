﻿using AutoMapper;
using MediatR;
using TodoList.Applications.Dtos;
using TodoList.Applications.Interfaces.Repositories;
using TodoList.Applications.Interfaces.Services;
using TodoList.Applications.Specifications;
using TodoList.Entities.Entities;

namespace TodoList.Applications.Features.TodoLists.Queries
{
    public class GetTodoListsByUserRequest : IRequest<IEnumerable<TodoTaskDto>>
    {
        public GetTodoListsByUserRequest(int pageSize, int pageIndex)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetTodoListByUserHandler : IRequestHandler<GetTodoListsByUserRequest, IEnumerable<TodoTaskDto>>
    {
        private readonly IAsyncRepository<TodoTask, Guid> _todoTaskRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetTodoListByUserHandler(IAsyncRepository<TodoTask, Guid> todoTaskRepo, IMapper mapper, IUserService userService)
        {
            _todoTaskRepo = todoTaskRepo;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<IEnumerable<TodoTaskDto>> Handle(GetTodoListsByUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _todoTaskRepo.GetListItemBySpecificationAsync(new TodoTaskSpecification(Guid.Parse(_userService.UserId)),request.PageSize, request.PageIndex);
            return _mapper.Map<IEnumerable<TodoTaskDto>>(result);
        }
    }
}
