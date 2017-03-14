namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WKMInfo
    {
        private Guid wkmId;
        public Guid WKMId
        {
            get{return this.wkmId;}
            set{ this.wkmId = value;}
        }

        private string titleDescription;
        public string TitleDescription
        {
            get { return this.titleDescription; }
            set { this.titleDescription = value; }
        }

        private string shareTitle;
        public string ShareTitle
        {
            get { return this.shareTitle; }
            set { this.shareTitle = value; }
        }

        private string shareDescription;
        public string ShareDescription
        {
            get { return this.shareDescription; }
            set { this.shareDescription = value; }
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        private WKMSubjectInfo subjectInfo;
        public WKMSubjectInfo SubjectInfo
        {
            get
            {
                if (this.subjectInfo == null)
                {
                    this.subjectInfo = new WKMSubjectInfo();
                }
                return this.subjectInfo;
            }
            set { this.subjectInfo = value; }
        }

        private IList<WKMOptionInfo> optionsInfo;
        public IList<WKMOptionInfo> OptionsInfo
        {
            get
            {
                if (this.optionsInfo == null)
                {
                    this.optionsInfo = new List<WKMOptionInfo>();
                }
                return this.optionsInfo;
            }
            set { this.optionsInfo = value; }
        }

    }
}

