using CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PagedList;

namespace CodeFirst.ViewModels
{
    public class ListViewModel
    {
        public List<int> NoOfSubscribers { get; set; }
        public List<NewList> lists { get; set; }

        public ListViewModel()
        {
            NoOfSubscribers = new List<int>();
            lists = new List<NewList>();
        }
    }

    public class SubscribersViewModel
    {
        public int? ListID { get; set; }
        public List<Subscriber> SubscribersToList { get; set; }

        public DataTable dataTable { get; set; }

        public SubscribersViewModel()
        {
            SubscribersToList = new List<Subscriber>();
        }
    }
}