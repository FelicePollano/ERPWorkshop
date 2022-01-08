﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerSearch
{
    public class ContainerPresenter : IContainerPresenter
    {
        private readonly IContainerView view;
        private readonly IContainerService containerService;
        private readonly int pageSize = 50;
        private Dictionary<int, ContainerModel[]> pages = new Dictionary<int, ContainerModel[]>();
        public ContainerPresenter(IContainerView view,IContainerService containerService)
        {
            this.view = view;
            this.containerService = containerService;
        }
        public ContainerModel GetItemAt(int index)
        {
            int page = index / pageSize;

            if (!pages.ContainsKey(page))
                pages[page]=FetchPage(index);
            var cp = pages[page];
            return cp[index % pageSize];
        }

        private ContainerModel[] FetchPage(int index)
        {
            var offset = index / pageSize * pageSize;
            return containerService.FetchPage(offset, pageSize, toSearch);
        }

        public void Initialize()
        {
            view.SetItemsCount(containerService.GetCount(toSearch));
        }
        string toSearch;
        public void Search(string searchFor)
        {
            toSearch = searchFor;
            ClearAllPages();
            Initialize();
        }

        private void ClearAllPages()
        {
            pages.Clear();
        }
    }
}
