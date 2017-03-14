namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PoiInfoList
    {
        public base_info base_info { get; set; }
    }

    public class base_info
    {
        public string business_name { get; set; }

        public string branch_name { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string district { get; set; }

        public string address { get; set; }

        public string telephone { get; set; }

        public List<string> categories { get; set; }

        public int offset_type { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string latitude { get; set; }

        public List<Photo> photo_list { get; set; }

        public string special { get; set; }

        public string open_time { get; set; }

        public int avg_price { get; set; }

        public string sid { get; set; }

        public string introduction { get; set; }

        public string recommend { get; set; }
        /// <summary>
        /// 门店id
        /// </summary>
        public string poi_id { get; set; }
        /// <summary>
        /// 门店是否可用状态。1 表示系统错误、2 表示审核中、3 审核通过、4 审核驳回。当该字段为1、2、4 状态时，poi_id 为空
        /// </summary>
        public int available_state { get; set; }
        /// <summary>
        /// 扩展字段是否正在更新中。1 表示扩展字段正在更新中，尚未生效，不允许再次更新； 0 表示扩展字段没有在更新中或更新已生效，可以再次更新
        /// </summary>
        public int update_status { get; set; }
    }

    public class Photo
    {
        public string photo_url { get; set; }
    }

}

