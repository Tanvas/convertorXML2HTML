using ContentLib.Core.Content.Learning;
using ContentLib.Core.Content.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// типы элементов курса
/// </summary>
public enum ItemType
{
    None,
    Course,
    Section,
    Theme,
    Step,    
    Test,
    OutTest,
    InTest
}
namespace CourceToWebUC.Model
{
    /// <summary>
    /// расширения элементов курса
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// возвращает тип элемента
        /// курса
        /// </summary>
        /// <param name="self">элемент курса</param>
        /// <returns>тип элемента курса
        /// или ItemType.None, если тип не может быть определен</returns>
        public static ItemType GetItemType(this AbstractLearningItem self)
        {
            if (self == null)
                return ItemType.None;
            if (self is LearningSection)
                return ItemType.Section;
            if (self is LearningItem)
                return ItemType.Theme;
            if (self is Test)
                return ItemType.Test;
            if (self is IncomingTest)
                return ItemType.InTest;
            if (self is OutcomingTest)
                return ItemType.OutTest;

            return ItemType.None;

        }
    }
}
