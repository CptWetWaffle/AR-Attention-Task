using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class LoadManager
    {
        public struct Category
        {
            public string Name { get; private set; }
            public IList<string> ImageList { get; private set; }

            public Category(string name) : this()
            {
                this.Name = name;
                this.ImageList = new List<string>();
            }
        }

        private static LoadManager _instance;


        public static LoadManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoadManager();

                return _instance;
            }
        }

        private IList<Category> CategoryList { get; set; }

        private LoadManager()
        {
            CategoryList = new List<Category>();
            var categoryNames = new List<string>
            {
                "Cat_Letters",
                "Cat_LettersC",
                "Cat_Numbers",
                "Cat_NumbersC",
                "Cat_Symbols",
                "Cat_SymbolsC",
                "Cat_Toulouse",
                "IA",
                "Tester1"
            };

            foreach (var categoryName in categoryNames)
            {
                var category = new Category(categoryName);
                //yield return null;
                var resources = Resources.LoadAll("Categories/" + categoryName);
                foreach (var resource in resources)
                    category.ImageList.Add(resource.name);

                CategoryList.Add(category);
            }
        }

        private IList<string> GetList(string name)
        {
            foreach (var category in CategoryList)
            {
                if (category.Name == name)
                    return category.ImageList;
            }
            return null;
        }

        public Texture RandomTexture()
        {
            var images = GetList("Cat_Symbols");
            var rand = UnityEngine.Random.Range(0, images.Count - 1);
            return Resources.Load("Categories/Cat_Symbols/" + images[rand]) as Texture;
        }

        public static void Initialize()
        {
            Debug.Log(LoadManager.Instance.CategoryList.ToString());
        }
    }
}
