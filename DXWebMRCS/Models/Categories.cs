using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DXWebMRCS.Models
{
    public abstract class ItemsData : IHierarchicalEnumerable, IEnumerable
    {
        public ItemsData()
        {
        }

        public abstract IEnumerable Data { get; }

        public IEnumerator GetEnumerator()
        {
            return Data.GetEnumerator();
        }
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return (IHierarchyData)enumeratedItem;
        }
    }

    public class ItemData : IHierarchyData
    {
        public string Text { get; protected set; }
        public string NavigateUrl { get; protected set; }

        public ItemData(string text, string navigateUrl)
        {
            Text = text;
            NavigateUrl = navigateUrl;
        }

        // IHierarchyData
        bool IHierarchyData.HasChildren
        {
            get { return HasChildren(); }
        }
        object IHierarchyData.Item
        {
            get { return this; }
        }
        string IHierarchyData.Path
        {
            get { return NavigateUrl; }
        }
        string IHierarchyData.Type
        {
            get { return GetType().ToString(); }
        }
        IHierarchicalEnumerable IHierarchyData.GetChildren()
        {
            return CreateChildren();
        }
        IHierarchyData IHierarchyData.GetParent()
        {
            return null;
        }

        protected virtual bool HasChildren()
        {
            return false;
        }
        protected virtual IHierarchicalEnumerable CreateChildren()
        {
            return null;
        }
    }

    public class CategoriesData : ItemsData
    {
        UsersContext DB = new UsersContext();
        public override IEnumerable Data
        {
            get { return DB.Menus.ToList().Select(c => new CategoryData(c)); }
        }
    }

    public class CategoryData : ItemData
    {
        public Menu Menu { get; protected set; }

        public CategoryData(Menu menu)
            : base(menu.NameMon, "?MenuID =" + menu.MenuID)
        {
            Menu = menu;
        }

        protected override bool HasChildren()
        {
            return false;
        }



    }
}