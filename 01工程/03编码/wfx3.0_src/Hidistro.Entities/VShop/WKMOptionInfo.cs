namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WKMOptionInfo
    {
        /// <summary>
        /// 选项id
        /// </summary>
        private IList<Guid> wkmOptionId;
        public IList<Guid> WKMOptionId
        {
            get
            {
                if (this.wkmOptionId == null)
                {
                    this.wkmOptionId = new List<Guid>();
                }
                return this.wkmOptionId;
            }
        }

        /// <summary>
        /// 选项内容
        /// </summary>
        private IList<string> optionContent;
        public IList<string> OptionContent
        {
            get
            {
                if (this.optionContent == null)
                {
                    this.optionContent = new List<string>();
                }
                return this.optionContent;
            }
        }

        /// <summary>
        /// 题目id
        /// </summary>
        private IList<Guid> titleId;
        public IList<Guid> TitleId
        {
            get
            {
                if (this.titleId == null)
                {
                    this.titleId = new List<Guid>();
                }
                return this.titleId;
            }
        }
    }
}

