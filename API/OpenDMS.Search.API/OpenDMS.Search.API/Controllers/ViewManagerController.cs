using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.DTOs;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Enumerators;

namespace OpenDMS.DocumentManager.Controllers
{
    [ApiController]
    [Route("/api/ui/[controller]")]
    public class ViewManagerController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ILoggedUserProfile userContext;
        private readonly IViewManager viewManager;

        public ViewManagerController(ILogger<ViewManagerController> logger, ILoggedUserProfile userContext, IViewManager viewManager)
        {
            this.logger = logger;
            this.userContext = userContext;
            this.viewManager = viewManager;
        }

        /// <summary>
        /// Recuperla le informazioni relative alla vista indicata
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        [HttpGet("{viewId}")]
        public async Task<ViewProperties> Get(string viewId)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.Get(viewId, userId);
        }

        [HttpGet("reset/{viewId}")]
        public async Task<ViewProperties> Reset(string viewId)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.Reset(viewId, userId);
        }

        [HttpPost("change/width/{viewId}/{columnId}/{width}")]
        public async Task<bool> ChangeWidth(string viewId, string columnId, string width)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.ChangeWidth(viewId, userId, columnId, width);
        }

        [HttpPost("change/visibility/{viewId}/{columnId}/{show}")]
        public async Task<bool> ChangeVisibility(string viewId, string columnId, bool show)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.ChangeVisibility(viewId, userId, columnId, show);
        }

        [HttpPost( "change/sorting/{viewId}/{columnId}/{sortingType}")]
        public async Task<bool> ChangeSorting(string viewId, string columnId, SortingType sortingType)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.ChangeSorting(viewId, userId, columnId, sortingType);
        }

        [HttpPost("change/aggregation/{viewId}/{columnId}/{aggregateType}")]
        public async Task<bool> ChangeAggregation(string viewId, string columnId, AggregateType aggregateType)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.ChangeAggregation(viewId, userId, columnId, aggregateType);
        }

        [HttpPost("change/column/{viewId}/{fromIndex}/{toIndex}")]
        public async Task<bool> MoveColumn(string viewId, string fromColumnId, string toColumnId)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.MoveColumn(viewId,userId, fromColumnId, toColumnId);
        }




        [HttpPost("change/groupBy/{viewId}/{columnId}")]
        public async Task<bool> GroupBy(string viewId, string columnId)
        {
            var u = userContext.Get();
            string userId = u.userId;
            return await viewManager.GroupBy(viewId,userId, columnId);
        }
    }
}
