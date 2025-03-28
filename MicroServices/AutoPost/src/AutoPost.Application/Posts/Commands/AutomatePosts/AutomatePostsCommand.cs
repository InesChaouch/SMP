using MediatR;
using SMP.Application.Common.Interfaces;
using SMP.Application.Interfaces;
using SMP.Application.PostContent.Commands.GenerateContent;
using SMP.Application.PostContent.Commands.CreatePost;
using SMP.Application.TodoItems.Commands.CreateTodoItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Dtos;
using SMP.Domain.Enums;
using SMP.Application.Posts.Commands.AutomatePosts;

namespace SMP.Application.PostContent.Commands.AutomatePosts
{
    public class AutomatePostsCommand : IRequest
    {
        public required string Category { get; set; }
        public required string Topic { get; set; }
        public required List<PlatformConfig> PlatformConfigs { get; set; }
    }

}

   
