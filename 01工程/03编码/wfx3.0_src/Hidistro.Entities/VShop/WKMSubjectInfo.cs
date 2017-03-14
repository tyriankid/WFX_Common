namespace Hidistro.Entities.VShop
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WKMSubjectInfo
    {
        private IList<Guid> wkmSubjectId;
        public IList<Guid> WKMSubjectId
        {
            get
            {
                if (this.wkmSubjectId == null)
                {
                    this.wkmSubjectId = new List<Guid>();
                }
                return this.wkmSubjectId;
            }
        }

        private IList<string> subjectContent;
        public IList<string> SubjectContent
        {
            get
            {
                if (this.subjectContent == null)
                {
                    this.subjectContent = new List<string>();
                }
                return this.subjectContent;
            }
        }


        private IList<Guid> activityId;
        public IList<Guid> ActivityId
        {
            get
            {
                if (this.activityId == null)
                {
                    this.activityId = new List<Guid>();
                }
                return this.activityId;
            }
        }

        private IList<string> imgUrl;
        public IList<string> ImgUrl
        {
            get
            {
                if (this.imgUrl == null)
                {
                    this.imgUrl = new List<string>();
                }
                return this.imgUrl;
            }
        }
    }
}

